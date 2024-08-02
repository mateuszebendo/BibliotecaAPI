using BibliotecaAPI.Application.Entities;

namespace BibliotecaAPI.Domain.Messaging.Producers;

public interface IUsuarioProducer
{
    void EnviaAvisoUsuarioBloqueado(Usuario usuario);
    void EnviaAvisoUsuarioDesabilitado(Usuario usuario);
    void EnviaAvisoNovoUsuarioCriado(Usuario usuario);
    void EnviaUsuarioAlterado(Usuario usuario);
}
