using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Model
{
    public class WeatherForecast
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
}