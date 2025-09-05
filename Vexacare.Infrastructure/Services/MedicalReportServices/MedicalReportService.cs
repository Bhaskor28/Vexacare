using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vexacare.Application.MedicalReport;
using Vexacare.Domain.Entities.MedicalReport;
using System.Net.Http.Headers;

namespace Vexacare.Infrastructure.Services.MedicalReportServices
{
    public class MedicalReportService : IMedicalReportService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MedicalReportService> _logger;

        public MedicalReportService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<MedicalReportService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;

            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        public async Task<MedicalReportAnalysisResult> AnalyzeMedicalReportAsync(IFormFile medicalReport, string userAllInfo)
        {
            try
            {
                var apiUrl = _configuration["ApiSettings:BaseUrl"] + "/api/Home/analyze-medical-report";

                _logger.LogInformation("Sending medical report to API: {FileName}", medicalReport.FileName);

                using var content = new MultipartFormDataContent();

                // Add medical report file
                using var fileStream = medicalReport.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(medicalReport.ContentType);
                content.Add(fileContent, "medicalReport", medicalReport.FileName);


                // Add additional parameters
                content.Add(new StringContent(userAllInfo), "userAllInfo");


                // Send POST request to the API
                var response = await _httpClient.PostAsync(apiUrl, content);

                // Read the response content first
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response: {ResponseContent}", responseContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Medical report analysis completed");

                    // Check if response is empty or invalid
                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        return new MedicalReportAnalysisResult
                        {
                            Success = false,
                            ErrorMessage = "API returned empty response"
                        };
                    }
                    string analysisJson = null;
                    try
                    {
                        var result = JsonSerializer.Deserialize<ApiResponse>(responseContent,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        // Extract JSON from the Markdown code blocks
                        analysisJson = ExtractJsonFromMarkdown(result.Analysis.ToString());

                        if (string.IsNullOrEmpty(analysisJson))
                        {
                            return new MedicalReportAnalysisResult
                            {
                                Success = false,
                                ErrorMessage = "Failed to extract JSON from API response"
                            };
                        }

                        var analysis = JsonSerializer.Deserialize<MedicalReportAnalysis>(
                            analysisJson,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        return new MedicalReportAnalysisResult
                        {
                            Success = true,
                            FileName = result.FileName,
                            ExtractedTextLength = result.ExtractedTextLength,
                            Analysis = analysis,
                            ProcessedAt = DateTime.UtcNow
                        };
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "Failed to parse API response JSON");
                        _logger.LogError("Raw JSON that failed to parse: {AnalysisJson}", analysisJson);  //if unable to parse JSON
                        return new MedicalReportAnalysisResult
                        {
                            Success = false,
                            ErrorMessage = $"Invalid JSON response from API: {jsonEx.Message}"
                        };
                    }
                }

                _logger.LogWarning("API error: {StatusCode}, {Content}", response.StatusCode, responseContent);

                return new MedicalReportAnalysisResult
                {
                    Success = false,
                    ErrorMessage = $"API error: {response.StatusCode} - {responseContent}"
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed");
                return new MedicalReportAnalysisResult
                {
                    Success = false,
                    ErrorMessage = $"Cannot connect to API: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing medical report");
                return new MedicalReportAnalysisResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        // Helper method to extract JSON from Markdown code blocks
        private string ExtractJsonFromMarkdown(string markdownResponse)
        {
            try
            {
                // Remove the ```json and ``` markers
                string json = markdownResponse;

                // Remove starting ```json
                if (json.StartsWith("```json"))
                {
                    json = json.Substring(7);
                }
                else if (json.StartsWith("```"))
                {
                    json = json.Substring(3);
                }

                // Remove ending ```
                if (json.EndsWith("```"))
                {
                    json = json.Substring(0, json.Length - 3);
                }

                // Trim any whitespace
                json = json.Trim();

                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to extract JSON from Markdown");
                return null;
            }
        }

        public async Task<bool> TestApiConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_configuration["ApiSettings:BaseUrl"] + "/api/Home/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private class ApiResponse
        {
            public bool Success { get; set; }
            public string FileName { get; set; }
            public int ExtractedTextLength { get; set; }
            public object Analysis { get; set; } // Change to Object
        }
    }
}

