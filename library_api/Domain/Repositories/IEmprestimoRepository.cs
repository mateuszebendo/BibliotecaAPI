using library_api.Application.Entities;

namespace library_api.Domain.Repositories;

public interface IEmprestimoRepository
{
    Task<Emprestimo> PostEmprestimoAsync(Emprestimo emprestimo);
    Task<IEnumerable<Emprestimo>> GetEmprestimosAsync();
    Task<IEnumerable<Emprestimo>> GetEmprestimosAtivosAsync();
    Task<Emprestimo> GetEmprestimoByIdAsync(int id);
    Task<bool> PutEmprestimoAsync(Emprestimo emprestimo, int id);
    Task<bool> ConcluiEmprestimoByIdAsync(int id);
}