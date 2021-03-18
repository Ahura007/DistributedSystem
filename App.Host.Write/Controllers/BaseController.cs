using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Host.Write.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        public readonly ILogger<BaseController> Logger;

        public readonly IServiceProvider ServiceProvider;

        public BaseController(ILogger<BaseController> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
        }
    }
}