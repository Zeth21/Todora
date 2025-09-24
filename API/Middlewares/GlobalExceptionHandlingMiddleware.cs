using Application.CQRS.Results;
using Application.Exceptions;
using Domain.Values;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Net;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

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
                Result<object> errorResponse;

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

                //Specifying response content for validation type
                context.Response.ContentType = "application/json";
                switch (ex)
                {
                    case ValidationException validationEx:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                        // Collecting validation errors as a list
                        var errors = validationEx.Errors.Select(err => err.ErrorMessage).ToList();

                        // Generating a standard fail response
                        errorResponse = Result<object>.Fail(StringValues.ValidationError, (int)HttpStatusCode.BadRequest, errors);
                        break;

                    case SaveDataException saveDataEx:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse = Result<object>.Fail(StringValues.SaveFail, (int)HttpStatusCode.InternalServerError);
                        break;

                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse = Result<object>.Exception(StringValues.InternalServerError, (int)HttpStatusCode.InternalServerError);
                        break;
                }
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
            }
        }
    }
}