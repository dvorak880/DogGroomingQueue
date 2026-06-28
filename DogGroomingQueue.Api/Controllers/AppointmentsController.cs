using DogGroomingQueue.Api.Data;
using DogGroomingQueue.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Dapper;

namespace DogGroomingQueue.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AppointmentsController(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(CreateAppointmentRequest request)
    {
        var userId = GetCurrentUserId();

        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        var parameters = new
        {
            UserId = userId,
            GroomingTypeId = request.GroomingTypeId,
            AppointmentDateTime = request.AppointmentDateTime
        };

        var newAppointmentId = await connection.ExecuteScalarAsync<int>(
            "sp_CreateAppointment",
            parameters,
            commandType: System.Data.CommandType.StoredProcedure
        );

        return Ok(new
        {
            AppointmentId = newAppointmentId,
            Message = "התור נוסף בהצלחה"
        });
    }
}