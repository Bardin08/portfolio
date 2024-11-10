using System.Net;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Analytics.Models;
using Portfolio.Api.MongoDb.Repositories;
using Portfolio.Api.Tracking;

namespace Portfolio.Api.Endpoints;

public static class TrackingEndpoints
{
    public static void MapTrackingEndpoints(this WebApplication app)
    {
        app.MapPost("/tracking", async (
            HttpContext req,
            [FromBody] PixelEvent pe,
            [FromServices] IPixelEventsRepository eventsRepository,
            [FromServices] GeoService gs) =>
        {
            if (!string.IsNullOrEmpty(pe.AdditionalData?.UserAgent))
            {
                var clientInfo = ClientDeviceInfo.FromUserAgent(pe.AdditionalData.UserAgent);
                pe = pe with
                {
                    AdditionalData = pe.AdditionalData with
                    {
                        ClientDeviceInfo = clientInfo
                    }
                };
            }

            if (pe is { EventType: "view", ElementId: "landingPage", AdditionalData: not null })
            {
                var ipAddress = req.GetClientIpAddress();
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    var geoInfo = await gs.GetGeoInfoByIp(ipAddress);
                    if (geoInfo != null)
                    {
                        pe = pe with
                        {
                            AdditionalData = pe.AdditionalData with
                            {
                                GeoLocation = geoInfo
                            }
                        };
                    }
                }
            }

            await eventsRepository.Add(pe);
        });
    }
}

public static class IpAddressHelper
{
    public static string? GetClientIpAddress(this HttpContext context)
    {
        var headersToCheck = new[] { "X-Forwarded-For" };

        foreach (var header in headersToCheck)
        {
            if (!context.Request.Headers.TryGetValue(header, out var values))
                continue;

            var forwardedIp = values.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(forwardedIp))
                continue;

            var ip = forwardedIp.Split(',').First().Trim();
            if (IPAddress.TryParse(ip, out _))
                return ip;
        }

        // Fallback to the RemoteIpAddress if headers are not available or invalid
        return context.Connection.RemoteIpAddress?.ToString();
    }
}