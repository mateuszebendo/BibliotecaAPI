using library_api.Application.DTOs;
using library_api.Presentation.Models;

namespace library_api.Domain.Repositories;

public interface ILivroRepository
{
    
    Task<bool> postLivroAsync(LivroRequest request);
    Task<IEnumerable<LivroDTO>> getLivroAsync();
    Task<LivroDTO> getLivroByIdAsync(int id);
    Task<bool> putLivroAsync(LivroRequest request, int id);
    Task<bool> deletaLivroByIdAsync(int id);
}