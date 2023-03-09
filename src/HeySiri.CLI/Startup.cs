using HeySiri.Core;
using HeySiri.Core.Bandwagon;
using HeySiri.Core.Glados;
using HeySiri.Core.Reporters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HeySiri.CLI;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        const string development = "Development";
        if (string.IsNullOrEmpty(env) || env == development)
        {
            builder.AddJsonFile($"appsettings.{development}.json", true);
        }

        builder.AddUserSecrets(GetType().Assembly);

        Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder
                .AddConfiguration(Configuration)
                .AddConsole();
        });
        services.AddSingleton(Configuration);
        services.AddHttpClient(BandwagonHandler.Bandwagon, http => http.BaseAddress = new Uri(Configuration["Bandwagon:BaseAddress"]));
        services.AddHttpClient(GladosHandler.Glados, http => http.BaseAddress = new Uri(Configuration["Glados:BaseAddress"]));
        services.AddSingleton<IDateTimeProvider>(new SystemDateTimeProvider());
        RegisterReporter();
        services.AddSingleton(sp =>
        {
            var factory = sp.GetService<IHttpClientFactory>();
            var httpClient = factory.CreateClient(BandwagonHandler.Bandwagon);
            var repoter = sp.GetService<IReporter>();
            return new BandwagonHandler(httpClient, repoter);
        });
        services.AddSingleton(sp =>
        {
            var factory = sp.GetService<IHttpClientFactory>();
            var httpClient = factory.CreateClient(GladosHandler.Glados);
            var repoter = sp.GetService<IReporter>();
            return new GladosHandler(httpClient, repoter);
        });

        void RegisterReporter()
        {
            var reporter = Configuration["Reporter"];
            if (reporter == "Telegram")
            {
                var configuration = new TelegramConfiguration();
                Configuration.GetSection("Telegram").Bind(configuration);
                services.AddSingleton<ITelegramBotClient>(new MyTelegramBotClient(configuration.Token));
                services.AddSingleton<IReporter>(sp => new TelegramReporter(Configuration["ReportTimeZone"], 
                    sp.GetService<IDateTimeProvider>(), 
                    sp.GetService<ITelegramBotClient>(), 
                    configuration));
            }
            else
            {
                services.AddSingleton<IReporter>(sp =>
                {
                    var logger = sp.GetService<ILoggerFactory>().CreateLogger<LogInformationReporter>();
                    var dateTimeProvider = sp.GetService<IDateTimeProvider>();
                    return new LogInformationReporter(Configuration["ReportTimeZone"], dateTimeProvider, logger);
                });
            }
        }
    }
}