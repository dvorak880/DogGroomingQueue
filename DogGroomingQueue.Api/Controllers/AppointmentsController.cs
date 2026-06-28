using DogGroomingQueue.Api.DTOs;
using DogGroomingQueue.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogGroomingQueue.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments(
        [FromQuery] string? customerName,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var appointments = await _appointmentService.GetAppointmentsAsync(
            customerName,
            fromDate,
            toDate);

        return Ok(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(CreateAppointmentRequest request)
    {
        var userId = GetCurrentUserId();

        var newAppointmentId = await _appointmentService.CreateAppointmentAsync(
            userId,
            request);

        return Ok(new
        {
            AppointmentId = newAppointmentId,
            Message = "התור נוסף בהצלחה"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(
        int id,
        UpdateAppointmentRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();

            await _appointmentService.UpdateAppointmentAsync(
                userId,
                id,
                request);

            return Ok("התור עודכן בהצלחה");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        try
        {
            var userId = GetCurrentUserId();

            await _appointmentService.DeleteAppointmentAsync(userId, id);

            return Ok("התור נמחק בהצלחה");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentById(int id)
    {
        try
        {
            var appointment = await _appointmentService.GetAppointmentDetailsByIdAsync(id);

            return Ok(appointment);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}