namespace DogGroomingQueue.Api.Models;

public class GroomingType
{
    public int GroomingTypeId_Int { get; set; }
    public string TypeName_Vch { get; set; } = string.Empty;
    public int DurationMinutes_Int { get; set; }
    public decimal Price_Dec { get; set; }
}