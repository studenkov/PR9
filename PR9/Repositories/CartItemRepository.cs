using Microsoft.EntityFrameworkCore;
using PR9.Models;

namespace PR9.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly AppDbContext _context;
    public CartItemRepository(AppDbContext context) => _context = context;

    public Task<List<CartItem>> GetAllAsync(int page, int pageSize, int? userId, CancellationToken ct)
    {
        var query = _context.CartItems.AsQueryable();
        if (userId.HasValue)
        {
            var userCartIds = _context.Carts.Where(c => c.UserId == userId.Value).Select(c => c.Id);
            query = query.Where(ci => userCartIds.Contains(ci.CartId));
        }
        return query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    }

    public Task<int> CountAsync(int? userId, CancellationToken ct)
    {
        var query = _context.CartItems.AsQueryable();
        if (userId.HasValue)
        {
            var userCartIds = _context.Carts.Where(c => c.UserId == userId.Value).Select(c => c.Id);
            query = query.Where(ci => userCartIds.Contains(ci.CartId));
        }
        return query.CountAsync(ct);
    }

    public Task<CartItem?> GetByIdAsync(int id, CancellationToken ct) => _context.CartItems.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<Cart?> FindCartAsync(int id, CancellationToken ct) => _context.Carts.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<int?> GetCartUserIdAsync(int cartId, CancellationToken ct)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(x => x.Id == cartId, ct);
        return cart?.UserId;
    }
    public Task<Product?> FindProductAsync(int id, CancellationToken ct) => _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(CartItem entity, CancellationToken ct) => _context.CartItems.AddAsync(entity, ct).AsTask();
    public void Update(CartItem entity) => _context.CartItems.Update(entity);
    public void Remove(CartItem entity) => _context.CartItems.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}