namespace HeySiri.Core.Reporters;

public class TelegramReporter : BaseReporter
{
    public TelegramReporter(
        string tz,
        IDateTimeProvider dateTimeProvider,
        ITelegramBotClient telegramBotClient,
        TelegramConfiguration configuration) : base(tz, dateTimeProvider)
    {
        _telegramBotClient = telegramBotClient;
        _configuration = configuration;
    }

    private readonly ITelegramBotClient _telegramBotClient;
    private readonly TelegramConfiguration _configuration;

    protected override async Task ReportCoreAsync(string report)
    {
        await _telegramBotClient.SendTextMessageAsync(
            chatId: _configuration.ChatId,
            text: report);
    }
}