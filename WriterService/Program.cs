using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
// See https://aka.ms/new-console-template for more information

ManualResetEvent _quitEvent = new ManualResetEvent(false);

Console.CancelKeyPress += (sender, eArgs) => {
    _quitEvent.Set();
    eArgs.Cancel = true;
};

string getEnv = Environment.GetEnvironmentVariable("WEBSITE_URL") ?? string.Empty;
Console.WriteLine(getEnv);
Console.WriteLine("Hello, World!");

Task.Delay(10000).Wait();

Console.WriteLine("Trying to Connect to Queue");

ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
factory.UserName = "guest";
factory.Password = "guest";
IConnection conn = factory.CreateConnection();
IModel channel = conn.CreateModel();
channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

Task.Delay(90000).Wait();
Console.WriteLine("Consuming Queue Now");

var consumer = new EventingBasicConsumer(channel);
Console.WriteLine("consumer", consumer);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.Span;
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received from Rabbit: {0}", message);
};
channel.BasicConsume(queue: "hello",
                        autoAck: true,
                        consumer: consumer);

Console.ReadLine();