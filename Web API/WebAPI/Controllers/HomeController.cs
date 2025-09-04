using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using System.Text;
using UglyToad.PdfPig;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        
        private readonly ChatClient _chatClient;

        public HomeController(ChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        [HttpPost("analyze-medical-report")]
        public async Task<IActionResult> AnalyzeMedicalReport(IFormFile medicalReport)
        {
            // Validate the file
            if (medicalReport == null || medicalReport.Length == 0)
                return BadRequest("Please upload a medical report PDF file.");

            if (!medicalReport.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only PDF files are supported.");

            if (medicalReport.Length > 10 * 1024 * 1024) // 10MB limit
                return BadRequest("File size must be less than 10MB.");

            string extractedText;
            try
            {
                // Extract text from PDF
                extractedText = await ExtractTextFromPdf(medicalReport);

                if (string.IsNullOrWhiteSpace(extractedText))
                    return BadRequest("No text could be extracted from the PDF.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to process PDF: {ex.Message}");
            }

            try
            {
                // Create structured prompt for medical report analysis
                string medicalPrompt = CreateMedicalAnalysisPrompt(extractedText);

                // Create chat messages
                var messages = new List<ChatMessage>
            {
                new UserChatMessage(medicalPrompt)
            };

                // Get analysis from OpenAI
                var completionResult = await _chatClient.CompleteChatAsync(messages);

                if (completionResult.Value.Content.Count == 0)
                {
                    return StatusCode(500, "No response generated from the AI model.");
                }

                string analysisResult = completionResult.Value.Content[0].Text;

                return Ok(new
                {
                    success = true,
                    fileName = medicalReport.FileName,
                    extractedTextLength = extractedText.Length,
                    analysis = analysisResult
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = "Failed to analyze medical report",
                    details = ex.Message
                });
            }
        }

        private async Task<string> ExtractTextFromPdf(IFormFile pdfFile)
        {
            using var memoryStream = new MemoryStream();
            await pdfFile.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var stringBuilder = new StringBuilder();

            using (var document = PdfDocument.Open(memoryStream))
            {
                foreach (var page in document.GetPages())
                {
                    stringBuilder.AppendLine(page.Text);
                }
            }

            return stringBuilder.ToString();
        }

        private string CreateMedicalAnalysisPrompt(string medicalText)
        {
            return @"
            You are a medical report analysis assistant. Analyze the following medical report and provide a structured response.

            **INPUT FORMAT:**
            - Medical report text extracted from PDF
            - May contain patient information, test results, diagnoses, medications, etc.

            **REQUIRED OUTPUT FORMAT (JSON structure):**
            {
            ""patientInformation"": {
            ""age"": ""extracted or estimated"",
            ""gender"": ""extracted if available"",
            ""relevantHistory"": ""summary""
            },
            ""keyFindings"": [
            ""list of main medical findings""
            ],
            ""diagnoses"": [
            ""list of diagnoses if mentioned""
            ],
            ""medications"": [
            ""list of medications prescribed""
            ],
            ""testResults"": {
            ""abnormalResults"": [""list""],
            ""normalResults"": [""list""]
            },
            ""recommendations"": [
            ""suggested follow-up actions""
            ],
            ""riskAssessment"": ""low/medium/high with explanation"",
            ""summary"": ""brief overall summary""
            }

            **ADDITIONAL INSTRUCTIONS:**
            1. Extract and structure all available medical information
            2. If information is not available in the report, state ""Not specified""
            3. Maintain patient privacy - do not include identifying information
            4. Focus on clinically relevant information
            5. Use medical terminology appropriately
            6. Flag any critical or urgent findings

            **MEDICAL REPORT TEXT:**
            " + medicalText + @"

            **ANALYSIS:**
            Please provide the analysis in the exact JSON format specified above.";
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                status = "Medical Report Analysis API is running",
                timestamp = DateTime.UtcNow,
                features = new[] { "PDF medical report analysis", "Structured JSON output" }
            });
        }
    }
}