using Microsoft.EntityFrameworkCore;
using PR9.Data;
using PR9.Models;

namespace PR9.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly OnlineStoreContext _context;
    public CategoryRepository(OnlineStoreContext context) => _context = context;
    public Task<List<Category>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.Categories.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Categories.CountAsync(ct);
    public Task<Category?> GetByIdAsync(int id, CancellationToken ct) => _context.Categories.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(Category entity, CancellationToken ct) => _context.Categories.AddAsync(entity, ct).AsTask();
    public void Update(Category entity) => _context.Categories.Update(entity);
    public void Remove(Category entity) => _context.Categories.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}

public class RoleRepository : IRoleRepository
{
    private readonly OnlineStoreContext _context;
    public RoleRepository(OnlineStoreContext context) => _context = context;
    public Task<List<Role>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.Roles.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Roles.CountAsync(ct);
    public Task<Role?> GetByIdAsync(int id, CancellationToken ct) => _context.Roles.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(Role entity, CancellationToken ct) => _context.Roles.AddAsync(entity, ct).AsTask();
    public void Update(Role entity) => _context.Roles.Update(entity);
    public void Remove(Role entity) => _context.Roles.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}

public class UserRepository : IUserRepository
{
    private readonly OnlineStoreContext _context;
    public UserRepository(OnlineStoreContext context) => _context = context;
    public Task<List<User>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.Users.Include(x => x.Role).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Users.CountAsync(ct);
    public Task<User?> GetByIdAsync(int id, CancellationToken ct) => _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<Role?> FindRoleAsync(int id, CancellationToken ct) => _context.Roles.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(User entity, CancellationToken ct) => _context.Users.AddAsync(entity, ct).AsTask();
    public void Update(User entity) => _context.Users.Update(entity);
    public void Remove(User entity) => _context.Users.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}

public class CartRepository : ICartRepository
{
    private readonly OnlineStoreContext _context;
    public CartRepository(OnlineStoreContext context) => _context = context;
    public Task<List<Cart>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.Carts.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Carts.CountAsync(ct);
    public Task<Cart?> GetByIdAsync(int id, CancellationToken ct) => _context.Carts.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<User?> FindUserAsync(int id, CancellationToken ct) => _context.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(Cart entity, CancellationToken ct) => _context.Carts.AddAsync(entity, ct).AsTask();
    public void Update(Cart entity) => _context.Carts.Update(entity);
    public void Remove(Cart entity) => _context.Carts.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}

public class CartItemRepository : ICartItemRepository
{
    private readonly OnlineStoreContext _context;
    public CartItemRepository(OnlineStoreContext context) => _context = context;
    public Task<List<CartItem>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.CartItems.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.CartItems.CountAsync(ct);
    public Task<CartItem?> GetByIdAsync(int id, CancellationToken ct) => _context.CartItems.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<Cart?> FindCartAsync(int id, CancellationToken ct) => _context.Carts.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<Product?> FindProductAsync(int id, CancellationToken ct) => _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(CartItem entity, CancellationToken ct) => _context.CartItems.AddAsync(entity, ct).AsTask();
    public void Update(CartItem entity) => _context.CartItems.Update(entity);
    public void Remove(CartItem entity) => _context.CartItems.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}

public class OrderRepository : IOrderRepository
{
    private readonly OnlineStoreContext _context;
    public OrderRepository(OnlineStoreContext context) => _context = context;
    public Task<List<Order>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.Orders.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Orders.CountAsync(ct);
    public Task<Order?> GetByIdAsync(int id, CancellationToken ct) => _context.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<User?> FindUserAsync(int id, CancellationToken ct) => _context.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(Order entity, CancellationToken ct) => _context.Orders.AddAsync(entity, ct).AsTask();
    public void Update(Order entity) => _context.Orders.Update(entity);
    public void Remove(Order entity) => _context.Orders.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}

public class OrderItemRepository : IOrderItemRepository
{
    private readonly OnlineStoreContext _context;
    public OrderItemRepository(OnlineStoreContext context) => _context = context;
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
