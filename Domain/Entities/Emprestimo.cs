using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BibliotecaAPI.Domain.Enums;

namespace BibliotecaAPI.Application.Entities;

public class Emprestimo
{
    public Emprestimo(){}
    
    public Emprestimo(int emprestimoId, DateTime dataEmprestimo, DateTime dataDevolucao, StatusEmprestimo status, int usuarioId, int livroId)
    {
        EmprestimoId = emprestimoId;
        DataEmprestimo = dataEmprestimo;
        DataDevolucao = dataDevolucao;
        Status = status;
        UsuarioId = usuarioId;
        LivroId = livroId;
    }
    
    [Key]
    public int EmprestimoId { get; set; }
    
    [Required]
    public DateTime DataEmprestimo { get; set; }
    
    public DateTime DataDevolucao { get; set; }
    
    [Required]
    [EnumDataType(typeof(StatusEmprestimo))]
    public StatusEmprestimo Status { get; set; }
    
    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }
    
    public Usuario Usuario { get; set; }
    
    [ForeignKey("Livro")]
    public int LivroId { get; set; }
    
    public Livro Livro { get; set; }

}
