using PR9.Models;
namespace PR9.Repositories;
public interface IOrderItemRepository
{
    Task<List<OrderItem>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<OrderItem?> GetByIdAsync(int id, CancellationToken ct);
    Task<Order?> FindOrderAsync(int id, CancellationToken ct);
    Task<Product?> FindProductAsync(int id, CancellationToken ct);
    Task AddAsync(OrderItem entity, CancellationToken ct);
    void Update(OrderItem entity);
    void Remove(OrderItem entity);
    Task SaveChangesAsync(CancellationToken ct);
}
