using System;
using System.Threading;
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
    public class WeatherForecastController : BaseController
    {
        private readonly ApplicationWriteDbContext _applicationWriteDbContext;
        private readonly IBus _bus;
        private readonly IPublishEndpoint _endpoint;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IServiceProvider _serviceProvider;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBus bus,
            ApplicationWriteDbContext applicationWriteDbContext, IServiceProvider serviceProvider,
            IPublishEndpoint endpoint) : base(logger,
            serviceProvider)
        {
            _logger = logger;
            _bus = bus;
            _applicationWriteDbContext = applicationWriteDbContext;
            _serviceProvider = serviceProvider;
            _endpoint = endpoint;
        }

        [HttpPost]
        public async Task<WeatherForecast> PostAsync(WeatherForecast weatherForecast, CancellationToken cancellationToken)
        {
            //check business validation

            await _applicationWriteDbContext.WeatherForecasts.AddAsync(weatherForecast, cancellationToken);
            await _applicationWriteDbContext.SaveChangesAsync(cancellationToken);

            await _endpoint.Publish(weatherForecast, cancellationToken);

            return weatherForecast;
        }
    }
}