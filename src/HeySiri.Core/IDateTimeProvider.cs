namespace HeySiri.Core;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}