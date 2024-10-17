using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Setting
{
    public int Id { get; set; }

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? Description { get; set; }
}
