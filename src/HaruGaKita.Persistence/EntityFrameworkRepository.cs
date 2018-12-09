using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HaruGaKita.Persistence.Interfaces;
using HaruGaKita.Domain.Entities;
using HaruGaKita.Persistence;

#pragma warning disable 1591
namespace HaruGaKita.Infrastructure.Data
{
    public class EntityFrameworkRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly HaruGaKitaDbContext _dbContext;

        public EntityFrameworkRepository(HaruGaKitaDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task<T> GetByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByGuidAsync(Guid guid)
        {
            return await _dbContext.Set<T>()
                                 .Where(e => e.Uid == guid)
                                 .FirstOrDefaultAsync();
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

            return await secondaryResult.Where(spec.Criteria).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}