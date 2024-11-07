using FTravel.Service.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.TripModels
{
    public class UpdateTripStatusModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public TripStatus Status { get; set; }
    }
}
