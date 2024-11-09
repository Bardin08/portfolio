using System.Text.Json.Serialization;

namespace Portfolio.Api.Configuration;

public record GoogleServicesOptions
{
    public const string SectionName = "GoogleApi";

    public required GoogleApiCredentialsOptions Credentials { get; set; }

    public required string[] Scopes { get; init; }
    public required string ApplicationName { get; init; }
}

public record GoogleApiCredentialsOptions
{
    public required string Type { get; init; }
    public required string ProjectId { get; init; }
    public required string PrivateKeyId { get; init; }
    public required string PrivateKey { get; init; }
    public required string ClientEmail { get; init; }
    public required string ClientId { get; init; }
    public required string AuthUri { get; init; }
    public required string TokenUri { get; init; }
    public required string UniverseDomain { get; init; }

    [JsonPropertyName("auth_provider_x509_cert_url")]
    public required string AuthProviderX509CertUrl { get; init; }

    [JsonPropertyName("client_x509_cert_url")]
    public required string ClientX509CertUrl { get; init; }
}