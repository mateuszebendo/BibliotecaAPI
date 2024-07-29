namespace library_api.Presentation.Requests;

using library_api.Application.DTOs;
using library_api.Domain.Enums;
using System.ComponentModel.DataAnnotations;

public record UsuarioRequest
{
    public UsuarioRequest() { }

    public UsuarioRequest(UsuarioDTO usuarioDto)
    {
        Nome = usuarioDto.Nome;
        Email = usuarioDto.Email;
        Telefone = usuarioDto.Telefone;
        Cargo = usuarioDto.Cargo;
        Senha = usuarioDto.Senha;
        Status = usuarioDto.Status;
    }

    [Required(ErrorMessage = "Nome obrigatório")]
    [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
    public string Nome { get; init; }

    [Required(ErrorMessage = "Email obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(255, ErrorMessage = "O email deve ter no máximo 255 caracteres")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Telefone obrigatório")]
    [RegularExpression(@"\(\d{2}\) \d{5}-\d{4}", ErrorMessage = "Formato de telefone inválido")]
    [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres")]
    public string Telefone { get; init; }

    [Required(ErrorMessage = "Cargo obrigatório")]
    public CargoEnum Cargo { get; init; }

    [Required(ErrorMessage = "Senha obrigatória")]
    [StringLength(255, ErrorMessage = "A senha deve ter no máximo 255 caracteres")]
    public string Senha { get; init; }

    [Required(ErrorMessage = "Status obrigatório")]
    public StatusUsuario Status { get; init; }
}
