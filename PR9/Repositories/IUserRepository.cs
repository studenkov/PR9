using PR9.Models;

namespace PR9.Repositories;

public interface IUserRepository
{
    Task<List<AppUser>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<AppUser?> GetByIdAsync(int id, CancellationToken ct);
    Task<AppUser?> GetByLoginAsync(string login, CancellationToken ct);
    Task<bool> ExistsByLoginAsync(string login, CancellationToken ct);
    Task<Role?> FindRoleAsync(int id, CancellationToken ct);
    Task AddAsync(AppUser entity, CancellationToken ct);
    void Update(AppUser entity);
    void Remove(AppUser entity);
    Task SaveChangesAsync(CancellationToken ct);
}
