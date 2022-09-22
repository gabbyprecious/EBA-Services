using System;
using RabbitMQ.Client;
using System.Text;

namespace FinquestTest.Services
{
    public interface IMessageService
    {
        bool EnqueueCreate(string message);
        bool EnqueueUpdate(string message);
    }

    public class MessageService : IMessageService
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _createChannel;
        IModel _updateChannel;
        public MessageService()
        {
            Console.WriteLine("about to connect to rabbit");

            _factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            _factory.UserName = "guest";
            _factory.Password = "guest";
            _conn = _factory.CreateConnection();
            _createChannel = _conn.CreateModel();
            _updateChannel = _conn.CreateModel();
            _createChannel.QueueDeclare(queue: "creation",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            _updateChannel.QueueDeclare(queue: "update",    
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);   
        }
        public bool EnqueueCreate(string messageString)
        {
            var body = Encoding.UTF8.GetBytes("server processed " + messageString);
            _createChannel.BasicPublish(exchange: "",
                                routingKey: "creation",
                                basicProperties: null,
                                body: body);
            Console.WriteLine(" [x] Published {0} to RabbitMQ", messageString);
            return true;
        }

        public bool EnqueueUpdate(string messageString)
        {
            var body = Encoding.UTF8.GetBytes("server processed " + messageString);
            _updateChannel.BasicPublish(exchange: "",
                                routingKey: "update",
                                basicProperties: null,
                                body: body);
            Console.WriteLine(" [x] Published {0} to RabbitMQ", messageString);
            return true;
        }
    }
}
