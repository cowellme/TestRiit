using Backend.Services;

namespace Backend.Tests.Fakes;

internal sealed class FakeRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : notnull
{
    public int Count { get; set; }
    public List<TEntity> All { get; } = new();
    public int AddCalls { get; private set; }
    public int AddRangeCalls { get; private set; }
    public TEntity? GetByIdResult { get; set; }

    public Task<int> CountAsync(CancellationToken cancellationToken) => Task.FromResult(Count);

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) => Task.FromResult(All);

    public ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(GetByIdResult);

    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        AddCalls++;
        return Task.FromResult(entity);
    }

    public Task<bool> UpdateAsync(TEntity entity, CancellationToken ct = default) => Task.FromResult(true);

    public Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default) => Task.FromResult(true);

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct)
    {
        AddRangeCalls++;
        return Task.CompletedTask;
    }
}