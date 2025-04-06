namespace AgricultureAgentSystem.Models
{
    public class CropData
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public string CropName { get; set; }
        public string SoilType { get; set; }
        public string OptimalSeason { get; set; }
        public double WaterRequirement { get; set; } // mm per season
        public double YieldPotential { get; set; }   // tons per hectare
        public string DiseaseRisks { get; set; }     // Common diseases
        public DateTime LastUpdated { get; set; }
    }
}