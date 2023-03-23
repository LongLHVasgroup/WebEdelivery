using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UpdateVehicleRegisterWhenFullPO.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

    }
}
