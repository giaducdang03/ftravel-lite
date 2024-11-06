using FTravel.API.ViewModels.ResponseModels;
using FTravel.Service.BusinessModels.OrderModels;
using FTravel.Service.BusinessModels.TicketModels;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FTravel.API.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;


        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            try
            {
                var result = await _ticketService.GetTicketByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không có ticket phù hợp"
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("cancel/{id}")]
        [Authorize]
        public async Task<IActionResult> CancelTicketById(int id)
        {
            try
            {
                var result = await _ticketService.CancelTicketAsync(id);

                if (!result)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Có lỗi trong quá trình hủy vé. Vui lòng thử lại sau."
                    });
                }
                return Ok(new ResponseModel
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Đã hủy vé thành công. Xem chi tiết ở thông báo."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
    }
}
