using HeySiri.Core.Reporters;

namespace HeySiri.Core.Hifini;

public class CheckInResponse : IReportable
{
    public string Message { get; set; }

    public string GetReport()
    {
        return $"{HifiniHandler.Hifini} Check In: {Message}";
    }
}