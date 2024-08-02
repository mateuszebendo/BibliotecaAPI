using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BibliotecaAPI.Domain.Enums;

namespace BibliotecaAPI.Application.Entities;

public class Usuario
{
    public Usuario() { }

    public Usuario(int usuarioId, string nome, string email, string senha, string telefone, CargoEnum cargo, StatusUsuario status)
    {
        UsuarioId = usuarioId;
        Nome = nome;
        Email = email;
        Senha = senha;
        Telefone = telefone;
        Cargo = cargo;
        Status = status;
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("usuarioId")]
    public int UsuarioId { get; set; }

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