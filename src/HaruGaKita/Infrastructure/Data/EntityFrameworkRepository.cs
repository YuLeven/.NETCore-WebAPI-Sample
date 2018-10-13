using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HaruGaKita.Entities;
using HaruGaKita.Infrastructure.Interfaces;
using System.Linq;

#pragma warning disable 1591
namespace HaruGaKita.Infrastructure.Data
{
    public class EntityFrameworkRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly HaruGaKitaContext _context;

        public EntityFrameworkRepository(HaruGaKitaContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByGuidAsync(Guid guid)
        {
            return await _context.Set<T>()
                                 .Where(e => e.Uid == guid)
                                 .FirstOrDefaultAsync();
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_context.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

            return await secondaryResult.Where(spec.Criteria).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}