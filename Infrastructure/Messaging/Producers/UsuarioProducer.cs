using System.Text;
using System.Text.Json;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.Messaging.Producers;
using RabbitMQ.Client;

namespace BibliotecaAPI.Infrastructure.Messaging.Producers;

public class UsuarioProducer : IUsuarioProducer
{
    private readonly IModel _channel;

    public UsuarioProducer(IModel channel)
    {
        _channel = channel;
    }

    public void EnviaAvisoUsuarioBloqueado(Usuario usuario)
    {
        var json = JsonSerializer.Serialize(usuario);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "usuario-alertas", routingKey: "usuario.alterado.bloqueado", basicProperties: null, body: body);
    }
    
    public void  EnviaAvisoUsuarioDesabilitado(Usuario usuario)
    {
        var json = JsonSerializer.Serialize(usuario);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "usuario-alertas", routingKey: "usuario.alterado.desabilitado", basicProperties: null, body: body);
    }

    public void EnviaAvisoNovoUsuarioCriado(Usuario usuario)
    {
        var json = JsonSerializer.Serialize(usuario);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "usuario-alertas", routingKey: "usuario.criado", basicProperties: null, body: body);
    }
    
    public void EnviaUsuarioAlterado(Usuario usuario)
    {
        var json = JsonSerializer.Serialize(usuario);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "usuario-alertas", routingKey: "usuario.alterado.alterado", basicProperties: null, body: body);
    }

}