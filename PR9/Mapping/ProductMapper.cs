using PR9.DTOs;
using PR9.Models;

namespace PR9.Mapping;

public static class ProductMapper
{
    public static ProductDto ToDto(Product product) => new()
    {
        Id = product.Id,
        Title = product.Title,
        Price = product.Price,
        CategoryId = product.CategoryId,
        Category = product.Category is null ? null : new CategoryDto
        {
            Id = product.Category.Id,
            Title = product.Category.Title,
            CreatedAt = product.Category.CreatedAt
        },
        ImagePath = product.ImagePath,
        StockQuantity = product.StockQuantity,
        CreatedAt = product.CreatedAt
    };

    public static List<ProductDto> ToDtos(IEnumerable<Product> products) => products.Select(ToDto).ToList();
}
