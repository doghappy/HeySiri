using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HeySiri.Core.Reporters;

namespace HeySiri.Core.Hifini;

public class HifiniHandler
{
    public HifiniHandler(HttpClient httpClient, IReporter reporter)
    {
        _httpClient = httpClient;
        _reporter = reporter;
    }

    public const string Hifini = "HiFiNi";

    private readonly HttpClient _httpClient;
    private readonly IReporter _reporter;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private async Task<CheckInResponse> CheckInAsync(string cookie)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.hifini.com/sg_sign.htm")
        {
            Content = new StringContent("000", Encoding.UTF8),
        };
        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.58");
        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
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