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
        private readonly bool _useMockService = false; // Set to true for mock, false for real AI service

        public AIHealthService(HttpClient httpClient, ILogger<AIHealthService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> CreateAISessionAsync()
        {
            if (_useMockService)
            {
                return "mock-session-" + Guid.NewGuid().ToString("N")[..8];
            }

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
            if (_useMockService)
            {
                return await GetMockAIInsightsAsync(patient, activities, medications, meals, appointments);
            }

            try
            {
                // Build patient health summary
                var healthSummary = BuildPatientHealthSummary(patient, activities, medications, meals, appointments);
                _logger.LogInformation("Built health summary for patient {PatientName}", patient.Name);

                foreach (var med in medications)
                {
                    Console.WriteLine($"----------> Medication: {med.MedicationName}, Time: {med.Timestamp}");
                }

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

        private async Task<List<string>> GetMockAIInsightsAsync(ElderlyPerson patient, 
            List<ActivityLog> activities, List<MedicationLog> medications, 
            List<MealLog> meals, List<AppointmentLog> appointments)
        {
            try
            {
                _logger.LogInformation("Mock AI service called for patient: {PatientName}", patient.Name);
                
                var age = DateTime.Now.Year - patient.DateOfBirth.Year;
                if (patient.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

                var concerns = new List<string>();
                var recommendations = new List<string>();

                // Analyze activities
                if (activities.Any())
                {
                    var totalActivityHours = activities
                        .Where(a => a.EndTime.HasValue)
                        .Sum(a => (a.EndTime.Value - a.StartTime).TotalHours);
                    
                    if (totalActivityHours < 2)
                    {
                        concerns.Add($"Incomplete Activity Log: {patient.Name} has been less active than recommended. Only {totalActivityHours:F1} hours of activity recorded recently.");
                        recommendations.Add("Consider encouraging more physical activity for better health outcomes and maintaining mobility.");
                    }
                    else
                    {
                        concerns.Add($"Good Activity Level: {patient.Name} has been maintaining a healthy activity level with {totalActivityHours:F1} hours of recent activities.");
                        recommendations.Add("Continue with the current activity routine as it's beneficial for overall well-being.");
                    }
                }

                // Analyze medications
                if (medications.Any())
                {
                    var missedMedications = medications.Count(m => !m.Taken);
                    var totalMedications = medications.Count;
                    
                    if (missedMedications > 0)
                    {
                        concerns.Add($"Medication Compliance Issue: {patient.Name} has missed {missedMedications} out of {totalMedications} recent medication doses.");
                        recommendations.Add("Implement medication reminders and consider involving a caregiver to ensure timely administration.");
                    }
                    else
                    {
                        concerns.Add($"Excellent Medication Compliance: {patient.Name} has taken all {totalMedications} recent medications on time.");
                        recommendations.Add("Maintain the current medication schedule as it's crucial for managing chronic conditions effectively.");
                    }
                }

                // Analyze meals
                if (meals.Any())
                {
                    var mealTypes = meals.Select(m => m.MealType).Distinct().ToList();
                    if (mealTypes.Count < 3)
                    {
                        concerns.Add($"Meal Pattern Concern: {patient.Name} has been skipping some meals (only {string.Join(", ", mealTypes)} recorded).");
                        recommendations.Add("Ensure regular meal intake is important for maintaining energy and health. Consider meal planning or assistance.");
                    }
                    else
                    {
                        concerns.Add($"Good Meal Compliance: {patient.Name} has been eating regular meals including {string.Join(", ", mealTypes)}.");
                        recommendations.Add("Continue with the current meal pattern as it supports overall nutrition and health.");
                    }
                }

                // Analyze medical conditions
                if (!string.IsNullOrEmpty(patient.MedicalConditions))
                {
                    if (patient.MedicalConditions.Contains("Diabetes") || patient.MedicalConditions.Contains("diabetes"))
                    {
                        concerns.Add($"Diabetes Management: {patient.Name} has diabetes requiring careful monitoring.");
                        recommendations.Add("Regular blood sugar monitoring and medication compliance are crucial. Consider checking blood sugar levels more frequently.");
                    }
                    
                    if (patient.MedicalConditions.Contains("Hypertension") || patient.MedicalConditions.Contains("hypertension"))
                    {
                        concerns.Add($"Blood Pressure Monitoring: {patient.Name} has hypertension requiring attention.");
                        recommendations.Add("Regular blood pressure checks and medication adherence are essential for cardiovascular health.");
                    }
                }

                // Add general health insights
                if (age > 80)
                {
                    concerns.Add($"Senior Health Focus: At {age} years old, {patient.Name} requires specialized care.");
                    recommendations.Add("Prioritize regular health check-ups, medication management, and fall prevention measures.");
                }

                // Build the formatted response
                var response = $"Health Insights for {patient.Name}\n\n";
                response += $"Upon reviewing {patient.Name}'s health summary, several concerns have been identified. Here are the insights and recommendations based on the available data:\n\n";

                for (int i = 0; i < concerns.Count; i++)
                {
                    response += $"{i + 1}. Concern: {concerns[i]}\n\n";
                    response += $"Recommendation: {recommendations[i]}\n\n";
                }

                response += "### Conclusion\n\n";
                response += $"While some aspects of {patient.Name}'s care are well-documented, addressing the identified gaps will contribute to more comprehensive management of their health conditions. Engaging with healthcare providers for regular assessments and updates can further enhance the care plan.";

                _logger.LogInformation("Mock AI service generated response with {ConcernCount} concerns", concerns.Count);
                return new List<string> { response };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in mock AI service: {Message}", ex.Message);
                throw new Exception($"Mock AI service error: {ex.Message}");
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
                foreach (var activity in activities)
                {
                    summary += $"- {activity.Description} ({activity.StartTime:MM/dd/yyyy})\n";
                }
            }

            // Recent medications
            if (medications.Any())
            {
                summary += "\nRecent Medications:\n";
                foreach (var med in medications)
                {
                    var takenStatus = med.Taken ? "Taken" : "Missed";
                    summary += $"- {med.MedicationName} ({takenStatus} on {med.Timestamp:MM/dd/yyyy HH:mm})\n";
                }
            }

            // Recent meals
            if (meals.Any())
            {
                summary += "\nRecent Meals:\n";
                foreach (var meal in meals)
                {
                    summary += $"- {meal.MealType} on {meal.MealTime:MM/dd/yyyy}\n";
                }
            }

            // Recent appointments
            if (appointments.Any())
            {
                summary += "\nRecent Appointments:\n";
                foreach (var appt in appointments)
                {
                    summary += $"- {appt.AppointmentType} with {appt.ProviderName} on {appt.ScheduledDateTime:MM/dd/yyyy HH:mm}\n";
                }
            }

            summary += "\nPlease provide insights and recommendations based on this patient's health data.";
            _logger.LogInformation("Health summary ----------> {summaryzation}", summary);
            return summary;
        }
    }
} 