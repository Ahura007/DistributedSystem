using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.EntityFrameworkCore;

namespace App.Host.Read.Context
{
    public class ApplicationReadDbContext : DbContext
    {
        public ApplicationReadDbContext(DbContextOptions<ApplicationReadDbContext> options) : base(options)
        {
        }


        public DbSet<WeatherForecast> WeatherForecasts { get; set; }


        public override int SaveChanges()
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new InvalidOperationException("This context is read-only.");
        }
    }
}