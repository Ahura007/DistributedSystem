using System.Collections.Generic;
using System.Threading.Tasks;
using App.Host.Read.Context;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Host.Read.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ApplicationReadDbContext _applicationReadDbContext;
        private readonly ILogger<WeatherForecastController> _logger;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, ApplicationReadDbContext applicationReadDbContext)
        {
            _logger = logger;
            _applicationReadDbContext = applicationReadDbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            return await _applicationReadDbContext.WeatherForecasts.ToListAsync();
        }
    }
}