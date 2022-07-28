namespace Sindbad.WorldWeather.WebApi.Entities;

public class Weather
{
    public string City { get; set; }

    public long CalculatedOn { get; set; }

    public string Summary { get; set; }

    public string Description { get; set; }

    public double Temp { get; set; }

    public double TempMin { get; set; }

    public double TempMax { get; set; }

    public double FeelsLike { get; set; }

    public double Visibility { get; set; }

    public double WindSpeed { get; set; }

    public double WindDegree { get; set; }

    public double Cloudiness { get; set; }

    public string Origin { get; set; }
}