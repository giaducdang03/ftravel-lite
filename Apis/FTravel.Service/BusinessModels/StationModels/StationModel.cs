using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.StationModels
{
    public class StationModel : BaseEntity
    {
        public string Name { get; set; } = "";

        public string Address { get; set; } = "";

        public string CityName { get; set; } = "";

        public string? Status { get; set; }
    }
}
