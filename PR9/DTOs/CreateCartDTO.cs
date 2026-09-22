using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class CreateCartDTO
{
    [Required]
    public int UserId { get; set; }
}
