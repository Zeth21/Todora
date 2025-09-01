using Application.CQRS.Results;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class ResultWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ResultWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            if (ShouldWrapResponse(context))
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                var bodyAsText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);

                var wrappedResponse = Result<object>.Success(JsonSerializer.Deserialize<object>(bodyAsText));

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(wrappedResponse, options);

                responseBody.SetLength(0);
                await context.Response.WriteAsync(json);
            }

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static bool ShouldWrapResponse(HttpContext context)
        {
            if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
            {
                return false;
            }

            if (context.Response.ContentLength == 0)
            {
                return false;
            }

            if (context.Response.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == false)
            {
                return false;
            }

            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                return false;
            }

            return true;
        }
    }
}