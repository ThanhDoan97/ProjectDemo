using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entities;
using Web.Domain.Repositories.Users;
using Web.Domain.UnitOfWork;
using Web.Infrastructure.Data;

namespace Web.Infrastructure.Repositories.Users
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public User GetByUserName(string userName)
        {
            var user = _context.Set<User>().FirstOrDefault(x => x.UserName.Equals(userName));
            return user;
        }
    }
}
