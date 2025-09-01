public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check is there a header in request
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId) || string.IsNullOrEmpty(correlationId))
        {
            correlationId = Guid.NewGuid().ToString(); // If not create a new one
        }

        // Add it to HttpContext
        context.Items[CorrelationIdHeader] = correlationId;
        context.TraceIdentifier = correlationId!;

        // Add it to Response
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationIdHeader] = correlationId;
            return Task.CompletedTask;
        });

        _logger.LogDebug("CorrelationId set: {CorrelationId}", correlationId!);

        await _next(context);
    }
}
