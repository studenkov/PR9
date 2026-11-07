using Microsoft.EntityFrameworkCore;
using PR9.Models;

namespace PR9.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    public CartRepository(AppDbContext context) => _context = context;
    public Task<List<Cart>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.Carts.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Carts.CountAsync(ct);
    public Task<Cart?> GetByIdAsync(int id, CancellationToken ct) => _context.Carts.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<User?> FindUserAsync(int id, CancellationToken ct) => _context.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(Cart entity, CancellationToken ct) => _context.Carts.AddAsync(entity, ct).AsTask();
    public void Update(Cart entity) => _context.Carts.Update(entity);
    public void Remove(Cart entity) => _context.Carts.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}