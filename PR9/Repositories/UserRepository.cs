using Microsoft.EntityFrameworkCore;
using PR9.Models;

namespace PR9.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;
    public Task<List<User>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.Users.Include(x => x.Role).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Users.CountAsync(ct);
    public Task<User?> GetByIdAsync(int id, CancellationToken ct) => _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<Role?> FindRoleAsync(int id, CancellationToken ct) => _context.Roles.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(User entity, CancellationToken ct) => _context.Users.AddAsync(entity, ct).AsTask();
    public void Update(User entity) => _context.Users.Update(entity);
    public void Remove(User entity) => _context.Users.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}