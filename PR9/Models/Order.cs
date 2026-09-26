using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR9.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateOnly DateOrder { get; set; }

    [NotMapped]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
