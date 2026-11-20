using PR9.Dtos;
using PR9.Models;

namespace PR9.Services;

public interface IUserService
{
    Task<AppUser> RegisterAsync(string login, string password, string role = "Customer", CancellationToken ct = default);
    Task<AppUser?> ValidateCredentialsAsync(string login, string password, CancellationToken ct = default);
    Task<(List<UserDTO> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<UserDTO?> GetByIdAsync(int id, CancellationToken ct);
    Task<UserDTO> CreateAsync(CreateUserDTO dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateUserDTO dto, CancellationToken ct);
    Task<bool> PatchAsync(int id, PatchUserDTO dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
