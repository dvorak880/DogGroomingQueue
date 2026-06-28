using DogGroomingQueue.Api.DTOs;

namespace DogGroomingQueue.Api.Services;

public interface IAppointmentService
{
    Task<int> CreateAppointmentAsync(int userId, CreateAppointmentRequest request);

    Task<List<AppointmentResponse>> GetAppointmentsAsync(
        string? customerName,
        DateTime? fromDate,
        DateTime? toDate);

    Task UpdateAppointmentAsync(int userId, int appointmentId, UpdateAppointmentRequest request);

    Task DeleteAppointmentAsync(int userId, int appointmentId);

    Task<AppointmentResponse> GetAppointmentDetailsByIdAsync(int appointmentId);
}