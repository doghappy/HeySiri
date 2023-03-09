namespace HeySiri.Core.Reporters;

public interface ITelegramBotClient
{
    Task SendTextMessageAsync(string chatId,string text);
}