using PR9.Models;

namespace PR9.Repositories;

public interface ICartItemRepository
{
    Task<List<CartItem>> GetAllAsync(int page, int pageSize, int? userId, CancellationToken ct);
    Task<int> CountAsync(int? userId, CancellationToken ct);
    Task<CartItem?> GetByIdAsync(int id, CancellationToken ct);
    Task<Cart?> FindCartAsync(int id, CancellationToken ct);
    Task<int?> GetCartUserIdAsync(int cartId, CancellationToken ct);
    Task<Product?> FindProductAsync(int id, CancellationToken ct);
    Task AddAsync(CartItem entity, CancellationToken ct);
    void Update(CartItem entity);
    void Remove(CartItem entity);
    Task SaveChangesAsync(CancellationToken ct);
}
