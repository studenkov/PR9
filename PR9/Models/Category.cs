using System;

namespace PR9.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
