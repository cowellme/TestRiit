using Backend.Models;

namespace Backend.Services
{
    public interface ITechnicService
    {
        Task<IReadOnlyList<Technic>> GetAll(CancellationToken cancellationToken);
        Task<Technic?> GetById(int id, CancellationToken cancellationToken);
        Task<Technic> Add(Technic technic, CancellationToken cancellationToken);
        Task<bool> Update(Technic technic, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
        Task AddRange(List<Technic> found, CancellationToken ct);
    }

    public class TechnicService(IRepository<Technic, int> repository) : ITechnicService
    {
        public async Task<IReadOnlyList<Technic>> GetAll(CancellationToken cancellationToken) =>
            await repository.GetAllAsync(cancellationToken);

        public async Task<Technic?> GetById(int id, CancellationToken cancellationToken) =>
            await repository.GetByIdAsync(id, cancellationToken);

        public async Task<Technic?> Add(Technic technic, CancellationToken cancellationToken)
        {
            if (await repository.CountAsync(cancellationToken) >= 2000)
            {
                throw new InvalidOperationException("Лимит не более 2000 записей");
            }

            return await repository.AddAsync(technic, cancellationToken);
        }

        public async Task<bool> Update(Technic technic, CancellationToken cancellationToken) =>
            await repository.UpdateAsync(technic, cancellationToken);

        public async Task<bool> Delete(int id, CancellationToken cancellationToken) =>
            await repository.DeleteAsync(id, cancellationToken);

        public async Task AddRange(List<Technic> found, CancellationToken cancellationToken)
        {
            if (await repository.CountAsync(cancellationToken) + found.Count >= 2000)
            {
                throw new InvalidOperationException("Лимит не более 2000 записей");
            }

            await repository.AddRangeAsync(found, cancellationToken);
        }
    }
}
