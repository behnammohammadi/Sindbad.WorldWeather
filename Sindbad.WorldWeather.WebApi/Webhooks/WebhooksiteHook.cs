using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Sindbad.WorldWeather.WebApi.Webhooks;

public class WebhooksiteHook : IWebHook
{
    private readonly ILogger<WebhooksiteHook> _logger;
    private readonly HttpClient _httpClient;

    public WebhooksiteHook(ILogger<WebhooksiteHook> logger, HttpClient httpClient)
    {
        this._logger = logger;

        this._httpClient = httpClient;
        this._httpClient.Timeout = TimeSpan.FromSeconds(7);
    }

    public string Title => "webhook.site";

    public async Task SendAcync<T>(T message, CancellationToken cancellationToken = default(CancellationToken))
    {
        var body = new StringContent(
                               JsonSerializer.Serialize<T>(message),
                               Encoding.UTF8,
                               Application.Json);

        using var httpResponseMessage =
            await _httpClient.PostAsync("https://webhook.site/f26b8410-e4eb-407c-a478-12c18a624a6e", body);

        _logger.LogInformation($"Web-hook |{nameof(WebhooksiteHook)} | {httpResponseMessage.StatusCode} | {httpResponseMessage.ReasonPhrase}  ");
    }
}