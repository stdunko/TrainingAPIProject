using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace PostPatientsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Delay(10000).GetAwaiter().GetResult();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string? baseUrl = configuration["ApiSettings:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
            {
                FileLogger.Log("API Base URL is not configured.");
                return;
            }

            FileLogger.Log("Starting to adding patients...");

            for (int i = 1; i <= 100; i++)
            {
                var patient = GeneratePatient(i);
                PostPatient(patient, baseUrl);
            }

            FileLogger.Log("Finished adding patients.");
        }

        static object GeneratePatient(int index)
        {
            return new
            {
                Name = new
                {
                    Use = "Official",
                    Family = "Smith",
                    Given = new List<string> { "John", "A." }
                },
                Gender = index % 2 == 0 ? "Male" : "Female",
                BirthDate = DateTime.UtcNow.AddYears(-30 - index).ToString("yyyy-MM-dd"),
                Active = index % 2 == 0 ? "True" : "False"
            };
        }

        static void PostPatient(object patient, string url)
        {
            using var client = new HttpClient();
            var json = JsonSerializer.Serialize(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = client.PostAsync(url, content).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    FileLogger.Log("Successfully added a patient.");
                }
                else
                {
                    FileLogger.Log($"Failed to add patient. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log($"Error adding patient: {ex.Message}");
            }
        }
    }

    public static class FileLogger
    {
        private static readonly string logFilePath = "logs/loginfo.log";

        static FileLogger()
        {
            var logDir = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
        }

        public static void Log(string message)
        {
            var logEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} - {message}{Environment.NewLine}";
            File.AppendAllText(logFilePath, logEntry);
        }
    }
}