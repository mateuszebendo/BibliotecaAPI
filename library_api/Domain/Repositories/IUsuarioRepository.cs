using library_api.Application.Entities;

namespace library_api.Domain.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario> PostUsuarioAsync(Usuario usuario);
    Task<IEnumerable<Usuario>> GetUsuarioAsync();
    Task<Usuario> GetUsuarioByIdAsync(int id);
    Task<bool> PutUsuarioAsync(Usuario usuario, int id);
    Task<bool> DeleteLogicoUsuarioByIdAsync(int id);
}