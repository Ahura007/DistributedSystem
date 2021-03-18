using System;
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
        private readonly ApplicationWriteDbContext _applicationWriteDbContext;

        private readonly IBus _bus;

        private readonly ILogger<WeatherForecastController> _logger;
 

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBus bus,
            ApplicationWriteDbContext applicationWriteDbContext)
        {
            _logger = logger;
            _bus = bus;
            _applicationWriteDbContext = applicationWriteDbContext;
        }

        [HttpPost]
        public async Task<WeatherForecast> PostAsync(WeatherForecast weatherForecast)
        {
            //check business validation
            await _applicationWriteDbContext.WeatherForecasts.AddAsync(weatherForecast);
            await _applicationWriteDbContext.SaveChangesAsync();
   
            await _bus.Publish<WeatherForecast>(weatherForecast);
            return weatherForecast;
        }
    }
}