using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Order : BaseEntity
{
    public string Code { get; set; } = null!;

    public int? TotalPrice { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentStatus { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
