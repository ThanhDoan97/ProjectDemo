using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Shared.Responses;

namespace Web.Shared.Helpers
{
    public static class ApiResponseHelper
    {
        // Phương thức trả về ApiResponse thành công
        public static IActionResult SuccessResponse<T>(T data, string message = "Request succeeded")
        {
            var response = ApiResponse<T>.SuccessResponse(data, message);
            return new OkObjectResult(response);
        }

        // Phương thức trả về ApiResponse lỗi
        public static IActionResult ErrorResponse(string message, int statusCode = 500)
        {
            var response = new ErrorResponse(statusCode, message);
            return new ObjectResult(response) { StatusCode = statusCode };
        }
    }
}
