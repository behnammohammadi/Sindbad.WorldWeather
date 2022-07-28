namespace Sindbad.WorldWeather.WebApi.Mappers;

public static class WeatherMapper
{
    private const string DateTimeFormat = "F dddd, dd MMMM(MM) yyyy, HH:mm zzz";

    public static WeatherDto ToDto(Weather entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new WeatherDto
        {
            CalculatedOn = DateTimeOffset.FromUnixTimeSeconds(entity.CalculatedOn).ToString(DateTimeFormat),
            City = entity.City,
            Cloudiness = $"{entity.Cloudiness} %",
            Description = entity.Description,
            FeelsLike = entity.FeelsLike,
            Summary = entity.Summary,
            Temp = entity.Temp,
            TempMax = entity.TempMax,
            TempMin = entity.TempMin,
            Visibility = entity.Visibility,
            WindDegree = entity.WindDegree,
            WindSpeed = entity.WindSpeed,
            Origin = entity.Origin
        };
    }

    public static Weather ToEntity(SuccessResponse response)
    {
        if (response == null)
        {
            return null;
        }

        return new Entities.Weather
        {
            City = response.Name,
            Summary = string.Join(", ", response.Weather.Select(i => i.Main)),
            Description = string.Join(", ", response.Weather.Select(i => i.Description)),
            Temp = response.Main.Temp,
            TempMin = response.Main.TempMin,
            TempMax = response.Main.TempMax,
            FeelsLike = response.Main.FeelsLike,
            Visibility = response.Visibility,
            Cloudiness = response.Clouds.All,
            WindSpeed = response.Wind.Speed,
            WindDegree = response.Wind.Deg,
            CalculatedOn = response.Dt
        };
    }
}