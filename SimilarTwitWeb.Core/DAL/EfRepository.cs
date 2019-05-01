using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SimilarTwitWeb.Core.Exceptions;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.DAL
{
    public class EfRepository<T> where T : BaseEntity
    {
        private const int UNIQUE_CONSTRAINER_ERROR_CODE = 19;

        protected DatabaseContext _dbContext;
        private DbSet<T> DbSet;
        
        public EfRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                entity.CreatedAt = DateTime.Now;

                DbSet.Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch(DbUpdateException e) when (e.InnerException != null && (e.InnerException is SqliteException))
            {
                var sqliteException = e.InnerException as SqliteException;

                switch (sqliteException.SqliteErrorCode)
                {
                    case UNIQUE_CONSTRAINER_ERROR_CODE:
                        throw new UniqueRowAlreadyExistsException();
                    default:
                        throw new Exception($"Could not complete transaction, Db error code - {sqliteException.SqliteErrorCode}");
                }
            }
        }
    }
}
