using Application.CQRS.Results;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Checking CorrelationId and UserId for logging
                var correlationId = context.Items.ContainsKey("CorrelationId") ? context.Items["CorrelationId"] : null;
                var userName = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "anonymous";

                using (LogContext.PushProperty("CorrelationId", correlationId))
                using (LogContext.PushProperty("UserName", userName))
                {
                    _logger.LogError(ex, "Unhandled exception on path {Path}", context.Request.Path);
                }

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, cannot write exception details.");
                    return;
                }

                // Response
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = Result<object>.Exception("Server error!", (int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
            }
        }
    }
}