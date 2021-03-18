using System;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using App.Host.Read.Context;
using Common;
using GreenPipes;
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

            var message = context.Message;

   
            var a = transactionContext.Transaction.TransactionInformation.Status;


            using (var scope = context.CreateTransactionScope(TimeSpan.FromMinutes(90)))
            {
                try
                {
                    scope.Complete();
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    Console.WriteLine(e);
                    throw;
                }
            }

            var b = transactionContext.Transaction.TransactionInformation.Status;
        }
    }
}