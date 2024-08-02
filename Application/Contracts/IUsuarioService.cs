using BibliotecaAPI.Application.DTOs;

namespace BibliotecaAPI.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDTO> CriaNovoUsuario(UsuarioDTO usuarioDto);
    Task<List<UsuarioDTO>> RecuperaTodosUsuarios();
    Task<UsuarioDTO> RecuperaUsuarioPorId(int id);
    Task<UsuarioDTO> AtualizaUsuario(UsuarioDTO usuarioDto, int id);
    Task<UsuarioDTO> DesabilitaUsuarioPorId(int id);
}