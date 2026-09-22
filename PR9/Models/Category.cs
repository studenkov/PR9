using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR9.Models;

public partial class Category
{
    public int Id { get; set; }

    [NotMapped]
    public int IdCategory
    {
        get => Id;
        set => Id = value;
    }

    public string Title { get; set; } = null!;

    [NotMapped]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
