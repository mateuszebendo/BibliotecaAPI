using System.Text;
using RabbitMQ.Client;

namespace library_api.Infrastructure.Messaging.Producers;

public class LivroProducer
{
    private readonly IModel _channel;

    public LivroProducer(IModel channel)
    {
        _channel = channel;
    }

    public void EnviarMensagem(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "livros_exchange", routingKey: "novo_livro", basicProperties: null, body: body);
    }
}
