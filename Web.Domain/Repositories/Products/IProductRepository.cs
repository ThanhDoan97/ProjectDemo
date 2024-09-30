using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entities;
using Web.Domain.Repositories;

namespace Web.Domain.Repositories.Products  
{
    public interface IProductRepository : IRepositoryBase<Product>    
    {
    }
}
