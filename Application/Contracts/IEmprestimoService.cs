using BibliotecaAPI.Application.DTOs;

namespace BibliotecaAPI.Application.Interfaces;

public interface IEmprestimoService
{
    Task<EmprestimoDTO> CriaNovoEmprestimo(EmprestimoDTO emprestimoDto);
    Task<List<EmprestimoDTO>> RecuperaTodosEmprestimos();
    Task<List<EmprestimoDTO>> RecuperaTodosEmprestimosAtivos();
    Task<EmprestimoDTO> RecuperaEmprestimoPorId(int id);
    Task<EmprestimoDTO> AtualizaEmprestimo(EmprestimoDTO emprestimoDto, int id);
    Task<bool> ConcluiEmprestimo(int id);
    Task<EmprestimoDTO> DesabilitaEmprestimoPorId(int id);
}