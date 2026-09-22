using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR9.Models;

public partial class Cart
{
    public int Id { get; set; }

    [NotMapped]
    public int IdCart
    {
        get => Id;
        set => Id = value;
    }

    public int UserId { get; set; }

    [NotMapped]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
