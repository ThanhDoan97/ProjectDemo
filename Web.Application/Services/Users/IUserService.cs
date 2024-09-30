using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs;

namespace Web.Application.Services.Users    
{
    public interface IUserService 
    {
        public List<UserDtoView> GetUserAlls();
        public Task<UserDtoView> GetUserById(int id);
        public Task<int> CreateUserAsync(UserDto user); 
        public Task<UserDto> UpdateUserAsync(UserDto userDto);
        public Task DeleteUserAsync(int id);
        string EncryptionPassword(string password);
    }
}
