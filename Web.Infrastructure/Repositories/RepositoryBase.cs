
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entities;
using Web.Domain.Repositories;
using Web.Infrastructure.Data;

namespace Web.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T>
     where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>() ?? throw new ArgumentNullException(nameof(_dbSet));
        }
        public IQueryable<T> GetAlls()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            return await _dbSet.FindAsync(Id);
        }
        public async Task Create(T entity)
        {   
          await  _dbSet.AddAsync(entity);
        }
        public async Task Update(T entity)
        {
            // Đánh dấu thực thể là đã thay đổi (modified)
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            // Kiểm tra xem có thay đổi không trước khi lưu
            if (_context.ChangeTracker.HasChanges())
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
            }
        }
        public  async Task Delete(int Id)
        {
            var res = await GetByIdAsync(Id);
            _dbSet.Remove(res);
        }
    }
}
