using PR9.Models;
namespace PR9.Repositories;
public interface IRoleRepository
{
    Task<List<Role>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<Role?> GetByIdAsync(int id, CancellationToken ct);
    Task AddAsync(Role entity, CancellationToken ct);
    void Update(Role entity);
    void Remove(Role entity);
    Task SaveChangesAsync(CancellationToken ct);
}
