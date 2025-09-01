using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.IO;
using System.Threading.Tasks;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = context.Items.ContainsKey(CorrelationIdHeader) ? context.Items[CorrelationIdHeader]?.ToString() : context.TraceIdentifier;
        var userName = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "anonymous";

        var request = context.Request;
        var startTime = DateTime.UtcNow;

        using (LogContext.PushProperty("UserName", userName))
        using (LogContext.PushProperty("CorrelationId", traceId))
        {
            _logger.LogInformation(
                "Incoming Request: {Method} {Url} | TraceId: {TraceId}",
                request.Method,
                $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                traceId
            );
        }

        var originalBodyStream = context.Response.Body;
        await using var responseBody = new MemoryStream(); 
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        finally
        {
            var duration = DateTime.UtcNow - startTime;

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            using (LogContext.PushProperty("UserName", userName))
            using (LogContext.PushProperty("CorrelationId", traceId))
            {
                _logger.LogInformation(
                    "Outgoing Response: {StatusCode} | Duration: {Duration}ms | TraceId: {TraceId}",
                    context.Response.StatusCode,
                    duration.TotalMilliseconds,
                    traceId
                );
            }

            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
}