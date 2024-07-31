using library_api.Application.DTOs;

namespace library_api.Application.Interfaces;

public interface IUsuarioService
{
    void IniciarConsumo();
    Task<UsuarioDTO> CriaNovoUsuario(UsuarioDTO usuarioDto);
    Task<List<UsuarioDTO>> RecuperaTodosUsuarios();
    Task<UsuarioDTO> RecuperaUsuarioPorId(int id);
    Task<UsuarioDTO> AtualizaUsuario(UsuarioDTO usuarioDto, int id);
    Task<UsuarioDTO> DesabilitaUsuarioPorId(int id);
}