using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AgricultureAgentSystem.Models
{
    public class AgricultureContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<QueryLog> QueryLogs { get; set; }
        public DbSet<CropData> CropDatas { get; set; }
        public DbSet<MarketPrice> MarketPrices { get; set; }
        public DbSet<WeatherData> WeatherDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Name=ConnectionStrings:AgricultureDB");
    }
}