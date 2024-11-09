namespace Portfolio.Api.Configuration;

public record EmailAccountOptions
{
    public const string SectionName = "EmailAccount";
    public required string AdminEmail { get; init; }
    public required string AppPassword { get; init; }
    public required string SenderName { get; set; }
}