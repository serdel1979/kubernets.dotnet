using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace ServiceB.Service
{
    public class RabbitMQListenerService : BackgroundService
    {

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQListenerService()
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Received message: {content}");
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("testQueue", false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

    }
}
