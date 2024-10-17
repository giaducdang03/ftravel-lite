﻿using FTravel.Service.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.TripModels
{
    public class CreateTripModel
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public int RouteId { get; set; }
        [Required]
        public DateTime OpenTicketDate { get; set; }
        [Required]
        public DateTime EstimatedStartDate { get; set; }

        public DateTime? EstimatedEndDate { get; set; }

        public int? DriverId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one TicketTypeId is required.")]
        public List<int> TicketTypeIds { get; set; } = new List<int>();
        [Required]
        public List<CreateTripServiceModel> TripServices { get; set; } = new List<CreateTripServiceModel>();
        [Required]
        public List<CreateTicketTripModel> TripTickets { get; set; } = new List<CreateTicketTripModel>();
    }
}
