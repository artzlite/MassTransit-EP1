using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Transit.Models;

namespace Publisher
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
            });
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await busControl.StartAsync(source.Token); //Start Service
            try
            {
                while (true)
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });
                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;
                    //Publish Message
                    await busControl.Publish<IMessageSubmitted>(new 
                    {
                        Message = value
                    });
                }
            }
            finally
            {
                //Ensure Stop Service
                await busControl.StopAsync();
            }
        }
    }
}
