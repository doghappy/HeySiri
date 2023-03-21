using HeySiri.Core.Glados;
using HeySiri.Core.Reporters;
using Moq;

namespace HeySiri.Core.Tests;

[TestClass]
public class TelegramReporterTests
{
    [TestMethod]
    [DynamicData(nameof(LocalTimeCases))]
    public async Task Should_report_with_local_date_time(string tz, IReportable data, string expectedMessage)
    {
        var telegramBotClient = new Mock<ITelegramBotClient>();
        var configuration = new TelegramConfiguration
        {
            ChatId = "test",
        };

        var repoter = new TelegramReporter(tz, new FakeDateTimeProvider(), telegramBotClient.Object, configuration);
        await repoter.ReportAsync(data);

        telegramBotClient.Verify(t => t.SendTextMessageAsync(
            It.Is<string>(x => x == "test"),
            It.Is<string>(x => x.EqualsIgnoreCarriageReturn(expectedMessage))
        ), Times.Once);
    }

    private static IEnumerable<object[]> LocalTimeCases =>
        LocalTimeTupleCases.Select(x => new object[] { x.tz, x.data, x.expectedMessage });

    private static IEnumerable<(string tz, IReportable data, string expectedMessage)> LocalTimeTupleCases
    {
        get
        {
            return new (string tz, IReportable data, string expectedMessage)[]
            {
                (
                    "China Standard Time",
                    new CheckInResponse { Message = "Hello 🌎🌏🌍" },
                    $@"Glados Check In: Hello 🌎🌏🌍

3/9/2023 7:10:45 AM(China Standard Time)"
                ),
            };
        }
    }
}