using Microsoft.EntityFrameworkCore;
using PR9.Data;
using PR9.Models;

namespace PR9.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly OnlineStoreContext _context;
    public ProductRepository(OnlineStoreContext context) => _context = context;

    public Task<List<Product>> GetAllAsync(int page, int pageSize, CancellationToken ct) =>
        _context.Products.Include(p => p.Category).OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Products.CountAsync(ct);
    public Task<Product?> GetByIdAsync(int id, CancellationToken ct) =>
        _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, ct);
    public Task<Category?> FindCategoryAsync(int categoryId, CancellationToken ct) => _context.Categories.FindAsync([categoryId], ct).AsTask();
    public Task<User?> FindUserAsync(int userId, CancellationToken ct) => _context.Users.FindAsync([userId], ct).AsTask();
    public Task AddAsync(Product product) => _context.Products.AddAsync(product).AsTask();
    public void Update(Product product) => _context.Products.Update(product);
    public void Remove(Product product) => _context.Products.Remove(product);
    public Task AddReservationAsync(ProductReservation reservation) => _context.ProductReservations.AddAsync(reservation).AsTask();
    public Task<ProductReservation?> GetReservationByIdAsync(int id, CancellationToken ct) =>
        _context.ProductReservations.Include(r => r.Product).FirstOrDefaultAsync(r => r.Id == id, ct);
    public async Task SaveChangesAsync(CancellationToken ct) => await _context.SaveChangesAsync(ct);
}
