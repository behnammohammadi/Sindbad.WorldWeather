using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Sindbad.WorldWeather.WebApi;

public class RetryPolicies
{
    private readonly ILogger _logger;

    public RetryPolicies(ILogger logger)
    {
        this._logger = logger;
    }

    public IAsyncPolicy<HttpResponseMessage> GetHttpPolicy(int medianFirstRetryDelay = 600, int retryCount = 4, int timeOut = 3500)
    {
        var cbPolicy = GetCircuitBreakerPolicy();
        var retryPolicy = GetRetryPolicy(
                                    medianFirstRetryDelay: medianFirstRetryDelay,
                                    retryCount: retryCount,
                                    timeOut: timeOut);

        return Policy.WrapAsync<HttpResponseMessage>(cbPolicy, retryPolicy);
    }

    private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .AdvancedCircuitBreakerAsync(0.4, TimeSpan.FromSeconds(10), 20, TimeSpan.FromSeconds(5))
                    ;
    }

    private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int medianFirstRetryDelay = 600, int retryCount = 4, int timeOut = 3500)
    {
        var delay = Backoff
                        .DecorrelatedJitterBackoffV2(
                                    medianFirstRetryDelay: TimeSpan.FromMilliseconds(medianFirstRetryDelay),
                                    retryCount: retryCount,
                                    fastFirst: false)
                        .OrderBy(x => x.Ticks)
                        .Where(x => x < TimeSpan.FromMilliseconds(timeOut))
                        .ToArray();

        var retryPolicy = HttpPolicyExtensions
                               .HandleTransientHttpError()
                               .WaitAndRetryAsync(delay, (del, time) =>
                               {
                                   _logger.LogTrace($"WaitAndRetry |{time}  {del.Exception.Message}");
                               });

        return retryPolicy;
    }
}