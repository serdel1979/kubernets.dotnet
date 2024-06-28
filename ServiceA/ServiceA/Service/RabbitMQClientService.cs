using RabbitMQ.Client;
using System.Text;

namespace ServiceA.Service
{
    public class RabbitMQClientService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQClientService()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" }; // Nombre del servicio RabbitMQ en Kubernetes
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "testQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                  routingKey: "testQueue",
                                  basicProperties: null,
                                  body: body);
        }

        ~RabbitMQClientService()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
