using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Otp : BaseEntity
{
    public string Email { get; set; } = null!;

    public string OtpCode { get; set; } = null!;

    public DateTime ExpiryTime { get; set; }

    public bool IsUsed { get; set; }
}
