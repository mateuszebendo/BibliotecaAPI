using library_api.Application.DTOs;

namespace library_api.Domain.DomainInterfaces;

public interface IEmprestimoDomainService
{
    Task<EmprestimoDTO> CriaNovoEmprestimo(EmprestimoDTO emprestimoDto);
    void IniciarConsumo();
    Task<bool> ConcluiEmprestimo(int id);
}