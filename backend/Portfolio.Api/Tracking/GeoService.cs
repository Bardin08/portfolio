using Portfolio.Api.Tracking.Models;

namespace Portfolio.Api.Tracking;

public class GeoService
{
    public async Task<IpAddressInfoViewModel?> GetGeoInfoByIp(string ipAddress)
    {
        var url = $"https://freeipapi.com/api/json/{ipAddress}";

        using var client = new HttpClient();
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<IpAddressInfoViewModel>();

        return null;
    }
}

