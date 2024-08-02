
using BibliotecaAPI.Application.Entities;

namespace BibliotecaAPI.Domain.Messaging.Producers;

public interface ILivroProducer
{
    void EnviaAvisoLivroDisponivel(Livro livro);
    void EnviaAvisoLivroLancado(string message);
}
