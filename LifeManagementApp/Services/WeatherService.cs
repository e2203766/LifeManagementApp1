using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LifeManagementApp.Models;

namespace LifeManagementApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.open-meteo.com")
            };
        }

        public async Task<WeatherResponse> GetWeatherDataAsync(double latitude, double longitude)
        {
            try
            {
                // Формирование полного URL для запроса данных о погоде
                string url = $"/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true&daily=temperature_2m_min,temperature_2m_max,precipitation_sum";
                Console.WriteLine("Request URL: " + url); // Отладочное сообщение для проверки URL

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Выброс исключения при ошибке HTTP

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("JSON Response: " + jsonResponse); // Вывод ответа для отладки

                // Отдельный try-catch блок для десериализации
                try
                {
                    // Десериализация JSON-ответа в объект WeatherResponse
                    var result = JsonSerializer.Deserialize<WeatherResponse>(jsonResponse);
                    if (result == null)
                    {
                        Console.WriteLine("Deserialization resulted in null.");
                        throw new Exception("Failed to deserialize JSON response.");
                    }
                    return result;
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"Deserialization error: {jsonEx.Message}");
                    throw; // Пробросьте исключение, если вам нужно обработать его выше
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                throw; // Проброс исключения для обработки в вызывающем коде
            }
        }



    }
}


