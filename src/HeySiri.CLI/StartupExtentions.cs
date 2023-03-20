using HeySiri.Core;
using HeySiri.Core.Reporters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace HeySiri.CLI;

public static class StartupExtentions
{
    public static void RegisterHandler<T>(this IServiceCollection services) where T:class
    {
        services.AddSingleton<T>(sp =>
        {
            var factory = sp.GetService<IHttpClientFactory>();
            var httpClient = factory.CreateClient();
            var reporter = sp.GetService<IReporter>();
            return (T)Activator.CreateInstance(typeof(T), httpClient, reporter);
        });
    }

    public static void RegisterReporter(this IServiceCollection services, IConfiguration configuration)
    {
        var reporter = configuration["Reporter"];
        if (reporter == "Telegram")
        {
            var telegramConfiguration = new TelegramConfiguration();
            configuration.GetSection("Telegram").Bind(telegramConfiguration);
            services.AddSingleton<ITelegramBotClient>(new MyTelegramBotClient(telegramConfiguration.Token));
            services.AddSingleton<IReporter>(sp => new TelegramReporter(configuration["ReportTimeZone"],
                sp.GetService<IDateTimeProvider>(),
                sp.GetService<ITelegramBotClient>(),
                telegramConfiguration));
        }
        else
        {
            services.AddSingleton<IReporter>(sp =>
            {
                var logger = sp.GetService<ILoggerFactory>().CreateLogger<LogInformationReporter>();
                var dateTimeProvider = sp.GetService<IDateTimeProvider>();
                return new LogInformationReporter(configuration["ReportTimeZone"], dateTimeProvider, logger);
            });
        }
    }
}