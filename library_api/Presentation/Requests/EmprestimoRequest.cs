using System.ComponentModel.DataAnnotations;
using library_api.Application.DTOs;
using library_api.Domain.Enums;

namespace library_api.Presentation.Requests;

public class EmprestimoRequest
{
    public EmprestimoRequest() {}
    
    public DateTime DataDevolucao { get; init; }

    [Required(ErrorMessage = "Status obrigatório")]
    public StatusEmprestimo Status { get; init; }
    
    [Required(ErrorMessage = "ID do usuário obrigatório")]
    public int UsuarioId { get; init; }

    [Required(ErrorMessage = "ID do livro obrigatório")]
    public int LivroId { get; init; }
}