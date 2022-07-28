namespace Sindbad.WorldWeather.WebApi.Webhooks;

public interface IWebHook
{
    string Title { get; }

    Task SendAcync<T>(T message, CancellationToken cancellationToken = default(CancellationToken));
}