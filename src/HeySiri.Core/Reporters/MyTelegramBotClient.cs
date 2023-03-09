using Telegram.Bot;

namespace HeySiri.Core.Reporters;

public class MyTelegramBotClient : ITelegramBotClient
{
    public MyTelegramBotClient(string token)
    {
        _telegramBotClient = new TelegramBotClient(token);
    }

    private readonly Telegram.Bot.ITelegramBotClient _telegramBotClient;

    public async Task SendTextMessageAsync(string chatId, string text)
    {
        await _telegramBotClient.SendTextMessageAsync(chatId, text);
    }
}