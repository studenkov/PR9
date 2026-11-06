using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class UpdateProductDto
{
    [Required, StringLength(100)] public string Title { get; set; } = null!;
    [Range(0.01, 1000000)] public decimal Price { get; set; }
    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
    public string? ImagePath { get; set; }
    [Range(0, int.MaxValue)] public int StockQuantity { get; set; }
}

public class UpdateProductDTO : UpdateProductDto { }
