using System;

namespace PR9.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
}
