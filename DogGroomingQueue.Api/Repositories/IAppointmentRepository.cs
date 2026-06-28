using DogGroomingQueue.Api.DTOs;
using DogGroomingQueue.Api.Models;

namespace DogGroomingQueue.Api.Repositories;

public interface IAppointmentRepository
{
    Task<int> CreateAppointmentAsync(int userId, CreateAppointmentRequest request);

    Task<List<AppointmentResponse>> GetAppointmentsAsync(
        string? customerName,
        DateTime? fromDate,
        DateTime? toDate);

    Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);

    Task<int> GetPastAppointmentsCountAsync(int userId);

    Task UpdateAppointmentAsync(Appointment appointment);

    Task DeleteAppointmentAsync(Appointment appointment);
   
    Task<AppointmentResponse?> GetAppointmentDetailsByIdAsync(int appointmentId);
}