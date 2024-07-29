using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using library_api.Application.DTOs;
using library_api.Domain.Enums;

namespace library_api.Application.Entities;

public class Usuario
{
    public Usuario() { }

    public Usuario(UsuarioDTO usuarioDTO)
    {
        Id = usuarioDTO.UsuarioId;
        Nome = usuarioDTO.Nome;
        Email = usuarioDTO.Email;
        Senha = usuarioDTO.Senha;
        Telefone = usuarioDTO.Telefone;
        Cargo = usuarioDTO.Cargo;
        Status = usuarioDTO.Status;
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("usuario_id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome obrigatório")]
    [Column("nome")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Email obrigatório")]
    [RegularExpression(@"^[a-zA-Z0-9+&-]+(?:\.[a-zA-Z0-9_+&-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Email inválido")]
    [Column("email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Telefone obrigatório")]
    [RegularExpression(@"\(\d{2}\) \d{5}-\d{4}", ErrorMessage = "Formato de telefone inválido")]
    [Column("telefone")]
    public string Telefone { get; set; }

    [Required]
    [Column("cargo")]
    public CargoEnum Cargo { get; set; }

    [Required(ErrorMessage = "Senha obrigatória")]
    [Column("senha")]
    public string Senha { get; set; }

    [Required]
    [Column("status")]
    public StatusUsuario Status { get; set; }
}