using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace BibliotecaAPI.Infrastructure.Messaging;

public static class RabbitMQConfig
{
    public static void AddRabbitMQServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnection>(sp =>
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQ");
            var connectionFactory = new ConnectionFactory()
            {
                // HostName = rabbitMQConfig["Host"],
                // Port = int.Parse(rabbitMQConfig["Port"]),
                // UserName = rabbitMQConfig["Username"],
                // Password = rabbitMQConfig["Password"]
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            return connectionFactory.CreateConnection();
        });

        services.AddSingleton<IModel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            var channel = connection.CreateModel();
            SetupQueuesAndExchanges(channel, configuration);
            return channel;
        });
    }

    public static void SetupQueuesAndExchanges(IModel channel, IConfiguration configuration)
    {
        var exchanges = configuration.GetSection("Exchanges").Get<List<ExchangeConfig>>();
        var queues = configuration.GetSection("Queues").Get<List<QueueConfig>>();

        foreach (var exchange in exchanges)
        {
            channel.ExchangeDeclare(exchange: exchange.Name, type: exchange.Type);
        }

        foreach (var queue in queues)
        {
            channel.QueueDeclare(queue: queue.Name, durable: queue.Durable, exclusive: queue.Exclusive, autoDelete: queue.AutoDelete, arguments: queue.Arguments);
    
            foreach (var binding in queue.Bindings)
            {
                channel.QueueBind(queue: queue.Name, exchange: binding.Exchange, routingKey: binding.RoutingKey);
            }
        }
    }

}