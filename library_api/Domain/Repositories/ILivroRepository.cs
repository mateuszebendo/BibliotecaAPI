using library_api.Application.Entities;

namespace library_api.Domain.Repositories;

public interface ILivroRepository
{
    
    Task<Livro> PostLivroAsync(Livro livro);
    Task<IEnumerable<Livro>> GetLivroAsync();
    Task<IEnumerable<Livro>> GetLivroAtivosAsync();
    Task<Livro> GetLivroByIdAsync(int id);
    Task<bool> PutLivroAsync(Livro livro, int id);
    Task<bool> DeleteLogicoLivroByIdAsync(int id);
}