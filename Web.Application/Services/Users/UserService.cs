using AutoMapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs;
using Web.Domain.Entities;
using Web.Domain.Repositories.Users;
using Web.Domain.UnitOfWork;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Web.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> CreateUserAsync(UserDto userDto)
        {
            try
            {
                VaditionCreateUser(userDto);
                 var user = _mapper.Map<User>(userDto);

                user.Password = EncryptionPassword(user.Password);
                await _unitOfWork.UserRepository.Create(user);
                await _unitOfWork.CompleteAsync();
                return user.Id;
            }
            catch(Exception ex)
            {
                _logger.Error("CreateUserAsync", ex.Message);
                throw ex;
            }
           
        }   

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                await  _unitOfWork.UserRepository.Delete(id);
                await _unitOfWork.CompleteAsync();

            }
            catch(Exception ex)
            {
                _logger.Error("DeleteUserAsync", ex.Message);
            }
        }

        public List<UserDtoView> GetUserAlls()
        {
            try
            {
               var users = _unitOfWork.UserRepository.GetAlls().ToList();
               var res = _mapper.Map<List<UserDtoView>>(users);
                return res;
            }
            catch(Exception ex)
            {
                _logger.Error("GetUserAlls", ex.Message);
                throw;
            }
        }

        public async Task<UserDtoView> GetUserById(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                var res = _mapper.Map<UserDtoView>(user);
                return res;
            }
            catch(Exception ex)
            {
                _logger.Error("GetUserById", ex.Message);
                throw;
            }
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            try
            {

                VaditionUpdateUser(userDto);
                var user = _mapper.Map<User>(userDto);
                await _unitOfWork.UserRepository.Update(user);
                var res = _mapper.Map<UserDto>(user);
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateUserAsync", ex.Message);
                throw ex;
            }
        }



        #region Funcion
         public void VaditionCreateUser(UserDto userDto)
        {
            // Kiểm tra userName tồn tại hay không
            var userByName = _unitOfWork.UserRepository.GetByUserName(userDto.UserName);
            if (userByName != null)
            {
                throw new Exception("Tài khoản đã tồn tại");
            }
            if (!IsValidEmail(userDto.Email))
            {
                throw new Exception("Email không đúng định dang");
            }

        }
        public async void VaditionUpdateUser(UserDto userDto) 
        {
            // Kiểm tra userName tồn tại hay không
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userDto.Id);
            var userByName = _unitOfWork.UserRepository.GetByUserName(userDto.UserName);
            if (userByName != null && user.UserName != userByName.UserName)
            {
                throw new Exception("Tài khoản đã tồn tại");
            }
            if (!IsValidEmail(userDto.Email))
            {
                throw new Exception("Email không đúng định dang");
            }
        }
        /// <summary>
        /// Mã hoá password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptionPassword(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] array = Encoding.UTF8.GetBytes(password);
            array = md5.ComputeHash(array);
            StringBuilder sb = new StringBuilder();

            foreach (byte ba in array)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
        

        // Kiểm tra định dạng Email
        public static bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        #endregion
    }
}
