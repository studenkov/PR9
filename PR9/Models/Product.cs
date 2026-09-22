using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR9.Models;

public partial class Product
{
    public int Id { get; set; }

    [NotMapped]
    public int IdProduct
    {
        get => Id;
        set => Id = value;
    }

    public string Title { get; set; } = null!;

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public string? ImagePath { get; set; }

    [NotMapped]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
