using System;

namespace AgricultureAgentSystem.Models
{
    public class WeatherData
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }      // Celsius
        public double Humidity { get; set; }         // Percentage
        public double Rainfall { get; set; }         // mm
        public double WindSpeed { get; set; }        // km/h
        public string WeatherCondition { get; set; } // "Sunny", "Rainy", etc.
        public double SoilMoisture { get; set; }     // Percentage
        public double SolarRadiation { get; set; }   // MJ/m²
        public string Forecast { get; set; }         // 7-day forecast JSON
    }
}