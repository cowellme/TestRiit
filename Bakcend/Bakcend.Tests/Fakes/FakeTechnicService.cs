using Backend.Models;
using Backend.Services;

namespace Backend.Tests.Fakes;

internal sealed class FakeTechnicService : ITechnicService
{
    public IReadOnlyList<Technic> All { get; set; } = new List<Technic>();
    public Technic? ById { get; set; }
    public Exception? AddException { get; set; }
    public Exception? UpdateException { get; set; }
    public bool DeleteResult { get; set; } = true;
    public Technic? Added { get; private set; }
    public Technic? Updated { get; private set; }

    public Task<IReadOnlyList<Technic>> GetAll(CancellationToken cancellationToken) => Task.FromResult(All);

    public Task<Technic?> GetById(int id, CancellationToken cancellationToken) => Task.FromResult(ById);

    public Task<Technic> Add(Technic technic, CancellationToken cancellationToken)
    {
        if (AddException is not null)
        {
            throw AddException;
        }

        Added = technic;
        return Task.FromResult(technic);
    }

    public Task<bool> Update(Technic technic, CancellationToken cancellationToken)
    {
        if (UpdateException is not null)
        {
            throw UpdateException;
        }

        Updated = technic;
        return Task.FromResult(true);
    }

    public Task<bool> Delete(int id, CancellationToken cancellationToken) => Task.FromResult(DeleteResult);

    public Task AddRange(List<Technic> found, CancellationToken ct) => Task.CompletedTask;
}