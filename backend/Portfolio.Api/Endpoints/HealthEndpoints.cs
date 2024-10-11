namespace Portfolio.Api.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this WebApplication app)
    {
        app.MapGet("healthz", () => Results.Ok("healthy"));
    }
}