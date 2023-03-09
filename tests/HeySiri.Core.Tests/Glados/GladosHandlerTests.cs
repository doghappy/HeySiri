using HeySiri.Core.Glados;
using HeySiri.Core.Reporters;
using Moq;
using RichardSzalay.MockHttp;

namespace HeySiri.Core.Tests.Glados;

[TestClass]
public class GladosHandlerTests
{
    [TestMethod]
    [DynamicData(nameof(CheckInCases))]
    public async Task Should_report_check_in(string json, string message)
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When("https://glados.one/api/user/checkin")
            .WithHeaders("authority","glados.one")
            .WithHeaders("content-type","application/json")
            .WithHeaders("cookie","cookie")
            .WithContent("{\"token\":\"glados.network\"}")
            .Respond("application/json", await File.ReadAllTextAsync(json));
        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://glados.one");
        var reporter = new Mock<IReporter>();
        var handler = new GladosHandler(httpClient, reporter.Object);
        await handler.ReportCheckInAsync("cookie");

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
                ("TestFiles/checkin-0.json", "Checkin! Get 1 Day"),
                ("TestFiles/checkin-1.json", "Please Try Tomorrow"),
            };
        }
    }
}