using RabbitMQ.Client;

namespace library_api.Infrastructure.Messaging;

internal static class RabbitMQConfig
{
    public static void AddRabbitMQServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnection>(sp =>
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:Host"],
                Port = int.Parse(configuration["RabbitMQ:Port"]),
                UserName = configuration["RabbitMQ:Username"],
                Password = configuration["RabbitMQ:Password"]
            };
            return connectionFactory.CreateConnection();
        });

        services.AddSingleton<IModel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            var channel = connection.CreateModel();
            SetupQueuesAndExchanges(channel);
            return channel;
        });
    }

    public static void SetupQueuesAndExchanges(IModel channel)
    {
        channel.ExchangeDeclare(exchange: "livros_exchange", type: ExchangeType.Direct);
        channel.QueueDeclare(queue: "novos_livros_queue", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "novos_livros_queue", exchange: "livros_exchange", routingKey: "novo_livro");
    }
}