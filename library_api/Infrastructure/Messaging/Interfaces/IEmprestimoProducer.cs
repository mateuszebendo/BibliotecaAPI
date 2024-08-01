using library_api.Application.DTOs;

namespace library_api.Domain.Messaging.Producers;

public interface IEmprestimoProducer
{
    void EnviaAvisoEmprestimoRealizadoComSucesso(EmprestimoDTO emprestimo);
    void EnviaAvisoEmprestimoFinalizado(EmprestimoDTO emprestimo);
}
