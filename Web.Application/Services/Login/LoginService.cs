using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Services.Users;
using Web.Domain.Entities;
using Web.Domain.UnitOfWork;

namespace Web.Application.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public LoginService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }
        public bool IsValidUserCredentials(string userName, string password)
        {
            _logger.Information($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            var _user = _unitOfWork.UserRepository.GetByUserName(userName);
            if (_user == null) return false;
            else
            {
                if (_user.Password == _userService.EncryptionPassword(password)) return true;
                else return false;
            }
        }
    }
}
