using Polly.Timeout;
using Sindbad.WorldWeather.WebApi.Mappers;

namespace Sindbad.WorldWeather.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> _logger;
    private readonly IWeatherProxy _proxy;
    private readonly IWeatherRepository _repository;
    private readonly IPropagationService _propagationService;

    public TimeoutPolicy GeneralTimeoutPolicy { get; }

    public WeatherController(
                            ILogger<WeatherController> logger,
                            IWeatherProxy proxy,
                            IWeatherRepository repository,
                            IPropagationService propagationService)
    {
        _logger = logger;
        _proxy = proxy;
        _repository = repository;
        _propagationService = propagationService;
    }

    [HttpGet("{city}", Name = nameof(GetCurrentWeather))]
    [ResponseCache(Duration = 10, VaryByQueryKeys = new[] { "city" })]
    public async Task<IActionResult> GetCurrentWeather(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return BadRequest("The city value was not provided.");
        }

        var proxyResponse = await ResolveRemotely(city);

        var isNotFound = proxyResponse.Code == 404;
        if (isNotFound)
        {
            return NotFound(proxyResponse.ErrorMessage);
        }

        var entity = proxyResponse?.Content;
        if (entity == null)
        {
            entity = await ResolveLocally(city);
        }

        if (entity == null)
        {
            return Ok(null);
        }

        var dto = WeatherMapper.ToDto(entity);

        _ = _propagationService.Propagate(dto);

        return Ok(dto);
    }

    private async Task<ProxyResponse> ResolveRemotely(string city)
    {
        ProxyResponse response;
        try
        {
            response = await _proxy.GetByCity(city);
        }
        catch (Exception ex)
        {
            return new ProxyResponse
            {
                Code = 500,
                ErrorMessage = ex.Message
            };
        }

        if (response.IsSuccess && response.Content != null)
        {
            response.Content.Origin = "Remote";
            _ = Task.Run(async () => await _repository.Add(response.Content));
        }

        return response;
    }

    private async Task<Weather> ResolveLocally(string city)
    {
        var entity = await _repository.GetLatestOrDefaultAsync(city);
        if (entity != null)
        {
            entity.Origin = "Local";
        }

        return entity;
    }
}