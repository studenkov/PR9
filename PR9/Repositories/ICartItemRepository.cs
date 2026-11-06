using PR9.Models;
namespace PR9.Repositories;
public interface ICartItemRepository
{
    Task<List<CartItem>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<CartItem?> GetByIdAsync(int id, CancellationToken ct);
    Task<Cart?> FindCartAsync(int id, CancellationToken ct);
    Task<Product?> FindProductAsync(int id, CancellationToken ct);
    Task AddAsync(CartItem entity, CancellationToken ct);
    void Update(CartItem entity);
    void Remove(CartItem entity);
    Task SaveChangesAsync(CancellationToken ct);
}
