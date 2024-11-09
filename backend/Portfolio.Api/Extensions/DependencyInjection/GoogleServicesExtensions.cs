using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using Portfolio.Api.Configuration;
using Portfolio.Api.Workspaces.Services;

namespace Portfolio.Api.Extensions.DependencyInjection;

public static class GoogleServicesExtensions
{
    public static void AddGoogleServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGoogleDriveService, GoogleDriveService>();

        services.Configure<GoogleServicesOptions>(configuration.GetSection("Google"));

        services.AddSingleton<DriveService>(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<GoogleServicesOptions>>().Value;
            var credential = GoogleCredential.FromFile(settings.CredentialsPath)
                .CreateScoped(settings.Scopes);
            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = settings.ApplicationName
            });
        });

        services.AddSingleton<DocsService>(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<GoogleServicesOptions>>().Value;
            var credential = GoogleCredential.FromFile(settings.CredentialsPath).CreateScoped(settings.Scopes);
            return new DocsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = settings.ApplicationName
            });
        });
    }
}