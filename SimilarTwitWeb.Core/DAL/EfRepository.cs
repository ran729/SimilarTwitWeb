using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.DAL
{
    public class EfRepository<T> where T : BaseEntity
    {
        protected DatabaseContext _dbContext;
        private DbSet<T> DbSet;
        
        public EfRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.Now;

            DbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
