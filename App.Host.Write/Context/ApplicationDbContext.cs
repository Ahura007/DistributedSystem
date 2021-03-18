using App.Host.Write.Controllers;
using Common;
using Microsoft.EntityFrameworkCore;

namespace App.Host.Write.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    }
}