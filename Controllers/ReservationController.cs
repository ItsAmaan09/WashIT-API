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

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve([FromBody] MakeReservationDto dto)
        {

            var result = await _service.ReserveMachineAsync(dto.UserName, dto.WashTypeId);
            if (result == null) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelReservationDto dto)
        {
            bool success = await _service.CancelReservationAsync(dto.ReservationId);
            if (!success) return NotFound();

            return Ok();
        }

        [HttpPost("waitlist")]
        public async Task<IActionResult> Waitlist([FromBody] JoinWaitingListDto dto)
        {
            int id = await _service.JoinWaitingListAsync(dto.UserName, dto.WashTypeId);
            return Ok(new { WaitingListId = id });
        }

        [HttpGet("machines")]
        public async Task<IActionResult> GetMachines()
        {
            return Ok(await _service.GetMachines());
        }

        [HttpGet("checkMachineAvailability")]
        public async Task<IActionResult> CheckMachineAvailability(int id, string userName)
        {
            var status = await _service.CheckMachineAvailability(id, userName);
            return Ok(new { status });
        }

        [HttpGet("by-machine")]
        public async Task<IActionResult> GetReservationByMachineId(int machineId, string userName)
        {
            var reservation = await _service.GetReservationByMachineIdAsync(machineId, userName);
            if (reservation == null)
                return NotFound("No active reservation found for this machine.");

            return Ok(reservation);
        }


    }
}
