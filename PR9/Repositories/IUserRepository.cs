using PR9.Models;
namespace PR9.Repositories;
public interface IUserRepository
{
    Task<List<User>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<User?> GetByIdAsync(int id, CancellationToken ct);
    Task<Role?> FindRoleAsync(int id, CancellationToken ct);
    Task AddAsync(User entity, CancellationToken ct);
    void Update(User entity);
    void Remove(User entity);
    Task SaveChangesAsync(CancellationToken ct);
}
