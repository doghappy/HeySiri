using HeySiri.Core.Hifini;

namespace HeySiri.Core.Tests.Hifini;

[TestClass]
public class CheckInResponseTests
{
    [TestMethod]
    [DynamicData(nameof(CheckInCases))]
    public void Should_get_report(CheckInResponse data, string expected)
    {
        data.GetReport().Should().Be(expected);
    }
    
    private static IEnumerable<object[]> CheckInCases =>
        CheckInTupleCases.Select(x => new object[] { x.data, x.expected });

    private static IEnumerable<(CheckInResponse data, string expected)> CheckInTupleCases
    {
        get
        {
            return new (CheckInResponse data, string expected)[]
            {
                (new CheckInResponse(), "HiFiNi Check In: "),
                (new CheckInResponse{ Message = "Hello 🌍🌏🌎"}, "HiFiNi Check In: Hello 🌍🌏🌎"),
            };
        }
    }
}