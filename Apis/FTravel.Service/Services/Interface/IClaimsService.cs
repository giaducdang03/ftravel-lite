﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IClaimsService
    {
        public string GetCurrentUserEmail { get; }
    }
}
