
using BibliotecaAPI.Application.Entities;

namespace BibliotecaAPI.Domain.Messaging.Producers;

public interface IEmprestimoProducer
{
    void EnviaAvisoEmprestimoRealizadoComSucesso(Emprestimo emprestimo);
    void EnviaAvisoEmprestimoFinalizado(Emprestimo emprestimo);
}
