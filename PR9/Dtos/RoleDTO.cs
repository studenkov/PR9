using System;

namespace PR9.Dtos;

public class RoleDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
