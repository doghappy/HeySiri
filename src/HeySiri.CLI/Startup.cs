using HeySiri.Core;
using HeySiri.Core.Bandwagon;
using HeySiri.Core.Glados;
using HeySiri.Core.Hifini;
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
        services.AddHttpClient();
        services.AddSingleton<IDateTimeProvider>(new SystemDateTimeProvider());
        services.RegisterReporter(Configuration);
        services.RegisterHandler<BandwagonHandler>();
        services.RegisterHandler<GladosHandler>();
        services.RegisterHandler<HifiniHandler>();
    }
}