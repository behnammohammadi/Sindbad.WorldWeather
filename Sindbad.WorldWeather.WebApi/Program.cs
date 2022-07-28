namespace Sindbad.WorldWeather.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Logging.AddConfiguration(builder.Configuration);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();

        var zzz = serviceCollection.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

        var serviceProvider = builder.Services.BuildServiceProvider();
        var logger = serviceProvider.GetService<ILogger<Program>>();

        AddServicessToDIContainer(builder.Services, logger);

        var app = builder.Build();

        ConfigurePipeline(app);

        app.Run();
    }

    private static void AddServicessToDIContainer(IServiceCollection services, ILogger logger)
    {
        services.AddControllers(option =>
        {
            option.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddResponseCaching(option =>
        {
            option.MaximumBodySize = 1024;
            option.SizeLimit = 512;
        });

        services.AddSingleton<DbContext>();
        services.AddTransient<IWeatherRepository, WeatherRepository>();
        services.AddTransient<IWeatherProxy, OpenWeatherProxy>();
        services.AddTransient<IPropagationService, PropagationService>();

        services.AddHttpClient<IOpenWeatherService, OpenWeatherService>()
            .AddTypedClient<IOpenWeatherService, OpenWeatherService>()
            .AddPolicyHandler(new RetryPolicies(logger).GetHttpPolicy());

        services.AddHttpClient<IWebHook, WebhooksiteHook>()
              .AddTypedClient<IOpenWeatherService, OpenWeatherService>()
              .AddPolicyHandler(new RetryPolicies(logger).GetHttpPolicy());
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseResponseCaching();

        app.MapControllers();
    }
}