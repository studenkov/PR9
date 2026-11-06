using AutoMapper;
using PR9.DTOs;
using PR9.Models;
using PR9.Mapping;
using PR9.Repositories;

namespace PR9.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    public ProductService(IProductRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }

    public async Task<(List<ProductDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100);
        var rows = await _repository.GetAllAsync(page, pageSize, ct);
        return (ProductMapper.ToDtos(rows), await _repository.CountAsync(ct));
    }
    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(id, ct);
        return product is null ? null : ProductMapper.ToDto(product);
    }
    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct)
    {
        if (await _repository.FindCategoryAsync(dto.CategoryId, ct) is null) throw MissingRelated(dto.CategoryId);
        var product = _mapper.Map<Product>(dto);
        await _repository.AddAsync(product); await _repository.SaveChangesAsync(ct);
        return ProductMapper.ToDto(await _repository.GetByIdAsync(product.Id, ct) ?? product);
    }
    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(id, ct); if (product is null) return null;
        if (await _repository.FindCategoryAsync(dto.CategoryId, ct) is null) throw MissingRelated(dto.CategoryId);
        _mapper.Map(dto, product); _repository.Update(product); await _repository.SaveChangesAsync(ct);
        return ProductMapper.ToDto(await _repository.GetByIdAsync(id, ct) ?? product);
    }
    public async Task<ProductDto?> PatchAsync(int id, PatchProductDto dto, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(id, ct); if (product is null) return null;
        if (dto.CategoryId is int categoryId && await _repository.FindCategoryAsync(categoryId, ct) is null) throw MissingRelated(categoryId);
        if (dto.Title is not null) product.Title = dto.Title;
        if (dto.Price is decimal price) product.Price = price;
        if (dto.CategoryId is int catId) { product.CategoryId = catId; product.Category = null; }
        if (dto.ImagePath is not null) product.ImagePath = dto.ImagePath;
        if (dto.StockQuantity is int stock) product.StockQuantity = stock;
        _repository.Update(product); await _repository.SaveChangesAsync(ct);
        return ProductMapper.ToDto(await _repository.GetByIdAsync(id, ct) ?? product);
    }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(id, ct); if (product is null) return false;
        _repository.Remove(product); await _repository.SaveChangesAsync(ct); return true;
    }
    public async Task<ProductReservationDto> ReserveProductAsync(int productId, int userId, int quantity, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(productId, ct) ?? throw new InvalidOperationException($"Товар с Id {productId} не найден.");
        if (await _repository.FindUserAsync(userId, ct) is null) throw MissingRelated(userId);
        if (quantity <= 0) throw new InvalidOperationException("Количество должно быть больше нуля.");
        if (product.StockQuantity < quantity) throw new InvalidOperationException("Недостаточно товара на складе.");
        product.StockQuantity -= quantity;
        var reservation = new ProductReservation { Product = product, ProductId = productId, UserId = userId, Quantity = quantity, ReservedAt = DateTime.UtcNow, IsActive = true };
        await _repository.AddReservationAsync(reservation); await _repository.SaveChangesAsync(ct);
        return _mapper.Map<ProductReservationDto>(reservation);
    }
    public async Task<ProductReservationDto?> CancelReservationAsync(int reservationId, CancellationToken ct)
    {
        var reservation = await _repository.GetReservationByIdAsync(reservationId, ct);
        if (reservation is null || !reservation.IsActive) return null;
        reservation.Product.StockQuantity += reservation.Quantity; reservation.IsActive = false;
        await _repository.SaveChangesAsync(ct); return _mapper.Map<ProductReservationDto>(reservation);
    }
    private static InvalidOperationException MissingRelated(int id) => new($"Связанная сущность с Id {id} не найдена.");
}
