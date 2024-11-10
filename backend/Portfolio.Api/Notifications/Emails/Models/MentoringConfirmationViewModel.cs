namespace Portfolio.Api.Notifications.Emails.Models;

public record MentoringConfirmation
{
    public required string Name { get; init; }
    public required string BookMeLink { get; set; }
}