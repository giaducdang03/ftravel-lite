﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTravel.Service.BusinessModels.TicketModels;

namespace FTravel.Service.BusinessModels.TripModels
{
    public class TripModel
    {
        public int Id { get; set; }
        public string? UnsignName { get; set; }
        public string Name { get; set; }
        public int RouteId {  get; set; }
        public string? RouteName { get; set; }
        public int LowestPrice {  get; set; }
        public DateTime? OpenTicketDate { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string? Status { get; set; }
        public bool? IsTemplate { get; set; }
        public int? DriverId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<TicketModel> Tickets { get; set; }
        public List<TripServiceModel> Services { get; set; }
    }
}
