using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Station : BaseEntity
{
    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Status { get; set; }

    public int? CityId { get; set; }

    public string? UnsignName { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<RouteStation> RouteStations { get; set; } = new List<RouteStation>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
