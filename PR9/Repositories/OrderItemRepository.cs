using Microsoft.EntityFrameworkCore;
using PR9.Models;

namespace PR9.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly AppDbContext _context;
    public OrderItemRepository(AppDbContext context) => _context = context;
    public Task<List<OrderItem>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.OrderItems.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.OrderItems.CountAsync(ct);
    public Task<OrderItem?> GetByIdAsync(int id, CancellationToken ct) => _context.OrderItems.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<Order?> FindOrderAsync(int id, CancellationToken ct) => _context.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<Product?> FindProductAsync(int id, CancellationToken ct) => _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(OrderItem entity, CancellationToken ct) => _context.OrderItems.AddAsync(entity, ct).AsTask();
    public void Update(OrderItem entity) => _context.OrderItems.Update(entity);
    public void Remove(OrderItem entity) => _context.OrderItems.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}