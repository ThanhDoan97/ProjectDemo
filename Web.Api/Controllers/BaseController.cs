using Microsoft.AspNetCore.Mvc;
using Web.Shared.Responses;

namespace Web.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
  
        protected IActionResult SuccessResponse<T>(T data, string message = "Success")
        {
            var response = ApiResponse<T>.SuccessResponse(data, message);
            return Ok(response);
        }

        protected IActionResult ErrorResponse<T>(T data,string message)
        {
            var response = ApiResponse<T>.ErrorResponse(data,message);
            return Ok(response);
        }
    }
}
