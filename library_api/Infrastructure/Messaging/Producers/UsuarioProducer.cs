using System.Text;
using RabbitMQ.Client;

namespace library_api.Infrastructure.Messaging.Producers;

public class UsuarioProducer
{
    private readonly IModel _channel;

    public UsuarioProducer(IModel channel)
    {
        _channel = channel;
    }

    public void EnviaAvisoUsuarioBloqueado(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "usuario-alertas", routingKey: "bloqueia-usuario", basicProperties: null, body: body);
    }
}