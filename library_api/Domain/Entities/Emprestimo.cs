using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using library_api.Application.DTOs;
using library_api.Domain.Enums;

namespace library_api.Application.Entities;

public class Emprestimo
{
    public Emprestimo(){}

    public Emprestimo(EmprestimoDTO emprestimoDto)
    {
        EmprestimoId = emprestimoDto.EmprestimoId;
        DataEmprestimo = emprestimoDto.DataEmprestimo;
        DataDevolucao = emprestimoDto.DataDevolucao;
        Status = emprestimoDto.Status;
        UsuarioId = emprestimoDto.UsuarioId;
        LivroId = emprestimoDto.LivroId;
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
