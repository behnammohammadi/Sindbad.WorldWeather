namespace Sindbad.WorldWeather.WebApi.ExternalServices.OpenWeather.Models;

public class Main
{
    public double Temp { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("temp_min")]
    public double TempMin { get; set; }

    [JsonPropertyName("temp_max")]
    public double TempMax { get; set; }

    public int Pressure { get; set; }

    public int Humidity { get; set; }
}