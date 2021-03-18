using System.Threading.Tasks;
using App.Host.Write.Context;
using Common;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Host.Write.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly IBus _bus;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBus bus,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _bus = bus;
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost]
        public async Task<WeatherForecast> PostAsync(WeatherForecast weatherForecast)
        {
            await _applicationDbContext.WeatherForecasts.AddAsync(weatherForecast);
            await _applicationDbContext.SaveChangesAsync();
            await _bus.Publish<WeatherForecast>(weatherForecast);
            return weatherForecast;
        }
    }
}