namespace HeySiri.Core.Tests;

public class FakeDateTimeProvider : IDateTimeProvider
{
    public FakeDateTimeProvider()
    {
        UtcNow = new DateTime(2023, 3, 8, 23, 10, 45);
    }

    public DateTime UtcNow { get; }
}