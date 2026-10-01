using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EnglishFlashcard3D.Models;

namespace EnglishFlashcard3D.Services
{
    public class GeminiService
    {
        private readonly string _apiKey;
        private static readonly HttpClient Client = new HttpClient();
        private static readonly string[] PreferredModels =
        {
            "gemini-2.5-flash-lite",
            "gemini-2.5-flash",
            "gemini-3.8-flash-lite",
            "gemini-3.8-flash",
            "gemini-3.0-flash"
        };

        private const int RetryCount = 3;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(2);
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public GeminiService(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        /// <summary>
        /// Sinh từ vựng tiếng Anh từ một chủ đề bằng Gemini AI
        /// </summary>
        public async Task<List<EnglishFlashcard>> GenerateVocabularyAsync(string topic, int count = 5, string level = "B1")
        {
            try
            {
                var prompt = GeneratePrompt(topic, count, level);
                var response = await CallGeminiAPI(prompt);

                if (string.IsNullOrWhiteSpace(response))
                    return new List<EnglishFlashcard>();

                var jsonContent = ExtractJsonFromResponse(response);
                var flashcards = JsonSerializer.Deserialize<List<EnglishFlashcard>>(jsonContent, JsonOptions);

                return flashcards ?? new List<EnglishFlashcard>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Gemini API Error: {ex.Message}");
                throw;
            }
        }

        private async Task<string> CallGeminiAPI(string prompt)
        {
            Exception? lastError = null;

            foreach (var modelName in PreferredModels)
            {
                for (var attempt = 1; attempt <= RetryCount; attempt++)
                {
                    var requestBody = new
                    {
                        contents = new[]
                        {
                            new
                            {
                                parts = new[]
                                {
                                    new { text = prompt }
                                }
                            }
                        }
                    };

                    var json = JsonSerializer.Serialize(requestBody);
                    using var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent?key={_apiKey}";
                    using var response = await Client.PostAsync(url, content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return ExtractTextFromGeminiResponse(responseBody);
                    }

                    lastError = new Exception($"API Error: {response.StatusCode} - {responseBody}");

                    if (IsRetryable(response.StatusCode, responseBody) && attempt < RetryCount)
                    {
                        await Task.Delay(RetryDelay * attempt);
                        continue;
                    }

                    if (!IsModelNotFound(response.StatusCode, responseBody) && !IsRetryable(response.StatusCode, responseBody))
                    {
                        throw lastError;
                    }

                    break;
                }
            }

            throw lastError ?? new Exception("Gemini API failed for all preferred models.");
        }

        private static bool IsModelNotFound(HttpStatusCode statusCode, string errorBody)
        {
            return statusCode == HttpStatusCode.NotFound ||
                   errorBody.Contains("NOT_FOUND", StringComparison.OrdinalIgnoreCase) ||
                   errorBody.Contains("no longer available", StringComparison.OrdinalIgnoreCase) ||
                   errorBody.Contains("not found", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsRetryable(HttpStatusCode statusCode, string errorBody)
        {
            return statusCode == HttpStatusCode.ServiceUnavailable ||
                   statusCode == HttpStatusCode.TooManyRequests ||
                   statusCode == HttpStatusCode.GatewayTimeout ||
                   errorBody.Contains("UNAVAILABLE", StringComparison.OrdinalIgnoreCase) ||
                   errorBody.Contains("high demand", StringComparison.OrdinalIgnoreCase) ||
                   errorBody.Contains("try again later", StringComparison.OrdinalIgnoreCase);
        }

        private string ExtractTextFromGeminiResponse(string responseJson)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(responseJson))
                {
                    var root = doc.RootElement;
                    var candidates = root.GetProperty("candidates");
                    var content = candidates[0].GetProperty("content");
                    var parts = content.GetProperty("parts");
                    var text = parts[0].GetProperty("text").GetString();
                    return text ?? string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GeneratePrompt(string topic, int count, string level)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Generate {count} English vocabulary words related to the topic \"{topic}\" at level {level}.");
            sb.AppendLine();
            sb.AppendLine("Return ONLY a JSON array in this exact format (NO extra text):");
            sb.AppendLine("[");
            sb.AppendLine("  {");
            sb.AppendLine("    \"word\": \"English word\",");
            sb.AppendLine("    \"phonetic\": \"/IPA pronunciation/\",");
            sb.AppendLine("    \"partOfSpeech\": \"Noun/Verb/Adjective\",");
            sb.AppendLine("    \"meaningVi\": \"Vietnamese definition\",");
            sb.AppendLine("    \"exampleEn\": \"Example sentence in English\",");
            sb.AppendLine("    \"exampleVi\": \"Vietnamese translation\",");
            sb.AppendLine("    \"note\": \"Synonyms or notes\"");
            sb.AppendLine("  }");
            sb.AppendLine("]");
            sb.AppendLine();
            sb.AppendLine("Rules:");
            sb.AppendLine("- IPA phonetic must be accurate");
            sb.AppendLine("- Vietnamese definition should be concise (1-2 lines)");
            sb.AppendLine("- Examples should be realistic and easy to understand");
            sb.AppendLine("- Return EXACTLY " + count + " words");
            sb.AppendLine("- Return ONLY JSON, no other text");
            sb.AppendLine("- If error, return empty array: []");

            return sb.ToString();
        }

        private string ExtractJsonFromResponse(string response)
        {
            response = response.Replace("```json", "").Replace("```", "").Trim();

            int startIndex = response.IndexOf('[');
            int endIndex = response.LastIndexOf(']');

            if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
            {
                return "[]";
            }

            return response.Substring(startIndex, endIndex - startIndex + 1);
        }
    }
}
