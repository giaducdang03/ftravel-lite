﻿using System.ComponentModel.DataAnnotations;

namespace FTravel.Service.BusinessModels.TripModels
{
    public class UpdateTripModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? ActualEndDate { get; set; }

        [Required]
        public string Status { get; set; }

        public int? DriverId { get; set; }

        public bool? IsTemplate { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one TicketTypeId is required.")]
        public List<int> TicketTypeIds { get; set; }


        public List<CreateTripServiceModel> TripServices { get; set; }
    }
}