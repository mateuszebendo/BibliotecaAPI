using library_api.Application.DTOs;

namespace library_api.Domain.Messaging.Producers;

public interface ILivroProducer
{
    void EnviaAvisoLivroDisponivel(LivroDTO livroDto);
    void EnviaAvisoLivroLancado(string message);
}
