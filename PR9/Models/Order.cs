using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR9.Models;

public partial class Order
{
    public int Id { get; set; }

    [NotMapped]
    public int IdOrder
    {
        get => Id;
        set => Id = value;
    }

    public int UserId { get; set; }

    public DateOnly DateOrder { get; set; }

    [NotMapped]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
