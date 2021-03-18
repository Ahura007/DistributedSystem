using System;
using Common;
using Microsoft.EntityFrameworkCore;

namespace App.Host.Read.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }


        public override int SaveChanges()
        {
            throw new InvalidOperationException("This context is read-only.");
        }
    }
}