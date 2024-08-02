using BibliotecaAPI.Application.Entities;

namespace BibliotecaAPI.Domain.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario> PostUsuarioAsync(Usuario usuario);
    Task<IEnumerable<Usuario>> GetUsuarioAsync();
    Task<Usuario> GetUsuarioByIdAsync(int id);
    Task<bool> PutUsuarioAsync(Usuario usuario, int id);
    Task<bool> DeleteLogicoUsuarioByIdAsync(int id);
}