using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.DomainInterfaces;
using BibliotecaAPI.Domain.Enums;

namespace BibliotecaAPI.Domain.Services;

public class EmprestimoDomainService : IEmprestimoDomainService
{
    public void CriaNovoEmprestimoLogica(IEnumerable<Emprestimo> listaEmprestimos, Emprestimo emprestimo, Livro livro, Usuario usuario)
    {
        int numeroDeEmprestimos = 0;
        foreach (var emprestimoDaLista in listaEmprestimos)
        {
            if (emprestimoDaLista.UsuarioId == emprestimo.UsuarioId)
            {
                numeroDeEmprestimos++;
            }
        }
        
        if (numeroDeEmprestimos >= 3)
        {
            throw new Exception("Limite de empréstimos antigido pelo usuário!");
        }
        
        if (usuario.Status != StatusUsuario.Ativo)
        {
            throw new Exception($"Usuário {usuario.Status.ToString()}!");
        }
        
        if (livro.Disponibilidade != StatusLivro.Disponivel)
        {
            throw new Exception("Livro não disponível");
        }
    }

    public (Emprestimo, Usuario) ConcluiEmprestimoLogica(Emprestimo emprestimo, Usuario usuario)
    {
        if (DateTime.Compare(emprestimo.DataDevolucao, DateTime.Today) < 0)
        {
            usuario.Status = StatusUsuario.Bloqueado;
            emprestimo.Status = StatusEmprestimo.Atrasado;
            return (emprestimo, usuario);
        }
        
        emprestimo.Status = StatusEmprestimo.Concluido;

        return (emprestimo, usuario);
    }
}