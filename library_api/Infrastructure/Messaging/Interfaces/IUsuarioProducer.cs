using library_api.Application.DTOs;
using RabbitMQ.Client;

namespace library_api.Domain.Messaging.Producers;

public interface IUsuarioProducer
{
    void EnviaAvisoUsuarioBloqueado(UsuarioDTO usuario);
    void EnviaAvisoUsuarioDesabilitado(UsuarioDTO usuario);
    void EnviaAvisoNovoUsuarioCriado(UsuarioDTO usuarioDto);
    void EnviaUsuarioAlterado(UsuarioDTO usuarioDto);
}
