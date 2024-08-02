using System.Text;
using System.Text.Json;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.Messaging.Producers;
using RabbitMQ.Client;

namespace BibliotecaAPI.Infrastructure.Messaging.Producers;

public class LivroProducer : ILivroProducer
{
    private readonly IModel _channel;

    public LivroProducer(IModel channel)
    {
        _channel = channel;
    }

    public void EnviaAvisoLivroDisponivel(Livro livroDto)
    {
        var json = JsonSerializer.Serialize(livroDto);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "livro-alertas", routingKey: "novo-livro-disponivel", basicProperties: null, body: body);
    }
    
    public void EnviaAvisoLivroLancado(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "livro-alertas", routingKey: "novo-livro-lancado", basicProperties: null, body: body);
    }
}
