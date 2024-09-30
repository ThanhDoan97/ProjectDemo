using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Repositories.Products;
using Web.Domain.Repositories.Users;

namespace Web.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        Task<int> CompleteAsync();
    }
}
