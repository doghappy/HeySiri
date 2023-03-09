using Microsoft.Extensions.Logging;

namespace HeySiri.Core.Reporters;

public class LogInformationReporter : BaseReporter
{
    public LogInformationReporter(
        string tz, 
        IDateTimeProvider dateTimeProvider, 
        ILogger<LogInformationReporter> logger) : base(tz, dateTimeProvider)
    {
        _logger = logger;
    }

    private readonly ILogger<LogInformationReporter> _logger;

    protected override Task ReportCoreAsync(string report)
    {
        _logger.LogInformation(report);
        return Task.CompletedTask;
    }
}