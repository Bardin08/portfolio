namespace Portfolio.Api.Models.Requests;

public record ServicesRequest(
    string Name,
    string Email,
    string Description,
    string Link
);

public class MentoringRequest
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MessengerType { get; set; } = string.Empty; // "telegram" or "whatsapp"

    public string MentoringDirection { get; set; } = string.Empty;

    public string GoalsAndExpectations { get; set; } = string.Empty;
}