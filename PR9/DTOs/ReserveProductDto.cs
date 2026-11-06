using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class ReserveProductDto
{
    [Range(1, int.MaxValue)] public int UserId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
}
