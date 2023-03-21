using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using HeySiri.Core.Reporters;

namespace HeySiri.Core.Bandwagon;

public class BandwagonHandler
{
    public BandwagonHandler(HttpClient httpClient, IReporter reporter)
    {
        _httpClient = httpClient;
        _reporter = reporter;
    }

    public const string Bandwagon = "Bandwagon";

    private readonly HttpClient _httpClient;
    private readonly IReporter _reporter;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
        }
    };

    private async Task<LiveServiceInfo> GetLiveServiceInfoAsync(string veid, string apiKey)
    {
        return await _httpClient.GetFromJsonAsync<LiveServiceInfo>(
            $"https://api.64clouds.com/v1/getLiveServiceInfo?veid={veid}&api_key={apiKey}",
            JsonSerializerOptions);
    }

    public async Task ReportLiveServiceInfoAsync(string veid, string apiKey)
    {
        var data = await GetLiveServiceInfoAsync(veid, apiKey);
        await _reporter.ReportAsync(data);
    }
}