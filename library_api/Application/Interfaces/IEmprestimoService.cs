using library_api.Application.DTOs;

namespace library_api.Application.Interfaces;

public interface IEmprestimoService
{
    Task<EmprestimoDTO> CriaNovoEmprestimo(EmprestimoDTO emprestimoDto);
    Task<List<EmprestimoDTO>> RecuperaTodosEmprestimos();
    Task<List<EmprestimoDTO>> RecuperaTodosEmprestimosAtivos();
    Task<EmprestimoDTO> RecuperaEmprestimoPorId(int id);
    Task<EmprestimoDTO> AtualizaEmprestimo(EmprestimoDTO emprestimoDto, int id);
    Task<EmprestimoDTO> DesabilitaEmprestimoPorId(int id);
}