using Microsoft.AspNetCore.Mvc;
using Web.Domain.UnitOfWork;
using ILogger = Serilog.ILogger;
using CMC.TS.EDU.UMS.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using CMC.TS.EDU.UMS.Biz.Model.Account;
using Web.Application.Services.Login;
using Web.Application.DTOs;
using AutoMapper;
using System.Security.Claims;
namespace Web.Api.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;
        public AccountController(IUnitOfWork unitOfWork, ILogger logger, IJwtAuthManager jwtAuthManager, ILoginService loginService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jwtAuthManager = jwtAuthManager;
            _loginService = loginService;
            _mapper = mapper;

        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginResult)
        {
            _logger.Information($"User [{loginResult.UserName}] logging in the system.");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _unitOfWork.UserRepository.GetByUserName(loginResult.UserName);

            if (user == null || user.Id == 0)
            {
                return Unauthorized();
            }
            if (!_loginService.IsValidUserCredentials(loginResult.UserName, loginResult.Password))
            {
                return Unauthorized();
            }
            var userView = _mapper.Map<UserDtoView>(user);
            return await BuildLoginResult(userView);
        }
        private async Task<IActionResult> BuildLoginResult(UserDtoView user)
        {
            var claims = new[]
              {
                    new Claim("UserID", $"{user.Id:0}"),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("Name", user.Name??""),
                    new Claim("Email", user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Role != null ? ( user.Role == Domain.Enum.EnumRole.Admin ? "Admin" : "User") : ""),
                new Claim("RoleName", user.RoleName != null ? ( user.Role == Domain.Enum.EnumRole.Admin ? "Quản trị" : "Người dùng") : ""),
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(user.UserName, claims, DateTime.Now);
            _logger.Information($"User [{user.UserName}] logged in the system.");
            return Ok(new LoginResult
            {
                UserID = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role != null ? (user.Role == Domain.Enum.EnumRole.Admin ? "Admin" : "User") : "",
                RoleName = user.Role != null ? (user.Role == Domain.Enum.EnumRole.Admin ? "Quản trị" : "Người dùng") : "",
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString,
            });
        }
    }
}
