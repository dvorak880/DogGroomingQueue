namespace DogGroomingQueue.Api.DTOs;

public class AuthResponse
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}