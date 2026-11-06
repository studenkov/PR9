using PR9.Models;
namespace PR9.Repositories;
public interface ICartRepository
{
    Task<List<Cart>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<Cart?> GetByIdAsync(int id, CancellationToken ct);
    Task<User?> FindUserAsync(int id, CancellationToken ct);
    Task AddAsync(Cart entity, CancellationToken ct);
    void Update(Cart entity);
    void Remove(Cart entity);
    Task SaveChangesAsync(CancellationToken ct);
}
