using HeySiri.Core.Bandwagon;
using HeySiri.Core.Reporters;
using Moq;
using RichardSzalay.MockHttp;

namespace HeySiri.Core.Tests.Bandwagon;

[TestClass]
public class BandwagonHandlerTests
{
    [TestMethod]
    [DynamicData(nameof(LiveServiceInfoCases))]
    public async Task Should_report_live_service_info(string json, string tz, TelegramConfiguration configuration, string message)
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When("https://api.64clouds.com/v1/getLiveServiceInfo")
            .WithExactQueryString(new Dictionary<string, string>
            {
                { "veid", "123" },
                { "api_key", "apiKey" },
            })
            .Respond("text/plain", await File.ReadAllTextAsync(json));
        var httpClient = mockHttp.ToHttpClient();
        var telegramBotClient = new Mock<ITelegramBotClient>();
        var reporter = new TelegramReporter(tz, new FakeDateTimeProvider(), telegramBotClient.Object, configuration);
        var handler = new BandwagonHandler(httpClient, reporter);
        await handler.ReportLiveServiceInfoAsync("123", "apiKey");

        telegramBotClient.Verify(t => t.SendTextMessageAsync(
            configuration.ChatId,
            It.Is<string>(x => x.EqualsIgnoreCarriageReturn(message))
        ), Times.Once());
    }

    private static IEnumerable<object[]> LiveServiceInfoCases =>
        LiveServiceInfoTupleCases.Select(x => new object[] { x.json, x.tz, x.configuration, x.message });

    private static IEnumerable<(string json, string tz, TelegramConfiguration configuration, string message)> LiveServiceInfoTupleCases
    {
        get
        {
            return new (string json, string tz, TelegramConfiguration configuration, string message)[]
            {
                (
                    "Bandwagon/liveServiceInfo.json",
                    "China Standard Time",
                    new TelegramConfiguration
                    {
                        ChatId = "123456",
                    },
                    @"Plan: kvmv5-the-plan-v1
Host Name: awesome-kitten-3.localdomain
Data Center: Japan: Osaka (Softbank)
VPS ID: 1764158
IP Address: 23.106.141.202
SSH Port: 26122
Status: Running
RAM Usage(MB): 703.40/2,048.00 34.35%
SWAP Usage(MB): 0.00/1,024.00 0.00%
Disk Usage(GB): 4.53/40.00 11.33%
Bandwidth Usage(GB): 21.70/1,000.00 2.17%
Bandwidth Resets: 3/14/2023 2:23:25 PM +00:00
OS: centos-7-x86_64-bbr

3/9/2023 7:10:45 AM(China Standard Time)"),
            };
        }
    }
}