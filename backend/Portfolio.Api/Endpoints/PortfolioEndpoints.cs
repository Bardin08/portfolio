using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Requests;
using Portfolio.Api.Notifications.Emails.Services;

namespace Portfolio.Api.Endpoints;

public static class PortfolioEndpoints
{
    public static void MapPortfolioEndpoints(this WebApplication app)
    {
        app.MapPost("/services", async (
            [FromBody] ServicesRequest req, [FromServices] EmailService es) =>
        {
            await es.SendServiceRequestEmailsAsync(req);
        });
    }
}