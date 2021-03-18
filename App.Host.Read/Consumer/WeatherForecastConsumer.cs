using System;
using System.Threading.Tasks;
using System.Transactions;
using App.Host.Read.Context;
using Common;
using MassTransit;

namespace App.Host.Read.Consumer
{
    public class WeatherForecastConsumer : IConsumer<WeatherForecast>
    {
        private readonly ApplicationReadDbContext _applicationReadDbContext;

        public WeatherForecastConsumer(ApplicationReadDbContext applicationReadDbContext)
        {
            _applicationReadDbContext = applicationReadDbContext;
        }

        public async Task Consume(ConsumeContext<WeatherForecast> context)
        {
            TransactionContext transactionContext;
            context.TryGetPayload(out transactionContext);


            using var scope = context.CreateTransactionScope(TimeSpan.FromSeconds(10));
            try
            {
                scope.Complete();
            }
            catch (TransactionAbortedException e)
            {
                
            }
            catch (Exception e)
            {
                scope.Dispose();
                Console.WriteLine(e);
                throw;
            }
        }
    }
}