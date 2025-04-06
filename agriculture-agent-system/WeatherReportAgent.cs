using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AgricultureAgentSystem
{
    public class WeatherReportAgent
    {
        private readonly HttpClient _httpClient;

        public WeatherReportAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetWeatherReportAsync(string region)
        {
            // Logic to fetch weather report for the specified region
            string apiUrl = $"https://api.example.com/weather/{region}"; // Placeholder API URL
            var response = await _httpClient.GetStringAsync(apiUrl);
            
            // Parse the response and extract relevant weather information
            var weatherData = JObject.Parse(response);
            return FormatWeatherReport(weatherData);
        }

        private string FormatWeatherReport(JObject weatherData)
        {
            // Format the weather data for agricultural purposes
            return $"Current Weather:\n" +
                   $"Temperature: {weatherData["temp"]}°C\n" +
                   $"Humidity: {weatherData["humidity"]}%\n" +
                   $"Rainfall: {weatherData["rainfall"]}mm\n" +
                   $"Wind Speed: {weatherData["wind_speed"]} km/h";
        }
    }
}