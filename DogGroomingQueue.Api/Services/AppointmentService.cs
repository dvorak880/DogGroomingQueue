using DogGroomingQueue.Api.DTOs;
using DogGroomingQueue.Api.Repositories;

namespace DogGroomingQueue.Api.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentService(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> CreateAppointmentAsync(int userId, CreateAppointmentRequest request)
    {
        return await _repository.CreateAppointmentAsync(userId, request);
    }

    public async Task<List<AppointmentResponse>> GetAppointmentsAsync(
        string? customerName,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var appointments = await _repository.GetAppointmentsAsync(
            customerName,
            fromDate,
            toDate);

        foreach (var appointment in appointments)
        {
            var pastCount = await _repository.GetPastAppointmentsCountAsync(appointment.UserId);

            if (pastCount > 3 && appointment.AppointmentDateTime > DateTime.Now)
            {
                appointment.HasDiscount = true;
                appointment.Price = appointment.Price * 0.9m;
            }
        }

        return appointments;
    }

    public async Task UpdateAppointmentAsync(
        int userId,
        int appointmentId,
        UpdateAppointmentRequest request)
    {
        var appointment = await _repository.GetAppointmentByIdAsync(appointmentId);

        if (appointment == null)
        {
            throw new Exception("התור לא נמצא");
        }

        if (appointment.UserId_Int != userId)
        {
            throw new UnauthorizedAccessException("לא ניתן לערוך תור שלא שייך לך");
        }

        appointment.GroomingTypeId_Int = request.GroomingTypeId;
        appointment.AppointmentDateTime_Dat = request.AppointmentDateTime;

        await _repository.UpdateAppointmentAsync(appointment);
    }

    public async Task DeleteAppointmentAsync(int userId, int appointmentId)
    {
        var appointment = await _repository.GetAppointmentByIdAsync(appointmentId);

        if (appointment == null)
        {
            throw new Exception("התור לא נמצא");
        }

        if (appointment.UserId_Int != userId)
        {
            throw new UnauthorizedAccessException("לא ניתן למחוק תור שלא שייך לך");
        }

        if (appointment.AppointmentDateTime_Dat.Date == DateTime.Today)
        {
            throw new InvalidOperationException("לא ניתן למחוק תור של היום");
        }

        await _repository.DeleteAppointmentAsync(appointment);
    }

    public async Task<AppointmentResponse> GetAppointmentDetailsByIdAsync(int appointmentId)
    {
        var appointment = await _repository.GetAppointmentDetailsByIdAsync(appointmentId);

        if (appointment == null)
        {
            throw new Exception("התור לא נמצא");
        }

        var pastCount = await _repository.GetPastAppointmentsCountAsync(appointment.UserId);

        if (pastCount > 3 && appointment.AppointmentDateTime > DateTime.Now)
        {
            appointment.HasDiscount = true;
            appointment.Price = appointment.Price * 0.9m;
        }

        return appointment;
    }
}