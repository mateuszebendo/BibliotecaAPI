using library_api.Application.DTOs;

namespace library_api.Presentation.Requests;

public record UsuarioReturn
{
    public UsuarioReturn() { }

    public UsuarioReturn(UsuarioDTO usuarioDto)
    {
        Nome = usuarioDto.Nome;
        Email = usuarioDto.Email;
        Telefone = usuarioDto.Telefone;
        Cargo = usuarioDto.Cargo.ToString();
    }

    public string Nome { get; init; }

    public string Email { get; init; }

    public string Telefone { get; init; }

    public string Cargo { get; init; }

}