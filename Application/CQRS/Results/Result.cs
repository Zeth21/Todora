using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Results
{
    public class Result<T>
    {
        public bool IsSucceeded { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; }
        public static Result<T> Success(T? data, int statusCode = 200, string message = "Success!") 
        {
            return new Result<T>
            {
                IsSucceeded = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static Result<T> Fail(string message = "Fail!", int statusCode = 400, List<string>? errors = null)
            => new Result<T> { IsSucceeded = false, StatusCode = statusCode, Message = message, Errors = errors ?? new List<string> { "Something went wrong!" } };

        public static Result<T> Exception(string message = "Internal Server Error!", int statusCode = 500, List<string>? errors = null)
            => new Result<T> { IsSucceeded = false, Message = message, StatusCode = statusCode, Errors = errors ?? new List<string> { "Something went wrong!" } };

        public static Result<T> Unauthorized()
            => new Result<T> { IsSucceeded = false, Message = "Unauthorized", StatusCode = 401 };

        public static Result<T> Forbidden()
            => new Result<T> { IsSucceeded = false, Message = "Forbidden", StatusCode = 403 };

        public static Result<T> NoContent(string message = "Nothing has found!")
            => new Result<T> { IsSucceeded = true, Message = message, StatusCode = 204 };
    }
}
