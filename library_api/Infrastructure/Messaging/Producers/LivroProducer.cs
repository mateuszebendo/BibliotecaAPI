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

    public void EnviaAvisoLivroDisponivel(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "livro-alertas", routingKey: "novo-livro-disponivel", basicProperties: null, body: body);
    }
    
    public void EnviaAvisoLivroLancado(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "livro-alertas", routingKey: "novo-livro-lancado", basicProperties: null, body: body);
    }
}
