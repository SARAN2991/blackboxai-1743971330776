using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AgricultureAgentSystem.Middleware;
using AgricultureAgentSystem.Models;
using AgricultureAgentSystem.Repositories;
using AgricultureAgentSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace AgricultureAgentSystem
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Log.Information("Starting web application");
                
                var builder = WebApplication.CreateBuilder(args);
                
                // Add Serilog
                builder.Host.UseSerilog();

            // Add services to the container
            builder.Services.AddControllers()
                .AddFluentValidation(fv => 
                {
                    fv.RegisterValidatorsFromAssemblyContaining<Program>();
                    fv.AutomaticValidationEnabled = true;
                    fv.ImplicitlyValidateChildProperties = true;
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Agriculture Agent System API", 
                    Version = "v1",
                    Description = "API for the Agriculture Agent System providing crop recommendations, market prices, weather reports and more.",
                    Contact = new OpenApiContact
                    {
                        Name = "Support Team",
                        Email = "support@agricultureagent.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License"
                    }
                });
                
                // Include XML comments if available
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // Configure rate limiting
            builder.Services.AddRateLimiter(options => 
            {
                options.AddFixedWindowLimiter("ApiLimiter", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 100;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 10;
                });
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
            
            // Add health checks
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AgricultureContext>()
                .AddCheck<CropRecommendationAgent>("CropRecommendationAgent")
                .AddCheck<MarketPriceAgent>("MarketPriceAgent")
                .AddCheck<WeatherReportAgent>("WeatherReportAgent")
                .AddCheck<YieldPredictionAgent>("YieldPredictionAgent")
                .AddCheck<DiseaseDetectionAgent>("DiseaseDetectionAgent");

            // Configure database context
            builder.Services.AddDbContext<AgricultureContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AgricultureDB")));

            // Register repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRepository<QueryLog>, Repository<QueryLog>>();

            // Register services
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<CropRecommendationAgent>();
            builder.Services.AddScoped<MarketPriceAgent>();
            builder.Services.AddScoped<WeatherReportAgent>();
            builder.Services.AddScoped<YieldPredictionAgent>();
            builder.Services.AddScoped<DiseaseDetectionAgent>();

            // Configure JWT authentication
            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
                    };
                });

            // Build the application
            var app = builder.Build();

            // Configure the HTTP request pipeline
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agriculture Agent System v1");
                c.RoutePrefix = "api-docs";
                c.DocumentTitle = "Agriculture Agent System API Documentation";
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // Add request logging middleware
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseRateLimiter();
            app.MapControllers();
            
            // Add health check endpoint
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(e => new
                        {
                            Name = e.Key,
                            Status = e.Value.Status.ToString(),
                            Duration = e.Value.Duration.TotalMilliseconds,
                            Exception = e.Value.Exception?.Message
                        }),
                        TotalDuration = report.TotalDuration.TotalMilliseconds
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });

            // Initialize database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AgricultureContext>();
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database");
                }
            }

            await app.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}