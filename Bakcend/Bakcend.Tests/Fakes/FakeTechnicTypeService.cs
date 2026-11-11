using Backend.Models;
using Backend.Services;

namespace Backend.Tests.Fakes;

internal sealed class FakeTechnicTypeService : ITechnicTypeService
{
    public IReadOnlyList<TechnicType> All { get; set; } = new List<TechnicType>();
    public TechnicType? ById { get; set; }
    public Exception? AddException { get; set; }
    public Exception? UpdateException { get; set; }
    public bool DeleteResult { get; set; } = true;
    public TechnicType? Added { get; private set; }

    public Task<IReadOnlyList<TechnicType>> GetAll(CancellationToken cancellationToken) => Task.FromResult(All);

    public Task<TechnicType?> GetById(int id, CancellationToken cancellationToken) => Task.FromResult(ById);

    public Task<TechnicType> Add(TechnicType technic, CancellationToken cancellationToken)
    {
        if (AddException is not null)
        {
            throw AddException;
        }

        Added = technic;
        return Task.FromResult(technic);
    }

    public Task<bool> Update(TechnicType technic, CancellationToken cancellationToken)
    {
        if (UpdateException is not null)
        {
            throw UpdateException;
        }

        return Task.FromResult(true);
    }

    public Task<bool> Delete(int id, CancellationToken cancellationToken) => Task.FromResult(DeleteResult);

    public Task AddRange(List<TechnicType> found, CancellationToken ct) => Task.CompletedTask;
}
