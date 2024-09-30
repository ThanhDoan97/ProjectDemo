using Microsoft.AspNetCore.Mvc;
using Web.Application.DTOs;
using Web.Application.Services.Users;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-all-user")]
        public IActionResult GetAll()
        {
            var res = _userService.GetUserAlls();
            return Ok(res);
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var User = _userService.GetUserById(id);
            if (User == null)
            {
                return ErrorResponse(User, "Not Found");
            }
            return SuccessResponse(User);

        }
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto UserDto)
        {

            var user = _userService.CreateUserAsync(UserDto);
            if (user == null)
            {
                return ErrorResponse(user, "Not Found");
            }
            return SuccessResponse(User);
        }
        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserDto UserDto)
        {
            var User = _userService.UpdateUserAsync(UserDto);
            if (User == null)
            {
                return ErrorResponse(User, "NotFound");
            }
            return SuccessResponse(User);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var res = _userService.DeleteUserAsync(id);
            if (res == null)
            {
                return ErrorResponse(new UserDto(), "NotFound");
            }
            return SuccessResponse(res);
        }


    }
}
