using System;

namespace PR9.DTOs;

public class OrderDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateOnly DateOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}
