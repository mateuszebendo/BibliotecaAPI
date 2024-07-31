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
        channel.ExchangeDeclare(exchange: "alertas-gerais", type: ExchangeType.Fanout);
        channel.ExchangeDeclare(exchange: "usuario-alertas", type: ExchangeType.Topic);
        channel.ExchangeDeclare(exchange: "livro-alertas", type: ExchangeType.Direct);
        channel.ExchangeDeclare(exchange: "emprestimo-alertas", type: ExchangeType.Topic);
        
        channel.QueueDeclare(queue: "alerta-admin-usuario", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "alerta-admin-usuario", exchange: "usuario-alertas", routingKey:"usuario.#");
         
        channel.QueueDeclare(queue: "usuarios-alterado", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "usuarios-alterado", exchange: "usuario-alertas", routingKey:"usuario.alterado.*");
        
        channel.QueueDeclare(queue: "usuario-criado", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "usuario-criado", exchange: "usuario-alertas", routingKey:"usuario.criado");
        
        channel.QueueDeclare(queue: "livro-disponivel", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "livro-disponivel", exchange: "livro-alertas", routingKey:"novo-livro-disponivel");
        
        channel.QueueDeclare(queue: "livro-lancado", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "livro-lancado", exchange: "livro-alertas", routingKey:"novo-livro-lancado");
        
        channel.QueueDeclare(queue: "emprestimo-criado", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "emprestimo-criado", exchange: "emprestimo-alertas", routingKey:"emprestimo.criado");

        channel.QueueDeclare(queue: "emprestimo-finalizado", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "emprestimo-finalizado", exchange: "emprestimo-alertas", routingKey:"emprestimo.finalizado");
        
        channel.QueueDeclare(queue: "alerta-admin", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "alerta-admin", exchange: "alertas-gerais", routingKey:"novo-livro-lancado");
        
        channel.QueueDeclare(queue: "alerta-usuario", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind(queue: "alerta-usuario", exchange: "alertas-gerais", routingKey:"novo-livro-lancado");

    }
}