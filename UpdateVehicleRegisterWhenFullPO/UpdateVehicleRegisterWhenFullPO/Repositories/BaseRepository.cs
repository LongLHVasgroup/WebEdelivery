using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UpdateVehicleRegisterWhenFullPO.Context;
using UpdateVehicleRegisterWhenFullPO.Contracts;

namespace UpdateVehicleRegisterWhenFullPO.Repositories
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : class
    {
        protected readonly WebContext _dbContext;

        public BaseRepository(WebContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
