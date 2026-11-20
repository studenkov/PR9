using System.ComponentModel.DataAnnotations;

namespace PR9.Dtos;

public class UpdateCartDTO
{
    [Required]
    public int UserId { get; set; }
}
