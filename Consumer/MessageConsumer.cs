using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Transit.Models;

namespace Consumer
{
    public class MessageConsumer : IConsumer<IMessageSubmitted>
    {
        public async Task Consume(ConsumeContext<IMessageSubmitted> context)
        {
            Console.WriteLine($"Message : {context.Message.Message}");
        }
    }
}
