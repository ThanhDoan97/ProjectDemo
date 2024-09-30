using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Repositories.Products;
using Web.Domain.Repositories.Users;
using Web.Domain.UnitOfWork;
using Web.Infrastructure.Data;
using Web.Infrastructure.Repositories;
using Web.Infrastructure.Repositories.Products;
using Web.Infrastructure.Repositories.Users;

namespace Web.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ?? (_userRepository = new UserRepository(_dbContext));
            }
        }


        private IProductRepository _productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ?? (_productRepository = new ProductRepository(_dbContext));
            }

        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
