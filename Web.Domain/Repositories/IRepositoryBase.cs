using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entities;

namespace Web.Domain.Repositories
{
    public interface IRepositoryBase<T>
      where T : BaseEntity
    {
        IQueryable<T> GetAlls();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(int Id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(int Id);
    }
}
