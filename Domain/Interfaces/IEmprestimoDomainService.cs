using BibliotecaAPI.Application.Entities;

namespace BibliotecaAPI.Domain.DomainInterfaces;

public interface IEmprestimoDomainService
{
    void CriaNovoEmprestimoLogica(IEnumerable<Emprestimo> listaEmprestimos, Emprestimo emprestimo, Livro livro, Usuario usuario);
    (Emprestimo, Usuario) ConcluiEmprestimoLogica(Emprestimo emprestimo, Usuario usuario);
}