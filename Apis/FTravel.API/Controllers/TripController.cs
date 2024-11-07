using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.TripModels;
using FTravel.Service.Enums;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IClaimsService _claimsService;

        public TripController(ITripService tripService, IClaimsService claimsService)
        {
            _tripService = tripService;
            _claimsService = claimsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTripStatusOpening([FromQuery] PaginationParameter paginationParameter, TripFilter filter)
        {
            try
            {
                var result = await _tripService.GetAllTripAsync(paginationParameter, filter);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy chuyến xe!"
                    });
                }
                else
                {
                    var metadata = new
                    {
                        result.TotalCount,
                        result.PageSize,
                        result.CurrentPage,
                        result.TotalPages,
                        result.HasNext,
                        result.HasPrevious
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
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

        [HttpGet("staff")]
        [Authorize(Roles = "STAFF")]
        public async Task<IActionResult> GetAllTripStaff([FromQuery] PaginationParameter paginationParameter, TripFilter filter)
        {
            try
            {
                var email = _claimsService.GetCurrentUserEmail;

                var result = await _tripService.GetTripStaffAsync(paginationParameter, filter, email);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy chuyến xe!"
                    });
                }
                else
                {
                    var metadata = new
                    {
                        result.TotalCount,
                        result.PageSize,
                        result.CurrentPage,
                        result.TotalPages,
                        result.HasNext,
                        result.HasPrevious
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
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

        [HttpGet("{id}")]
        [Authorize]

        public async Task<IActionResult> GetTripDetailByIdStatusOpening(int id)
        {
            try
            {
                var result = await _tripService.GetTripByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm tháy chuyến xe!"
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
        [HttpPost()]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddTrip([FromBody] CreateTripModel tripModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _tripService.CreateTripAsync(tripModel);
                if (result)
                {
                    return Ok(
                        new ResponseModel
                        {
                            HttpCode = StatusCodes.Status200OK,
                            Message = "Tạo chuyến xe mới thành công!"
                        });
                }
                else
                {
                    return BadRequest(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Xảy ra lỗi khi tạo chuyến xe mới!"
                    });
                }
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
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateTrip(UpdateTripModel tripModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _tripService.UpdateTripAsync(tripModel);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Cập nhật chuyến xe thành công"
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Xảy ra lỗi khi cập nhật chuyến xe!"
                    });
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel
                {
                    HttpCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
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
        [HttpPut("status")]
        [Authorize(Roles = "ADMIN,STAFF")]
        public async Task<IActionResult> UpdateTripStatus(UpdateTripStatusModel updateTripStatus)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _tripService.UpdateTripStatusAsyncV2(updateTripStatus);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Cập nhật trạng thái chuyến xe thành công"
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Xảy ra lỗi khi cập nhật trạng thái chuyến xe!"
                    });
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel
                {
                    HttpCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
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
        //[HttpPut("{id}/cancel")]
        //[Authorize(Roles = "ADMIN")]
        //public async Task<IActionResult> CancelTrip(int id, string status)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var result = await _tripService.CancelTripAsync(id, status);
        //        if (result)
        //        {
        //            return Ok("Xóa chuyến xe thành công!");
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel
        //            {
        //                HttpCode = StatusCodes.Status400BadRequest,
        //                Message = "Xảy ra lỗi khi xóa chuyến xe!"
        //            });
        //        }
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ResponseModel
        //        {
        //            HttpCode = StatusCodes.Status404NotFound,
        //            Message = ex.Message
        //        });
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = StatusCodes.Status400BadRequest,
        //            Message = ex.Message
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = StatusCodes.Status400BadRequest,
        //            Message = ex.Message
        //        });
        //    }
        //}
        [HttpGet("template")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetTemplateTrip()
        {
            try
            {
                var result = await _tripService.GetTemplateTripAsync();

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm tháy chuyến xe mẫu!"
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
    }
}

