using BibliotecaAPI.Application.DTOs;

namespace BibliotecaAPI.Presentation.Requests;

public record UsuarioReturn
{
    public UsuarioReturn() { }

    public UsuarioReturn(UsuarioDTO usuarioDto)
    {
        UsuarioId = usuarioDto.UsuarioId;
        Nome = usuarioDto.Nome;
        Email = usuarioDto.Email;
        Telefone = usuarioDto.Telefone;
        Cargo = usuarioDto.Cargo.ToString();
        Status = usuarioDto.Status.ToString();
    }

    public int UsuarioId { get; init; }
    public string Nome { get; init; }

    public string Email { get; init; }

    public string Telefone { get; init; }

    public string Cargo { get; init; }

    public string Status { get; init; }

}