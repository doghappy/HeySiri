using System.CommandLine;
using HeySiri.CLI;
using HeySiri.Core.Bandwagon;
using HeySiri.Core.Glados;
using HeySiri.Core.Hifini;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = CreateServiceProvider();
var rootCommand = CreateRootCommand(
    serviceProvider.GetService<IConfiguration>(),
    serviceProvider.GetService<BandwagonHandler>(),
    serviceProvider.GetService<GladosHandler>(),
    serviceProvider.GetService<HifiniHandler>()
);
await rootCommand.InvokeAsync(args);

IServiceProvider CreateServiceProvider()
{
    IServiceCollection services = new ServiceCollection();
    Startup startup = new Startup();
    startup.ConfigureServices(services);
    return services.BuildServiceProvider();
}

RootCommand CreateRootCommand(
    IConfiguration configuration,
    BandwagonHandler bandwagonHandler,
    GladosHandler gladosHandler,
    HifiniHandler hifiniHandler)
{
    var rootCommand = new RootCommand();
    RegisterBandwagon();
    RegisterGlados();
    RegisterHifini();
    return rootCommand;

    void RegisterBandwagon()
    {
        var veidOption = new Option<string>("--veid", () => configuration["Bandwagon:VEID"]);
        var apiKeyOption = new Option<string>("--api-key", () => configuration["Bandwagon:ApiKey"])
        {
            IsRequired = true,
        };
        var bandwagonCommand = new Command("bandwagon");
        var statusCommand = new Command("status")
        {
            veidOption,
            apiKeyOption,
        };
        bandwagonCommand.AddCommand(statusCommand);
        rootCommand.AddCommand(bandwagonCommand);

        statusCommand.SetHandler(async (veid, apiKey) => await bandwagonHandler.ReportLiveServiceInfoAsync(veid, apiKey), veidOption, apiKeyOption);
    }

    void RegisterGlados()
    {
        var cookieOption = new Option<string>("--cookie", () => configuration["Glados:Cookie"]);
        var gladosCommand = new Command("glados");
        var checkInCommand = new Command("checkin")
        {
            cookieOption,
        };
        gladosCommand.AddCommand(checkInCommand);
        rootCommand.AddCommand(gladosCommand);

        checkInCommand.SetHandler(async cookie => await gladosHandler.ReportCheckInAsync(cookie), cookieOption);
    }

    void RegisterHifini()
    {
        var cookieOption = new Option<string>("--cookie", () => configuration["Hifini:Cookie"]);
        var hifiniCommand = new Command("hifini");
        var checkInCommand = new Command("checkin")
        {
            cookieOption,
        };
        hifiniCommand.AddCommand(checkInCommand);
        rootCommand.AddCommand(hifiniCommand);

        checkInCommand.SetHandler(async cookie => await hifiniHandler.ReportCheckInAsync(cookie), cookieOption);
    }
}