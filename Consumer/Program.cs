using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => //Connect to Host
                {
                    h.Username("guest"); //Default User
                    h.Password("guest"); //Default Password
                });
                cfg.ReceiveEndpoint("message-listener", e =>
                {
                    e.Consumer<MessageConsumer>();
                });
            });
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await busControl.StartAsync(source.Token); //Start Service
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
