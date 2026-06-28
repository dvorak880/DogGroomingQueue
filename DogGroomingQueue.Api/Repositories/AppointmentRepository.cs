using Dapper;
using DogGroomingQueue.Api.Data;
using DogGroomingQueue.Api.DTOs;
using DogGroomingQueue.Api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DogGroomingQueue.Api.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AppointmentRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<int> CreateAppointmentAsync(int userId, CreateAppointmentRequest request)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        var newAppointmentId = await connection.ExecuteScalarAsync<int>(
            "sp_CreateAppointment",
            new
            {
                UserId = userId,
                GroomingTypeId = request.GroomingTypeId,
                AppointmentDateTime = request.AppointmentDateTime
            },
            commandType: System.Data.CommandType.StoredProcedure
        );

        return newAppointmentId;
    }

    public async Task<List<AppointmentResponse>> GetAppointmentsAsync(
        string? customerName,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var query = _context.Set<AppointmentResponse>()
            .FromSqlRaw(@"
                SELECT
                    AppointmentId_Int AS AppointmentId,
                    UserId_Int AS UserId,
                    CustomerName,
                    DogTypeName,
                    Price_Dec AS Price,
                    DurationMinutes_Int AS DurationMinutes,
                    AppointmentDateTime_Dat AS AppointmentDateTime,
                    CreatedAt_Dat AS CreatedAt,
                    CAST(0 AS bit) AS HasDiscount
                FROM vw_AppointmentsDetailsSwish
                WHERE IsDeleted_Bit = 0
            ");

        var appointments = await query.ToListAsync();

        if (!string.IsNullOrWhiteSpace(customerName))
        {
            appointments = appointments
                .Where(x => x.CustomerName.Contains(customerName))
                .ToList();
        }

        if (fromDate.HasValue)
        {
            appointments = appointments
                .Where(x => x.AppointmentDateTime.Date >= fromDate.Value.Date)
                .ToList();
        }

        if (toDate.HasValue)
        {
            appointments = appointments
                .Where(x => x.AppointmentDateTime.Date <= toDate.Value.Date)
                .ToList();
        }

        return appointments;
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
    {
        return await _context.Appointments
            .FirstOrDefaultAsync(x =>
                x.AppointmentId_Int == appointmentId &&
                !x.IsDeleted_Bit);
    }

    public async Task<int> GetPastAppointmentsCountAsync(int userId)
    {
        return await _context.Appointments
            .CountAsync(x =>
                x.UserId_Int == userId &&
                x.AppointmentDateTime_Dat < DateTime.Now &&
                !x.IsDeleted_Bit);
    }

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAppointmentAsync(Appointment appointment)
    {
        appointment.IsDeleted_Bit = true;
        await _context.SaveChangesAsync();
    }

    public async Task<AppointmentResponse?> GetAppointmentDetailsByIdAsync(int appointmentId)
    {
        return await _context.AppointmentResponses
            .FromSqlRaw(@"
            SELECT
                AppointmentId_Int AS AppointmentId,
                UserId_Int AS UserId,
                CustomerName,
                DogTypeName,
                Price_Dec AS Price,
                DurationMinutes_Int AS DurationMinutes,
                AppointmentDateTime_Dat AS AppointmentDateTime,
                CreatedAt_Dat AS CreatedAt,
                CAST(0 AS bit) AS HasDiscount
            FROM vw_AppointmentsDetailsSwish
            WHERE AppointmentId_Int = {0}
              AND IsDeleted_Bit = 0
        ", appointmentId)
            .FirstOrDefaultAsync();
    }
}