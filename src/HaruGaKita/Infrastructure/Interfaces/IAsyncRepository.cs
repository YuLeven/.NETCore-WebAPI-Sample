using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaruGaKita.Entities;

#pragma warning disable 1591
namespace HaruGaKita.Infrastructure.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(long id);
        Task<T> GetByGuidAsync(Guid guid);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}