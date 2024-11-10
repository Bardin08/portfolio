namespace Portfolio.Api.Tracking.Models;

public record IpAddressInfoViewModel(
    int IpVersion,
    string IpAddress,
    double Latitude,
    double Longitude,
    string CountryName,
    string CountryCode,
    string TimeZone,
    string ZipCode,
    string CityName,
    string RegionName,
    string Continent,
    string ContinentCode,
    bool IsProxy,
    Currency Currency,
    string Language,
    IReadOnlyList<string> TimeZones,
    IReadOnlyList<string> Tlds
);

public record Currency(
    string Code,
    string Name
);