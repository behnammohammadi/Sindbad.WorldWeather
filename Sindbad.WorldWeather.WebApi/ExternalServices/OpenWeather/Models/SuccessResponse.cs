namespace Sindbad.WorldWeather.WebApi.ExternalServices.OpenWeather.Models;

public class SuccessResponse
{
    public List<WeatherDescription> Weather { get; set; }

    public Main Main { get; set; }

    public int Visibility { get; set; }

    public Wind Wind { get; set; }

    public Clouds Clouds { get; set; }

    public long Dt { get; set; }

    public string Name { get; set; }
}