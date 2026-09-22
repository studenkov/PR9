using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class UpdateCartItemDTO
{
    [Required]
    public int CartId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, 10000)]
    public int Quantity { get; set; }
}
