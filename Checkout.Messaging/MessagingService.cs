using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Checkout.Messaging
{
    public class MessagingService : IMessagingService
    {
        public void EnqueueMessage(string rabbitMQHostname, string exchangeName, object message)
        {
            var factory = new ConnectionFactory() { HostName = rabbitMQHostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName, 
                                        type: "fanout");


                var messageAsJsonString = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageAsJsonString);

                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }

        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;

        public void ActivateMessageListener(string rabbitMQHostname, string exchangeName, Action<string> predicate)
        {
            var factory = new ConnectionFactory() { HostName = rabbitMQHostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: exchangeName,
                                     type: "fanout");

            var queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: "");

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                predicate(message);
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: _consumer);
        }

        public void DeactivateMessageListener()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
