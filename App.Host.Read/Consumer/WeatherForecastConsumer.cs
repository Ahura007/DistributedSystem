using System;
using System.Threading.Tasks;
using System.Transactions;
using App.Host.Read.Context;
using Common;
using Common.Event;
using Common.Model;
using MassTransit;

namespace App.Host.Read.Consumer
{
    public class WeatherForecastConsumer : IConsumer<WeatherForecastAdded>
    {
        private readonly ApplicationReadDbContext _applicationReadDbContext;

        public WeatherForecastConsumer(ApplicationReadDbContext applicationReadDbContext)
        {
            _applicationReadDbContext = applicationReadDbContext;
        }

        public async Task Consume(ConsumeContext<WeatherForecastAdded> context)
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
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}