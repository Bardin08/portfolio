using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using Portfolio.Api.Configuration;
using Portfolio.Api.Workspaces.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Portfolio.Api.Extensions.DependencyInjection;

public static class GoogleServicesExtensions
{
    public static void AddGoogleServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGoogleDriveService, GoogleDriveService>();

        services.Configure<GoogleServicesOptions>(configuration.GetSection(GoogleServicesOptions.SectionName));

        services.AddSingleton<DriveService>(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<GoogleServicesOptions>>().Value;
            var creds = GetGoogleApisCreds(provider);

            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = creds,
                ApplicationName = settings.ApplicationName
            });
        });

        services.AddSingleton<DocsService>(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<GoogleServicesOptions>>().Value;
            var credential = GetGoogleApisCreds(provider);

            return new DocsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = settings.ApplicationName
            });
        });
    }

    private static GoogleCredential GetGoogleApisCreds(IServiceProvider provider)
    {
        var settings = provider.GetRequiredService<IOptions<GoogleServicesOptions>>().Value;

        var credsJson = JsonSerializer.Serialize(settings.Credentials,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

        return GoogleCredential.FromJson(credsJson).CreateScoped(settings.Scopes);
    }
}