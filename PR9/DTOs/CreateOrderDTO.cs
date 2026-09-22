using System;
using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class CreateOrderDTO
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public DateOnly DateOrder { get; set; }
}
