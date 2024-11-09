namespace Portfolio.Api.Configuration;

public record ContactInfoOptions
{
    public const string SectionName = "ContactInfo";
    public required string AdminEmail { get; set; }
}