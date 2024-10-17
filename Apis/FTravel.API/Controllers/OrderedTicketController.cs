﻿//using FTravel.API.ViewModels.ResponseModels;
//using FTravel.Service.Services.Interface;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace FTravel.API.Controllers
//{
//    [Route("api/orderedticket")]
//    [ApiController]
//    public class OrderedTicketController : ControllerBase
//    {
//        private readonly IOrderedTicketService _orderedTicketService;

//        public OrderedTicketController(IOrderedTicketService orderedTicketService)
//        {
//            _orderedTicketService = orderedTicketService;
//        }


//        [HttpGet("by-cutomerid/{cusid}")]
//        public async Task<IActionResult> GetOrderedTicketByCustomerId(int cusid)
//        {
//            try
//            {
//                var data = await _orderedTicketService.GetAllOrderedTicketByCustomerIdService(cusid);
//                if (cusid == null)
//                {
//                    return BadRequest();
//                }
//                return Ok(data);
//            }
//            catch (Exception ex)
//            {

//                return BadRequest(new ResponseModel
//                {
//                    HttpCode = 400,
//                    Message = ex.Message
//                });
//            }

//        }
//        [HttpGet("historytrip/{cusid}")]
//        public async Task<IActionResult> GetHistoryOfTripsTakenByCustomerId(int cusid)
//        {
//            try
//            {
//                var data = await _orderedTicketService.GetHistoryOfTripsTakenByCustomerIdService(cusid);
//                if (cusid == null)
//                {
//                    return BadRequest();
//                }
//                return Ok(data);
//            }
//            catch (Exception ex)
//            {

//                return BadRequest(new ResponseModel
//                {
//                    HttpCode = 400,
//                    Message = ex.Message
//                });
//            }

//        }
//    }
//}
