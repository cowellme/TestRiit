using Backend.Models;

namespace Backend.Services
{
    public interface ITechnicTypeService
    {
        Task<IReadOnlyList<TechnicType>> GetAll(CancellationToken cancellationToken);
        Task<TechnicType?> GetById(int id, CancellationToken cancellationToken);
        Task<TechnicType> Add(TechnicType technic, CancellationToken cancellationToken);
        Task<bool> Update(TechnicType technic, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
        Task AddRange(List<TechnicType> found, CancellationToken ct);
    }

    public class TechnicTypeService(IRepository<TechnicType, int> repository) : ITechnicTypeService
    {
        public async Task<IReadOnlyList<TechnicType>> GetAll(CancellationToken cancellationToken) =>
            await repository.GetAllAsync(cancellationToken);

        public async Task<TechnicType?> GetById(int id, CancellationToken cancellationToken) =>
            await repository.GetByIdAsync(id, cancellationToken);

        public async Task<TechnicType> Add(TechnicType technic, CancellationToken cancellationToken) =>
            await repository.AddAsync(technic, cancellationToken);

        public async Task<bool> Update(TechnicType technic, CancellationToken cancellationToken) =>
            await repository.UpdateAsync(technic, cancellationToken);

        public async Task<bool> Delete(int id, CancellationToken cancellationToken) =>
            await repository.DeleteAsync(id, cancellationToken);

        public async Task AddRange(List<TechnicType> found, CancellationToken ct) =>
            await repository.AddRangeAsync(found, ct);
    }
}