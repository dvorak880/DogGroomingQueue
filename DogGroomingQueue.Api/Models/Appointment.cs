namespace DogGroomingQueue.Api.Models;

public class Appointment
{
    public int AppointmentId_Int { get; set; }
    public int UserId_Int { get; set; }
    public int GroomingTypeId_Int { get; set; }
    public DateTime AppointmentDateTime_Dat { get; set; }
    public DateTime CreatedAt_Dat { get; set; }
    public bool IsDeleted_Bit { get; set; }
}