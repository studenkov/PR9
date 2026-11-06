using PR9.DTOs;

namespace PR9.Services;

public interface IProductService
{
    Task<(List<ProductDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct);
    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct);
    Task<ProductDto?> PatchAsync(int id, PatchProductDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<ProductReservationDto> ReserveProductAsync(int productId, int userId, int quantity, CancellationToken ct);
    Task<ProductReservationDto?> CancelReservationAsync(int reservationId, CancellationToken ct);
}

public class ProductReservationDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public DateTime ReservedAt { get; set; }
    public bool IsActive { get; set; }
}
