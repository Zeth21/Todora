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

            try
            {
            await _next(context);

            if (ShouldWrapResponse(context))
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                var bodyAsText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);

                    Result<object> wrappedResponse;

                    if (context.Response.ContentType?.Contains("text/plain", StringComparison.OrdinalIgnoreCase) == true && !string.IsNullOrWhiteSpace(bodyAsText))
                    {
                        wrappedResponse = Result<object>.Success(data: null, message: bodyAsText);
                    }

                    else
                    {
                        object? bodyObject = string.IsNullOrWhiteSpace(bodyAsText)
                            ? null
                            : JsonSerializer.Deserialize<object>(bodyAsText);
                        wrappedResponse = Result<object>.Success(bodyObject);
                    }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(wrappedResponse, options);
                responseBody.SetLength(0);
                await context.Response.WriteAsync(json);
            }
            }
            catch (Exception)
            {
                context.Response.Body = originalBodyStream;
                throw;
            }

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }

        private static bool ShouldWrapResponse(HttpContext context)
        {
            if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
            {
                return false;
            }

            {
                return false;
            }

            {
                return false;
            }

            return true;
        }
    }
}