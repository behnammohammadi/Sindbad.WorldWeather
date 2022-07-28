using Sindbad.WorldWeather.WebApi.Entities;

namespace Sindbad.WorldWeather.Test.Controllers;

public class WeatherControllerTest
{
    [Fact]
    public async Task GetBadRequestError()
    {
        var logger = Mock.Of<ILogger<WeatherController>>();
        var proxy = Mock.Of<IWeatherProxy>();
        var repository = Mock.Of<IWeatherRepository>();

        var propagationService = new Mock<IPropagationService>();
        propagationService.Setup(s => s.Propagate<WeatherDto>(It.IsAny<WeatherDto>()))
                          .Returns(Task.CompletedTask);

        var controller = new WeatherController(logger, proxy, repository, propagationService.Object);
        var actualResult = await controller.GetCurrentWeather(null);
        actualResult.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetNotFoundError()
    {
        var cityName = "XXXXXXX";

        var logger = Mock.Of<ILogger<WeatherController>>();
        var repository = Mock.Of<IWeatherRepository>();

        var proxy = new Mock<IWeatherProxy>();
        proxy.Setup(s => s.GetByCity(cityName))
            .Returns(Task.FromResult(new ProxyResponse { Code = 404, ErrorMessage = "Not Found" }));

        var propagationService = new Mock<IPropagationService>();
        propagationService.Setup(s => s.Propagate<WeatherDto>(It.IsAny<WeatherDto>()))
                          .Returns(Task.CompletedTask);

        var controller = new WeatherController(logger, proxy.Object, repository, propagationService.Object);
        var actualResult = await controller.GetCurrentWeather(cityName);
        actualResult.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetNullResponse()
    {
        var cityName = "Milan";

        var logger = Mock.Of<ILogger<WeatherController>>();
        var repository = new Mock<IWeatherRepository>();
        repository.Setup(s => s.GetLatestOrDefaultAsync(cityName))
            .Returns(Task.FromResult<Weather>(null));

        var proxy = new Mock<IWeatherProxy>();
        proxy.Setup(s => s.GetByCity(cityName))
            .Returns(Task.FromResult(new ProxyResponse { Code = -1, ErrorMessage = "some Exception" }));

        var propagationService = new Mock<IPropagationService>();
        propagationService.Setup(s => s.Propagate<WeatherDto>(It.IsAny<WeatherDto>()))
                          .Returns(Task.CompletedTask);

        var controller = new WeatherController(logger, proxy.Object, repository.Object, propagationService.Object);
        var actualResult = await controller.GetCurrentWeather(cityName);
        var actualResponse = actualResult.Should().BeOfType<OkObjectResult>();
        actualResponse.Subject.Value.Should().BeNull();
    }
}