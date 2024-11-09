namespace Portfolio.Api.Notifications.Emails.Models;

public record ServiceRequestNotificationModel
{
    public required string ClientName { get; init; }
    public required string ConsultantName { get; init; }
    public required string Location { get; init; }
    public required string GoogleDocsUrl { get; init; }
}