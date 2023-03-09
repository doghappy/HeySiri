using HeySiri.Core.Glados;

namespace HeySiri.Core.Tests.Glados;

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
                (new CheckInResponse(), "Glados Check In: "),
                (new CheckInResponse{ Message = "Hello 🌍🌏🌎"}, "Glados Check In: Hello 🌍🌏🌎"),
            };
        }
    }
}