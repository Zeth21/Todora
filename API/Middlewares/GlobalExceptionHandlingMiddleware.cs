using Application.CQRS.Results;
using FluentValidation;
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
            catch (ValidationException validationEx) // Catch validation exceptions first
            {
                // Checking CorrelationId and UserId for logging
                var correlationId = context.Items.ContainsKey("CorrelationId") ? context.Items["CorrelationId"] : null;
                var userName = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "anonymous";

                using (LogContext.PushProperty("CorrelationId", correlationId))
                using (LogContext.PushProperty("UserName", userName))
                {
                    _logger.LogError(validationEx, "Unhandled exception on path {Path}", context.Request.Path);
                }

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, cannot write exception details.");
                    return;
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                // Collecting validation errors as a list
                var errors = validationEx.Errors.Select(err => err.ErrorMessage).ToList();

                // Generating a standard fail response
                var errorResponse = Result<object>.Fail("One or more validation errors occured!", (int)HttpStatusCode.BadRequest, errors);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
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