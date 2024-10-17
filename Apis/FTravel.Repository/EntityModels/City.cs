﻿using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class City : BaseEntity
{
    public string? Name { get; set; }

    public string? UnsignName { get; set; }

    public int Code { get; set; }

    public virtual ICollection<Route> RouteEndPointNavigations { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteStartPointNavigations { get; set; } = new List<Route>();

    public virtual ICollection<Station> Stations { get; set; } = new List<Station>();
}
