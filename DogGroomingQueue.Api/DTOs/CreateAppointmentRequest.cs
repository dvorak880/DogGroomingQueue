namespace DogGroomingQueue.Api.DTOs;

public class CreateAppointmentRequest
{
    public int GroomingTypeId { get; set; }

    public DateTime AppointmentDateTime { get; set; }
}