using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RepositoryLayer.Interface;
using ModelLayer.Model;
using BusinessLayer.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryLayer.Service
{
    public class RabbitMQConsumer : BackgroundService, IRabbitMQConsumer
    {
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<RabbitMQConsumer> _logger;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public RabbitMQConsumer(IConfiguration config, IServiceScopeFactory serviceScopeFactory, ILogger<RabbitMQConsumer> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:Host"],
                UserName = _config["RabbitMQ:Username"],
                Password = _config["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = _config["RabbitMQ:QueueName"];
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            InitializeRabbitMQ();
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using (var scope = _serviceScopeFactory.CreateScope())  // 🔹 Create Scope for Scoped Services
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();  // 🔹 Get IEmailService

                    try
                    {
                        var emailModel = JsonConvert.DeserializeObject<EmailModel>(message);
                        if (emailModel == null)
                        {
                            _logger.LogError("Invalid message format received.");
                            return;
                        }

                        emailService.SendEmail(emailModel);
                        _logger.LogInformation($"Email sent successfully to: {emailModel.To}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing RabbitMQ message.");
                    }
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
