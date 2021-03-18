using System;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class WeatherForecast : IWeatherForecast
    {
        public WeatherForecast()
        {
            Date = DateTime.Now;
        }

        [Key] public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    public interface IWeatherForecast
    {
        int Id { get; set; }
        DateTime Date { get; set; }

        int TemperatureC { get; set; }

        int TemperatureF { get; }

        string Summary { get; set; }
    }
}