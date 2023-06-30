using HeySiri.Core.Hifini;
using HeySiri.Core.Reporters;
using Moq;
using RichardSzalay.MockHttp;

namespace HeySiri.Core.Tests.Hifini;

[TestClass]
public class HifiniHandlerTests
{
    [TestMethod]
    [DynamicData(nameof(CheckInCases))]
    public async Task Should_report_check_in(string json, string message)
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When("https://www.hifini.com/sg_sign.htm")
            .WithHeaders("User-Agent","Mozilla/5.0")
            .WithHeaders("User-Agent","(Windows NT 10.0; Win64; x64)")
            .WithHeaders("User-Agent","AppleWebKit/537.36")
            .WithHeaders("User-Agent","(KHTML, like Gecko)")
            .WithHeaders("User-Agent","Chrome/114.0.0.0")
            .WithHeaders("User-Agent","Safari/537.36")
            .WithHeaders("User-Agent","Edg/114.0.1823.58")
            .WithHeaders("X-Requested-With","XMLHttpRequest")
            .WithHeaders("cookie","bbs_token=test")
            .Respond("text/html", await File.ReadAllTextAsync(json));
        var httpClient = mockHttp.ToHttpClient();
        var reporter = new Mock<IReporter>();
        var handler = new HifiniHandler(httpClient, reporter.Object);
        await handler.ReportCheckInAsync("bbs_token=test");

        reporter.Verify(t => t.ReportAsync(It.Is<CheckInResponse>(p => p.Message == message)), Times.Once());
    }

    private static IEnumerable<object[]> CheckInCases =>
        CheckInTupleCases.Select(x => new object[] { x.json, x.message });

    private static IEnumerable<(string json, string message)> CheckInTupleCases
    {
        get
        {
            return new (string json, string message)[]
            {
                ("Hifini/checkin-0.txt", "今天已经签过啦！"),
            };
        }
    }
}