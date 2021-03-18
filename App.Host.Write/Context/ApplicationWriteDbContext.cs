using App.Host.Write.Controllers;
using Common;
using Microsoft.EntityFrameworkCore;

namespace App.Host.Write.Context
{
    public class ApplicationWriteDbContext : DbContext
    {
        public ApplicationWriteDbContext(DbContextOptions<ApplicationWriteDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    }
}