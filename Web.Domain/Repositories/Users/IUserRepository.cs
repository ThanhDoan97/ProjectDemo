using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entities;


namespace Web.Domain.Repositories.Users 
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        User GetByUserName(string userName);
    }
}
