using System.Text;
using System.Text.Json;
using library_api.Application.DTOs;
using RabbitMQ.Client;

namespace library_api.Infrastructure.Messaging.Producers;

public class UsuarioProducer
{
    private readonly IModel _channel;

    public UsuarioProducer(IModel channel)
    {
        _channel = channel;
    }

    public void EnviaAvisoUsuarioBloqueado(UsuarioDTO usuario)
    {
        var json = JsonSerializer.Serialize(usuario);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "usuario-alertas", routingKey: "usuario.alterado.bloqueado", basicProperties: null, body: body);
    }
    
    public void  EnviaAvisoUsuarioDesabilitado(UsuarioDTO usuario)
    {
        var json = JsonSerializer.Serialize(usuario);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "usuario-alertas", routingKey: "usuario.alterado.desabilitado", basicProperties: null, body: body);
    }

    public void EnviaAvisoNovoUsuarioCriado(UsuarioDTO usuarioDto)
    {
        var json = JsonSerializer.Serialize(usuarioDto);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "usuario-alertas", routingKey: "usuario.criado", basicProperties: null, body: body);
    }
    
    public void EnviaUsuarioAlterado(UsuarioDTO usuarioDto)
    {
        var json = JsonSerializer.Serialize(usuarioDto);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "usuario-alertas", routingKey: "usuario.alterado.alterado", basicProperties: null, body: body);
    }
}