using library_api.Application.Entities;
using library_api.Domain.Enums;
using library_api.Presentation.Requests;

namespace library_api.Application.DTOs;

public class EmprestimoDTO
{
    public EmprestimoDTO(Emprestimo emprestimo)
    {
        EmprestimoId = emprestimo.EmprestimoId;
        DataEmprestimo = emprestimo.DataEmprestimo;
        DataDevolucao = emprestimo.DataDevolucao;
        Status = emprestimo.Status;
        UsuarioId = emprestimo.UsuarioId;
        LivroId = emprestimo.LivroId;
    }

    public EmprestimoDTO(EmprestimoRequest emprestimoRequest)
    {
        DataEmprestimo = DateTime.Today;
        DataDevolucao = emprestimoRequest.DataDevolucao;
        Status = emprestimoRequest.Status;
        UsuarioId = emprestimoRequest.UsuarioId;
        LivroId = emprestimoRequest.LivroId;
    }
    public int EmprestimoId { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataDevolucao { get; set; }
    public StatusEmprestimo Status { get; set; }
    public int UsuarioId { get; set; }
    public int LivroId { get; set; }
}