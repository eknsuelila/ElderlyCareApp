using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ElderlyCareApp.Models;

namespace ElderlyCareApp.Services
{
    // Model classes for AI service communication
    public class RequestPart
    {
        public string text { get; set; } = string.Empty;
    }

    public class NewMessage
    {
        public string role { get; set; } = string.Empty;
        public List<RequestPart> parts { get; set; } = new List<RequestPart>();
    }

    public class FastApiRequest
    {
        public string app_name { get; set; } = string.Empty;
        public string user_id { get; set; } = string.Empty;
        public string session_id { get; set; } = string.Empty;
        public NewMessage new_message { get; set; } = new NewMessage();
        public bool streaming { get; set; }
    }

    public class FastApiResponse
    {
        public Content content { get; set; } = new Content();
    }

    public class Content
    {
        public string role { get; set; } = string.Empty;
        public List<Part> parts { get; set; } = new List<Part>();
    }

    public class Part
    {
        public string text { get; set; } = string.Empty;
    }

    public class AIHealthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AIHealthService> _logger;

        public AIHealthService(HttpClient httpClient, ILogger<AIHealthService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> CreateAISessionAsync()
        {
            try
            {
                var url = "http://asl.serveblog.net:8000/apps/manager/users/k8k_id/sessions";
                _logger.LogInformation("Attempting to create AI session at: {Url}", url);

                var requestBody = new
                {
                    state = new { additionalProp1 = new { } },
                    events = new object[] { }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending session creation request...");
                var response = await _httpClient.PostAsync(url, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("AI session creation failed. Status: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                    throw new Exception($"AI session creation failed: {response.StatusCode} - {errorContent}");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("AI session created successfully. Response: {Response}", responseBody);
                
                dynamic result = JsonConvert.DeserializeObject(responseBody);
                return result?.id;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error creating AI session: {Message}", ex.Message);
                throw new Exception($"Network error connecting to AI service: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating AI session: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<string>> GetAIInsightsAsync(string sessionId, ElderlyPerson patient, 
            List<ActivityLog> activities, List<MedicationLog> medications, 
            List<MealLog> meals, List<AppointmentLog> appointments)
        {
            try
            {
                // Build patient health summary
                var healthSummary = BuildPatientHealthSummary(patient, activities, medications, meals, appointments);
                _logger.LogInformation("Built health summary for patient {PatientName}", patient.Name);

                var url = "http://asl.serveblog.net:8000/run_sse";
                _logger.LogInformation("Sending AI insights request to: {Url}", url);

                var requestPayload = new FastApiRequest
                {
                    app_name = "manager",
                    user_id = "k8k_id",
                    session_id = sessionId,
                    streaming = false,
                    new_message = new NewMessage
                    {
                        role = "user",
                        parts = new List<RequestPart>
                        {
                            new RequestPart
                            {
                                text = healthSummary
                            }
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(requestPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending AI request with session ID: {SessionId}", sessionId);
                var response = await _httpClient.PostAsync(url, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("AI insights request failed. Status: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                    throw new Exception($"AI insights request failed: {response.StatusCode} - {errorContent}");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Received AI response: {Response}", responseString);

                var messages = new List<string>();

                var parts = responseString.Split(new[] { "data: " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var raw in parts)
                {
                    var line = raw.Trim().Split('\n')[0];

                    try
                    {
                        var result = JsonConvert.DeserializeObject<FastApiResponse>(line);
                        var text = result?.content?.parts?.FirstOrDefault()?.text;
                        if (!string.IsNullOrEmpty(text))
                        {
                            messages.Add(text);
                            _logger.LogInformation("Parsed AI message: {Message}", text);
                        }
                    }
                    catch (Exception parseEx)
                    {
                        _logger.LogWarning("Failed to parse AI response line: {Line}, Error: {Error}", line, parseEx.Message);
                        continue;
                    }
                }

                _logger.LogInformation("Successfully processed {Count} AI messages", messages.Count);
                return messages;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error getting AI insights: {Message}", ex.Message);
                throw new Exception($"Network error connecting to AI service: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting AI insights: {Message}", ex.Message);
                throw;
            }
        }

        private string BuildPatientHealthSummary(ElderlyPerson patient, List<ActivityLog> activities, 
            List<MedicationLog> medications, List<MealLog> meals, List<AppointmentLog> appointments)
        {
            var age = DateTime.Now.Year - patient.DateOfBirth.Year;
            if (patient.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            var summary = $"Patient Health Summary for {patient.Name}:\n\n";
            summary += $"Age: {age} years old\n";
            
            if (!string.IsNullOrEmpty(patient.MedicalConditions))
                summary += $"Medical Conditions: {patient.MedicalConditions}\n";
            
            if (!string.IsNullOrEmpty(patient.Allergies))
                summary += $"Allergies: {patient.Allergies}\n";

            // Recent activities
            if (activities.Any())
            {
                summary += "\nRecent Activities:\n";
                foreach (var activity in activities.Take(3))
                {
                    summary += $"- {activity.Description} ({activity.StartTime:MM/dd/yyyy HH:mm})\n";
                }
            }

            // Recent medications
            if (medications.Any())
            {
                summary += "\nRecent Medications:\n";
                foreach (var med in medications.Take(3))
                {
                    var takenStatus = med.Taken ? "Taken" : "Missed";
                    summary += $"- {med.MedicationName} ({takenStatus} on {med.Timestamp:MM/dd/yyyy HH:mm})\n";
                }
            }

            // Recent meals
            if (meals.Any())
            {
                summary += "\nRecent Meals:\n";
                foreach (var meal in meals.Take(3))
                {
                    summary += $"- {meal.MealType} on {meal.MealTime:MM/dd/yyyy HH:mm}\n";
                }
            }

            // Recent appointments
            if (appointments.Any())
            {
                summary += "\nRecent Appointments:\n";
                foreach (var appt in appointments.Take(3))
                {
                    summary += $"- {appt.AppointmentType} with {appt.ProviderName} on {appt.ScheduledDateTime:MM/dd/yyyy HH:mm}\n";
                }
            }

            summary += "\nPlease provide insights and recommendations based on this patient's health data.";

            return summary;
        }
    }
}
