﻿using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.ServiceModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{

    [Route("api/services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _service;
        public ServiceController(IServiceService service)
        {
            _service = service;
        }
        [HttpGet("by-route-id/{routeId}")]
        [Authorize]
        public async Task<IActionResult> GetServicesByRouteId(int routeId, [FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _service.GetAllServiceByRouteIdAsync(routeId, paginationParameter);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No service"
                    });
                }
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

        [HttpGet("by-station-id/{stationId}")]
        [Authorize]
        public async Task<IActionResult> GetServicesByStationId(int stationId, [FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _service.GetAllServiceByStationIdAsync(stationId, paginationParameter);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No service"
                    });
                }
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllServices([FromQuery] PaginationParameter paginationParameter, ServiceFilter filter)
        {
            try
            {
                var result = await _service.GetAllAsync(paginationParameter, filter);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No services found"
                    });
                }

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

        [HttpGet("{serviceId}")]
        [Authorize]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            try
            {
                var result = await _service.GetServiceByIdAsync(serviceId);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Service not found"
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
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddService(CreateServiceModel serviceModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                bool isAdded = await _service.AddServiceAsync(serviceModel);

                if (isAdded)
                {
                    // Return a success response
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Thêm dịch vụ thành công."
                    });
                }
                else
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không thể thêm dịch vụ này."
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
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateService(int id, UpdateServiceModel serviceModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool isUpdated = await _service.UpdateServiceAsync(id, serviceModel);

                if (isUpdated)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Cập nhật dịch vụ thành công."
                    });
                }
                else
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không thể cập nhật dịch vụ này."
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
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                bool isDeleted = await _service.DeleteServiceAsync(id);

                if (isDeleted)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Xóa dịch vụ thành công!"
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Xảy ra lỗi khi xóa dịch vụ!"
                    });
                }
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ResponseModel
                {
                    HttpCode = StatusCodes.Status404NotFound,
                    Message = $"Không tìm thấy dịch vụ với id: {id}"
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
