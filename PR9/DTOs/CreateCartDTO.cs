using System.ComponentModel.DataAnnotations;

namespace PR9.Dtos;

public class CreateCartDTO
{
    [Required]
    public int UserId { get; set; }
}
