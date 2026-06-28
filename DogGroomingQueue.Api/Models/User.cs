namespace DogGroomingQueue.Api.Models;

public class User
{
    public int UserId_Int { get; set; }
    public string Username_Vch { get; set; } = string.Empty;
    public string PasswordHash_Vch { get; set; } = string.Empty;
    public string FirstName_Vch { get; set; } = string.Empty;
    public DateTime CreatedAt_Dat { get; set; }
}