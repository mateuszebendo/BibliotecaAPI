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
        channel.ExchangeDeclare(exchange: "usuario-alertas", type: ExchangeType.Direct);
        channel.ExchangeDeclare(exchange: "livro-alertas", type: ExchangeType.Direct);
        
        channel.QueueDeclare(queue: "usuarios-bloqueados", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueDeclare(queue: "livro-disponivel", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueDeclare(queue: "livro-lancado", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        
        channel.QueueBind(queue: "usuarios-bloqueados", exchange: "usuario-alertas", routingKey:"bloqueia-usuario");
        channel.QueueBind(queue: "livro-disponivel", exchange: "livro-alertas", routingKey:"novo-livro-disponivel");
        channel.QueueBind(queue: "livro-lancado", exchange: "livro-alertas", routingKey:"novo-livro-lancado");
    }
}