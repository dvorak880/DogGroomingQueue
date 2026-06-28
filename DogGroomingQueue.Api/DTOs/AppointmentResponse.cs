namespace DogGroomingQueue.Api.DTOs;

public class AppointmentResponse
{
    public int AppointmentId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string DogTypeName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int DurationMinutes { get; set; }

    public DateTime AppointmentDateTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool HasDiscount { get; set; }
}