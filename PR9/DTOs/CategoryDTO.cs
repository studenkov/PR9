namespace PR9.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class CategoryDTO : CategoryDto { }
