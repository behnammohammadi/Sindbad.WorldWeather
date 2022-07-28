namespace Sindbad.WorldWeather.WebApi.Webhooks;

public class PropagationService : IPropagationService
{
    private readonly ILogger<PropagationService> _logger;
    private readonly IEnumerable<IWebHook> _webHooks;

    public PropagationService(ILogger<PropagationService> logger, IEnumerable<IWebHook> webHooks)
    {
        _logger = logger;
        _webHooks = webHooks;
    }

    public async Task Propagate<T>(T message)
    {
        await Parallel.ForEachAsync<IWebHook>(_webHooks, async (webhook, ct) =>
        {
            try
            {
                await webhook.SendAcync(message, ct);
            }
            catch (Exception exception)
            {
                _logger.LogError($"{webhook.Title} | Exception | {exception.Message}");
            }
        });
    }
}