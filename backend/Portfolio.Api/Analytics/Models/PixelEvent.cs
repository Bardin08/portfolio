using Portfolio.Api.Tracking.Models;
using UAParser;

namespace Portfolio.Api.Analytics.Models;

public record PixelEvent
{
    public required string EventType { get; init; }
    public required string ElementId { get; init; }
    public DateTimeOffset Timestamp { get; init; }

    public required string UserId { get; init; }
    public required string SessionId { get; init; }
    public required string PageUrl { get; init; }

    public AdditionalData? AdditionalData { get; init; }
}

public record AdditionalData
{
    public int ScreenWidth { get; init; }
    public int ScreenHeight { get; init; }
    public int ViewportWidth { get; init; }
    public int ViewportHeight { get; init; }

    public string? UserAgent { get; init; }
    public string? Referrer { get; init; }
    public string? Campaign { get; init; }
    public string? UtmSource { get; init; }
    public string? UtmMedium { get; init; }
    public string? UtmCampaign { get; init; }

    public required InteractionDetails InteractionDetails { get; init; }
    public IpAddressInfoViewModel? GeoLocation { get; set; }
    public ClientDeviceInfo? ClientDeviceInfo { get; init; }
}

public record ClientDeviceInfo
{
    public string? Browser { get; init; }
    public string? BrowserVersion { get; init; }
    public string? OperatingSystem { get; init; }
    public string? OsVersion { get; init; }
    public string? DeviceType { get; init; }
    public string? DeviceBrandAndModel { get; init; }

    public static ClientDeviceInfo? FromUserAgent(string userAgent)
    {
        try
        {
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);

            if (clientInfo == null)
                return null;

            return new ClientDeviceInfo
            {
                Browser = clientInfo.UA.Family,
                BrowserVersion = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}.{clientInfo.UA.Patch}",
                DeviceType = clientInfo.Device.IsSpider ? "Spider" : clientInfo.Device.Family,
                DeviceBrandAndModel = $"{clientInfo.Device.Brand} (${clientInfo.Device.Model})",
                OperatingSystem = clientInfo.OS.Family,
                OsVersion = $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}.{clientInfo.OS.Patch}",
            };
        }
        catch (Exception _)
        {
           return null;
        }
    }
}

public record InteractionDetails
{
    public int ScrollDepth { get; init; }
    public int TimeOnPage { get; init; }
    public bool IsRepeatVisitor { get; init; }
    public bool IsFirstInteraction { get; init; }
    public string? PreviousElementId { get; init; }
}
