using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
           // CreateHostBuilder(args).Build().Run();
            string url = "amqps://lqdhszah:FsyKTNh5FYyVMShPAfce0d5RTC4L9ROb@sparrow.rmq.cloudamqp.com/lqdhszah";
            // create a connection and open a channel, dispose them when done
            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri(url)
            };

            
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.BasicQos(0, 1, false);

            MessageReceiver messageReceiver = new MessageReceiver(channel);

            channel.BasicConsume("Sensor_simulator", false, messageReceiver);
 
           Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
