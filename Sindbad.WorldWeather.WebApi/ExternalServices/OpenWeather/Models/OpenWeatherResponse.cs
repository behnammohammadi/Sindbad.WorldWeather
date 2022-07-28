namespace Sindbad.WorldWeather.WebApi.ExternalServices.OpenWeather.Models;

public class OpenWeatherResponse
{
    private OpenWeatherResponse()
    {
    }

    public static OpenWeatherResponse CreateCityError()
    {
        return new OpenWeatherResponse
        {
            Code = 404,
            ErrorMessage = "City must be defined",
        };
    }

    public static OpenWeatherResponse CreateErrorResponse(Error error)
    {
        int errorCode = -1;
        int.TryParse(error.Code, out errorCode);
        return new OpenWeatherResponse
        {
            Code = errorCode,
            ErrorMessage = error.Message ?? "Error message is not Provided",
        };
    }

    public static OpenWeatherResponse CreateSuccessResponse(SuccessResponse content)
    {
        return new OpenWeatherResponse
        {
            Code = 200,
            Content = content
        };
    }

    public int? Code { get; private set; }

    public string ErrorMessage { get; private set; }

    public SuccessResponse Content { get; private set; }

    public bool IsSuccess => Code == 200;
}