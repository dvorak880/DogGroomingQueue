namespace DogGroomingQueue.Api.DTOs;

public class UpdateAppointmentRequest
{
    public int GroomingTypeId { get; set; }

    public DateTime AppointmentDateTime { get; set; }
}