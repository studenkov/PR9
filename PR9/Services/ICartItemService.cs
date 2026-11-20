using PR9.Dtos;

namespace PR9.Services;

public interface ICartItemService
{
    Task<(List<CartItemDTO> Items, int TotalCount)> GetAllAsync(int page, int pageSize, int? userId, CancellationToken ct);
    Task<CartItemDTO?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> CanAccessCartAsync(int cartId, int userId, CancellationToken ct);
    Task<bool> CanAccessCartItemAsync(int cartItemId, int userId, CancellationToken ct);
    Task<CartItemDTO> CreateAsync(CreateCartItemDTO dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateCartItemDTO dto, CancellationToken ct);
    Task<bool> PatchAsync(int id, PatchCartItemDTO dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
