using library_api.Application.Entities;
using library_api.Domain.Enums;
using library_api.Presentation.Requests;

namespace library_api.Application.DTOs;

public class UsuarioDTO
{
    public UsuarioDTO(Usuario usuario)
    {
        UsuarioId = usuario.Id;
        Nome = usuario.Nome;
        Email = usuario.Email;
        Telefone = usuario.Telefone;
        Cargo = usuario.Cargo;
        Senha = usuario.Senha;
        Status = usuario.Status;
    }
    
    public UsuarioDTO(UsuarioRequest usuarioRequest)
    {
        Nome = usuarioRequest.Nome;
        Email = usuarioRequest.Email;
        Telefone = usuarioRequest.Telefone;
        Cargo = usuarioRequest.Cargo;
        Senha = usuarioRequest.Senha;
        Status = usuarioRequest.Status;
    }
    public int UsuarioId { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public CargoEnum Cargo { get; set; }
    public string Senha { get; set; }
    public StatusUsuario Status { get; set; }
}
