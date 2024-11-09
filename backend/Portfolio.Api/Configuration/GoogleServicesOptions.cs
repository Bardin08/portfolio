namespace Portfolio.Api.Configuration;

public record GoogleServicesOptions
{
    public required string CredentialsPath { get; init; }
    public required string[] Scopes { get; init; }
    public required string ApplicationName { get; init; }
}