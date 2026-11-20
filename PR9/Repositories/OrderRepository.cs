using Microsoft.EntityFrameworkCore;
using PR9.Models;

namespace PR9.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Order>> GetAllAsync(int page, int pageSize, int? userId, CancellationToken ct)
    {
        var query = _context.Orders.AsQueryable();
        if (userId.HasValue)
        {
            query = query.Where(x => x.UserId == userId.Value);
        }
        return query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    }

    public Task<int> CountAsync(int? userId, CancellationToken ct)
    {
        var query = _context.Orders.AsQueryable();
        if (userId.HasValue)
        {
            query = query.Where(x => x.UserId == userId.Value);
        }
        return query.CountAsync(ct);
    }

    public Task<Order?> GetByIdAsync(int id, CancellationToken ct) =>
        _context.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<AppUser?> FindUserAsync(int id, CancellationToken ct) =>
        _context.Users.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task AddAsync(Order entity, CancellationToken ct) =>
        _context.Orders.AddAsync(entity, ct).AsTask();

    public void Update(Order entity) =>
        _context.Orders.Update(entity);

    public void Remove(Order entity) =>
        _context.Orders.Remove(entity);

    public Task SaveChangesAsync(CancellationToken ct) =>
        _context.SaveChangesAsync(ct);
}
