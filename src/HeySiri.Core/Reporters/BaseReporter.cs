namespace HeySiri.Core.Reporters;

public abstract class BaseReporter : IReporter
{
    protected BaseReporter(string tz, IDateTimeProvider dateTimeProvider)
    {
        _tz = tz;
        _dateTimeProvider = dateTimeProvider;
    }

    private readonly string _tz;
    private readonly IDateTimeProvider _dateTimeProvider;

    private string GetReport(IReportable data)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_tz);
        var now = TimeZoneInfo.ConvertTimeFromUtc(_dateTimeProvider.UtcNow, timeZone);
        return $@"{data.GetReport()}

{now}({_tz})";
    }
    
    public async Task ReportAsync(IReportable data)
    {
        var report = GetReport(data);
        await ReportCoreAsync(report);
    }

    protected abstract Task ReportCoreAsync(string report);
}