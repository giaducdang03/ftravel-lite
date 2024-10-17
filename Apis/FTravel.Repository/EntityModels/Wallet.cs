using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Wallet : BaseEntity
{
    public int? UserId { get; set; }

    public int? AccountBalance { get; set; }

    public string? Status { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
