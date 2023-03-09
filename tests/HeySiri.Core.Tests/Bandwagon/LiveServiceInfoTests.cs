using HeySiri.Core.Bandwagon;

namespace HeySiri.Core.Tests.Bandwagon;

[TestClass]
public class LiveServiceInfoTests
{
    [TestMethod]
    [DynamicData(nameof(LiveServiceInfoCases))]
    public void Should_throw_an_exception(LiveServiceInfo info, Type innerException)
    {
        info.Invoking(d => d.GetReport())
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Could not generic report, please check the response.")
            .WithInnerException(innerException);
    }
    
    private static IEnumerable<object[]> LiveServiceInfoCases =>
        LiveServiceInfoCasesTupleCases.Select(x => new object[] { x.info, x.innerException });

    private static IEnumerable<(LiveServiceInfo info, Type innerException)> LiveServiceInfoCasesTupleCases
    {
        get
        {
            return new (LiveServiceInfo info, Type innerException)[]
            {
                (new LiveServiceInfo(), typeof(ArgumentException)),
            };
        }
    }
}