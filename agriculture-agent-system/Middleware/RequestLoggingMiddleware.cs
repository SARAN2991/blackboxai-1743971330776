using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AgricultureAgentSystem.Models;
using AgricultureAgentSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AgricultureAgentSystem.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly IRepository<QueryLog> _logRepository;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger,
            IRepository<QueryLog> logRepository)
        {
            _next = next;
            _logger = logger;
            _logRepository = logRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;
            var response = context.Response;

            try
            {
                await _next(context);
                stopwatch.Stop();

                if (request.Path.StartsWithSegments("/api"))
                {
                    var logEntry = new QueryLog
                    {
                        UserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                        QueryType = $"{request.Method} {request.Path}",
                        Parameters = $"{request.QueryString}",
                        Response = $"Status: {response.StatusCode}",
                        Timestamp = DateTime.UtcNow,
                        ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                        Success = response.StatusCode < 400
                    };

                    await _logRepository.AddAsync(logEntry);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Request failed");
                throw;
            }
        }
    }
}