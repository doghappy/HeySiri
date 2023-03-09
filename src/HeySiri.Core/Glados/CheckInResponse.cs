using HeySiri.Core.Reporters;

namespace HeySiri.Core.Glados;

public class CheckInResponse : IReportable
{
    public string Message { get; set; }

    public string GetReport()
    {
        return $"{GladosHandler.Glados} Check In: {Message}";
    }
}