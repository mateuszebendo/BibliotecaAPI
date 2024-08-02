using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.Enums;

namespace BibliotecaAPI.Application.DTOs;

public class UsuarioDTO
{
    public UsuarioDTO(){}
    public UsuarioDTO(Usuario usuario)
    {
        UsuarioId = usuario.UsuarioId;
        Nome = usuario.Nome;
        Email = usuario.Email;
        Telefone = usuario.Telefone;
        Cargo = usuario.Cargo;
        Senha = usuario.Senha;
        Status = usuario.Status;
    }
    
    public UsuarioDTO(string nome, string email, string telefone, CargoEnum cargo, string senha, StatusUsuario status)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cargo = cargo;
        Senha = senha;
        Status = status;
    }
    public int UsuarioId { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public CargoEnum Cargo { get; set; }
    public string Senha { get; set; }
    public StatusUsuario Status { get; set; }
}
