using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using WriterService;
using Newtonsoft.Json.Linq;
// See https://aka.ms/new-console-template for more information

ManualResetEvent _quitEvent = new ManualResetEvent(false);

Console.CancelKeyPress += (sender, eArgs) => {
    _quitEvent.Set();
    eArgs.Cancel = true;
};

Console.WriteLine("A writing service to create txt files after users have been created");

string websiteUrl = Environment.GetEnvironmentVariable("WEBSITE_URL") ?? string.Empty;

Task.Delay(15000).Wait();

Console.WriteLine("Trying to Connect to Queue");

ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
factory.UserName = "guest";
factory.Password = "guest";
IConnection conn = factory.CreateConnection();
IModel channel = conn.CreateModel();
channel.QueueDeclare(queue: "creation",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

Console.WriteLine("Consuming Queue Now");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    string cleanedMessage =  message.Replace("server processed ", "").Replace("  ", " ");

    User user = JsonConvert.DeserializeObject<User>(cleanedMessage);

    string fileName = $"Welcome-{user.Username}.txt";
    string text =
    $"Hello {user.FirstName} {user.LastName},\n" +
    "Welcome to Finquest candidate test platform, your registering have been approved, and now\n" +
    $"you can connect to {websiteUrl}/login to use the platform.";

    Console.WriteLine(text);

    File.WriteAllText(fileName, text);
    Console.WriteLine($"Writing to file {fileName} Done");

    Console.WriteLine(" [x] Received from Rabbit: {0}", message);
};
channel.BasicConsume(queue: "creation",
                        autoAck: true,
                        consumer: consumer);

Console.ReadLine();