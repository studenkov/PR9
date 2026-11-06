namespace PR9.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public CategoryDto? Category { get; set; }
    public string? ImagePath { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProductDTO : ProductDto { }
