using BibliotecaAPI.Application.Entities;

namespace BibliotecaAPI.Domain.Repositories;

public interface IEmprestimoRepository
{
    Task<Emprestimo> PostEmprestimoAsync(Emprestimo emprestimo);
    Task<IEnumerable<Emprestimo>> GetEmprestimosAsync();
    Task<IEnumerable<Emprestimo>> GetEmprestimosAtivosAsync();
    Task<Emprestimo> GetEmprestimoByIdAsync(int id);
    Task<(Usuario usuario, Livro livro)> GetUsuarioAndLivroByIdAsync(int usuarioId, int livroId);
    Task<bool> PutEmprestimoAsync(Emprestimo emprestimo, int id);
    Task<bool> ConcluiEmprestimoByIdAsync(int id);
}