using System;

namespace AgricultureAgentSystem.Models
{
    public class MarketPrice
    {
        public int Id { get; set; }
        public string CropName { get; set; }
        public string MarketName { get; set; }
        public string Region { get; set; }
        public decimal PricePerKg { get; set; }
        public string Currency { get; set; } = "INR";
        public DateTime PriceDate { get; set; }
        public string Source { get; set; }
        public bool IsOrganic { get; set; }
        public string Grade { get; set; }
    }
}