using Bakcend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public interface IRepository<TEntity, in TKey> where TEntity : class where TKey : notnull
    {

        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        public Task<bool> UpdateAsync(TEntity entity, CancellationToken ct = default);

        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct);
    }
    public class Repository<TEntity, TKey>(AppDbContext db) : IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : notnull
    {
        
        // Подключаем навигационные свойства-ссылки (не коллекции),
        // чтобы в ответах API были заполнены вложенные объекты (Technic.TechnicType)
        private IQueryable<TEntity> WithReferences()
        {
            var entityType = db.Model.FindEntityType(typeof(TEntity))!;
            var query = (IQueryable<TEntity>)db.Set<TEntity>();

            foreach (var navigation in entityType.GetNavigations().Where(n => !n.IsCollection))
            {
                query = query.Include(navigation.Name);
            }

            return query;
        }

        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await WithReferences()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await db.Set<TEntity>().FindAsync([id], cancellationToken);

            if (entity is not null)
            {
                var entityType = db.Model.FindEntityType(typeof(TEntity))!;

                foreach (var navigation in entityType.GetNavigations().Where(n => !n.IsCollection))
                {
                    await db.Entry(entity).Reference(navigation.Name).LoadAsync(cancellationToken);
                }
            }

            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            db.Set<TEntity>().Add(entity);
            await db.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            db.Set<TEntity>().Update(entity);
            return await db.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await db.Set<TEntity>()
                .FindAsync([id], cancellationToken);
            if (entity is null)
                return false;

            db.Set<TEntity>().Remove(entity);
            return await db.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            await db.Set<TEntity>().AddRangeAsync(entities, ct);
            await db.SaveChangesAsync(ct);
        }
    }
}
