using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Shared.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }   
        public T Data { get; set; }
        public string Message { get; set; }
        public ApiResponse(bool success, T data, string message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
        {
            return new ApiResponse<T>(true, data, message);
        }
        public static ApiResponse<T> ErrorResponse(T data,string message)
        {
            return new ApiResponse<T>(false, data, message);
        }
    }
}
