using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Share.Interfaces
{
    public interface IRepositoryBase<T>
    where T : BaseEntity
    {
        IQueryable<T> GetAlls();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(int Id);
        void CreateAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(int Id);
    }
}
