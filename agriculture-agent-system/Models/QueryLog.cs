using System;

namespace AgricultureAgentSystem.Models
{
    public class QueryLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string QueryType { get; set; }  // "crop", "price", "weather", etc.
        public string Parameters { get; set; } // JSON string of input parameters
        public string Response { get; set; }   // JSON string of output
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public double ExecutionTimeMs { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public User User { get; set; }
    }
}