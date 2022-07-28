namespace Sindbad.WorldWeather.WebApi.ExternalServices.OpenWeather;

public class OpenWeatherService : IOpenWeatherService
{
    private const string Api = @"http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&appid=3a63c95460191cd24a279ad82727a52f";

    private readonly JsonSerializerOptions serializeOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public OpenWeatherService(HttpClient httpClient)
    {
        HttpClient = httpClient;
        HttpClient.Timeout = TimeSpan.FromSeconds(4);
    }

    private HttpClient HttpClient { get; }

    public async Task<OpenWeatherResponse> GetByCityAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return OpenWeatherResponse.CreateCityError();
        }

        using HttpRequestMessage request = new(HttpMethod.Get, string.Format(Api, city));
        using var httpResponse = await HttpClient.SendAsync(request);
        var responseBody = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.IsSuccessStatusCode)
        {
            var content = System.Text.Json.JsonSerializer.Deserialize<SuccessResponse>(responseBody, serializeOptions);
            return OpenWeatherResponse.CreateSuccessResponse(content);
        }

        var error = System.Text.Json.JsonSerializer.Deserialize<Error>(responseBody, serializeOptions);
        return OpenWeatherResponse.CreateErrorResponse(error);
    }
}