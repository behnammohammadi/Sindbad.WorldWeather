namespace Sindbad.WorldWeather.WebApi.Webhooks;

public interface IPropagationService
{
    Task Propagate<T>(T message);
}