using System;
namespace PuzzleAPI.Models
{
    public class AppResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static AppResponse<T> Valid(T data, string message)
        {
            return new AppResponse<T> { Data = data, Success = true, Message = message };
        }
        public static AppResponse<T> Invalid(string message)
        {
            return new AppResponse<T> { Success = false, Message = message };
        }
    }
    public class AppResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public static AppResponse Valid(string message)
        {
            return new AppResponse { Success = true, Message = message };
        }
        public static AppResponse Invalid(string message)
        {
            return new AppResponse { Success = false, Message = message };
        }
    }
}

