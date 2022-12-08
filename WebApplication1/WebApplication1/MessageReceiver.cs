using System;

using System.Text;

using RabbitMQ.Client;



namespace WebApplication1

{

    public class MessageReceiver : DefaultBasicConsumer

    {

        private readonly IModel _channel;

        public MessageReceiver(IModel channel)

        { 

            _channel = channel;

        }
       
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)

        {

            Console.WriteLine($"Consuming Message");

            Console.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(body.Span)));

            _channel.BasicAck(deliveryTag, false);

        }

    }

}