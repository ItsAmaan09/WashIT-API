using Microsoft.AspNetCore.Mvc;
using washit.services;
using washit.dtos;
using Microsoft.AspNetCore.Authorization;

namespace washit.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReservationsController(IReservationService service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve([FromBody] MakeReservationDto dto)
        {
            try
            {
                // var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);

                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
                int? userId = null;
                if (int.TryParse(userIdClaim, out var parsedUserId))
                {
                    userId = parsedUserId;
                }

                var result = await _service.ReserveMachineAsync(userId, dto.WashTypeId, dto.MachineId);
                if (result == null) return BadRequest(result);

                return Ok(result);

            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelReservationDto dto)
        {
            try
            {
                // var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
                int? userId = null;
                if (int.TryParse(userIdClaim, out var parsedUserId))
                {
                    userId = parsedUserId;
                }
                bool success = await _service.CancelReservationAsync(dto.ReservationId, userId);
                if (!success) return BadRequest(success);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("waitlist")]
        public async Task<IActionResult> Waitlist([FromBody] JoinWaitingListDto dto)
        {
            try
            {
                // var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
                int? userId = null;
                if (int.TryParse(userIdClaim, out var parsedUserId))
                {
                    userId = parsedUserId;
                }
                int id = await _service.JoinWaitingListAsync(userId, dto.WashTypeId);
                return Ok(new { WaitingListId = id });

            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("machines")]
        public async Task<IActionResult> GetMachines()
        {
            return Ok(await _service.GetMachines());
        }

        [Authorize]
        [HttpGet("GetMachinesWithStatus")]
        public async Task<IActionResult> GetMachinesWithStatus()
        {
            try
            {
                var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);

                var machines = await _service.GetMachinesWithStatusAsync(userId);
                return Ok(machines);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
