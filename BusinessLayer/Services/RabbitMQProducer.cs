using BusinessLayer.Interface;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IConfiguration _config;

        public RabbitMQProducer(IConfiguration config)
        {
            _config = config;
        }

        public void SendProductMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:Host"]
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string queueName = _config["RabbitMQ:QueueName"];
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
        }
    }
}
