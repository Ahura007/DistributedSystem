using System.Threading.Tasks;
using System.Transactions;
using Common;
using MassTransit;

namespace App.Host.Read.Consumer
{
    public class WeatherForecastConsumer : IConsumer<WeatherForecast>
    {
        public async Task Consume(ConsumeContext<WeatherForecast> context)
        {
            TransactionContext transactionContext;
            context.TryGetPayload(out transactionContext);


            var a = transactionContext.Transaction.TransactionInformation.Status;
            using (var scope = new TransactionScope(transactionContext.Transaction))
            {
                scope.Complete();
            }

            var b = transactionContext.Transaction.TransactionInformation.Status;



        }
    }
}