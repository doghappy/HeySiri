namespace HeySiri.Core.Reporters;

public interface IReporter
{
    Task ReportAsync(IReportable data);
}