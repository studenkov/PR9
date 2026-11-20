using System.ComponentModel.DataAnnotations;

namespace PR9.Dtos;

public class ReserveProductDto
{
    [Range(1, int.MaxValue)] public int UserId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
}
