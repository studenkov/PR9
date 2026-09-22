using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class UpdateCartDTO
{
    [Required]
    public int UserId { get; set; }
}
