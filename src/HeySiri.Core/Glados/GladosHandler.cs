using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HeySiri.Core.Reporters;

namespace HeySiri.Core.Glados;

public class GladosHandler
{
    public GladosHandler(HttpClient httpClient, IReporter reporter)
    {
        _httpClient = httpClient;
        _reporter = reporter;
    }

    public const string Glados = "Glados";

    private readonly HttpClient _httpClient;
    private readonly IReporter _reporter;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private async Task<CheckInResponse> CheckInAsync(string cookie)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/user/checkin")
        {
            Content = new StringContent("{\"token\":\"glados.network\"}", Encoding.UTF8, new MediaTypeHeaderValue("application/json")),
        };
        request.Headers.Add("accept", "application/json");
        request.Headers.Add("authority", "glados.one");
        request.Headers.Add("cookie", cookie);
        var resMsg = await _httpClient.SendAsync(request);
        if (resMsg.IsSuccessStatusCode)
        {
            return await resMsg.Content.ReadFromJsonAsync<CheckInResponse>(JsonSerializerOptions);
        }

        return new CheckInResponse
        {
            Message = resMsg.StatusCode.ToString()
        };
    }

    public async Task ReportCheckInAsync(string cookie)
    {
        var data = await CheckInAsync(cookie);
        await _reporter.ReportAsync(data);
    }
}