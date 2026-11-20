using Microsoft.EntityFrameworkCore;
using PR9.Models;

namespace PR9.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<AppUser>> GetAllAsync(int page, int pageSize, CancellationToken ct) =>
        _context.Users
            .Include(x => x.RoleNavigation)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public Task<int> CountAsync(CancellationToken ct) =>
        _context.Users.CountAsync(ct);

    public Task<AppUser?> GetByIdAsync(int id, CancellationToken ct) =>
        _context.Users
            .Include(x => x.RoleNavigation)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<AppUser?> GetByLoginAsync(string login, CancellationToken ct) =>
        _context.Users
            .Include(x => x.RoleNavigation)
            .FirstOrDefaultAsync(x => x.Login == login, ct);

    public Task<bool> ExistsByLoginAsync(string login, CancellationToken ct) =>
        _context.Users.AnyAsync(x => x.Login == login, ct);

    public Task<Role?> FindRoleAsync(int id, CancellationToken ct) =>
        _context.Roles.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task AddAsync(AppUser entity, CancellationToken ct) =>
        _context.Users.AddAsync(entity, ct).AsTask();

    public void Update(AppUser entity) =>
        _context.Users.Update(entity);

    public void Remove(AppUser entity) =>
        _context.Users.Remove(entity);

    public Task SaveChangesAsync(CancellationToken ct) =>
        _context.SaveChangesAsync(ct);
}
