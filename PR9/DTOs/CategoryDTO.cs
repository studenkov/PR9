using System;

namespace PR9.DTOs;

public class CategoryDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
