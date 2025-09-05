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
        public async Task<IActionResult> AnalyzeMedicalReport(IFormFile medicalReport, [FromForm] string userAllInfo)
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
                string medicalPrompt = CreateMedicalAnalysisPrompt(extractedText, userAllInfo);

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

        private string CreateMedicalAnalysisPrompt(string medicalText, string patientAllInfo)
        {
            return @$"
            You are a medical report analysis assistant. Analyze the following medical report and provide a structured response.

            **USER BASIC INFORMATION:**
            {patientAllInfo}

            **MEDICAL REPORT TEXT:**
            {medicalText}

            **REQUIRED OUTPUT FORMAT (JSON structure):**
            {{
                ""patientInformation"": {{
                    ""age"": ""extracted or estimated"",
                    ""gender"": ""extracted if available"",
                    ""relevantHistory"": ""summary""
                }},
                ""keyFindings"": [
                    ""list of main medical findings""
                ],
                ""diagnoses"": [
                    ""list of diagnoses if mentioned""
                ],
                ""medications"": [
                    ""list of medications prescribed""
                ],
                ""testResults"": {{
                    ""abnormalResults"": [""list""],
                    ""normalResults"": [""list""]
                }},
                ""recommendations"": {{
                    ""diet"": [
                        {{
                            ""title"": ""recommendation title"",
                            ""description"": ""detailed description"",
                            ""impact"": ""specific impact or benefit"",
                            ""duration"": ""recommended duration"",
                            ""keyPoints"": ""key implementation points"",
                            ""mainBenefits"": [""benefit 1"", ""benefit 2""],
                            ""howToUse"": ""detailed usage instructions"",
                            ""applicationFrequency"": ""how often to apply"",
                            ""caution"": ""any precautions or warnings"",
                            ""goal"": ""primary objective of this recommendation""
                        }}
                    ],
                    ""supplements"": [
                        {{
                            ""title"": ""supplement name"",
                            ""description"": ""detailed description"",
                            ""dosage"": ""recommended dosage"",
                            ""frequency"": ""how often to take"",
                            ""duration"": ""recommended duration"",
                            ""purpose"": ""primary purpose"",
                            ""benefits"": [""benefit 1"", ""benefit 2""],
                            ""instructions"": ""how to take"",
                            ""precautions"": ""any precautions"",
                            ""interactions"": ""potential interactions""
                        }}
                    ],
                    ""galenicForm"": [
                        {{
                            ""title"": ""form recommendation title"",
                            ""description"": ""detailed description"",
                            ""formType"": ""tablet/capsule/liquid etc"",
                            ""advantages"": [""advantage 1"", ""advantage 2""],
                            ""usageInstructions"": ""how to use this form"",
                            ""duration"": ""recommended duration"",
                            ""compatibility"": ""compatibility with other forms""
                        }}
                    ],
                    ""lifestyle"": [
                        {{
                            ""title"": ""lifestyle recommendation title"",
                            ""description"": ""detailed description"",
                            ""frequency"": ""how often to practice"",
                            ""duration"": ""recommended duration"",
                            ""benefits"": [""benefit 1"", ""benefit 2""],
                            ""implementationSteps"": ""step-by-step instructions"",
                            ""timeRequired"": ""time commitment needed"",
                            ""precautions"": ""any precautions""
                        }}
                    ]
                }},
                ""riskAssessment"": ""low/medium/high with explanation"",
                ""summary"": ""brief overall summary""
            }}

            **CRITICAL INSTRUCTIONS:**
            1. Provide COMPLETE, DETAILED recommendations for each category as shown in the structure
            2. Each recommendation should have all the specified fields filled
            3. For diet recommendations, include specific foods, quantities, and meal timing
            4. For supplements, include exact dosages, brands (if known), and timing
            5. For galenic forms, specify exact formulations and administration methods
            6. For lifestyle, provide actionable, measurable activities
            7. All recommendations should be personalized based on the user's medical report and information
            8. Include practical implementation details and timelines
            9. Provide cautions and precautions where applicable
            10. Return ONLY valid JSON - no additional text or explanations

            **EXAMPLE DIET RECOMMENDATION:**
            {{
                ""title"": ""Increase Probiotic Intake"",
                ""description"": ""Include natural probiotics in your diet to boost digestion, balance gut flora, and improve gut health."",
                ""impact"": ""Improves bacterial diversity by 25%"",
                ""duration"": ""3-4 weeks"",
                ""keyPoints"": ""Include in breakfast 3 times per week. Start with small portions and gradually increase."",
                ""mainBenefits"": [
                    ""Improves Digestive Health: Supports gut flora balance, aiding in better digestion and reduced bloating"",
                    ""Strengthens Immune System: Boosts body's natural defenses by enhancing microbiome diversity"",
                    ""Enhances Nutrient Absorption: Helps body absorb vitamins and minerals more efficiently""
                ],
                ""howToUse"": ""Start by gradually introducing fermented foods into daily routine. Consume 100-150ml per day, preferably in the morning on empty stomach or alongside meals. Can be taken directly or combined with smoothies, oats, or salads. Ensure consistent intake for 2-4 weeks."",
                ""applicationFrequency"": ""Daily"",
                ""caution"": ""Not recommended during antibiotic treatment. Begin with smaller portions if new to fermented foods."",
                ""goal"": ""Improve gut flora and immune system""
            }}

            **ANALYSIS:**
            Provide ONLY the JSON object, no additional text:";
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