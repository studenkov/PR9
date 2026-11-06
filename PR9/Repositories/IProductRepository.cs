using PR9.Models;

namespace PR9.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct);
    Task<Category?> FindCategoryAsync(int categoryId, CancellationToken ct);
    Task<User?> FindUserAsync(int userId, CancellationToken ct);
    Task AddAsync(Product product);
    void Update(Product product);
    void Remove(Product product);
    Task AddReservationAsync(ProductReservation reservation);
    Task<ProductReservation?> GetReservationByIdAsync(int id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
