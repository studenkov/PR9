using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class UpdateProductDTO
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public string? ImagePath { get; set; }
}
