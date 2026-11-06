using PR9.Models;
namespace PR9.Repositories;
public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<Order?> GetByIdAsync(int id, CancellationToken ct);
    Task<User?> FindUserAsync(int id, CancellationToken ct);
    Task AddAsync(Order entity, CancellationToken ct);
    void Update(Order entity);
    void Remove(Order entity);
    Task SaveChangesAsync(CancellationToken ct);
}
