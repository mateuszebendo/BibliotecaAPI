using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.Enums;

namespace BibliotecaAPI.Application.DTOs;

public class EmprestimoDTO
{
    
    public EmprestimoDTO(){}

    public EmprestimoDTO(DateTime dataDevolucao, int usuarioId, int livroId)
    {
        DataEmprestimo = DateTime.Today;
        DataDevolucao = dataDevolucao;
        Status = StatusEmprestimo.Ativo;
        UsuarioId = usuarioId;
        LivroId = livroId;
    }
    
    public EmprestimoDTO(int emprestimoId, DateTime dataEmprestimo, DateTime dataDevolucao, StatusEmprestimo status, int usuarioId, int livroId)
    {
        EmprestimoId = emprestimoId;
        DataEmprestimo = dataEmprestimo;
        DataDevolucao = dataDevolucao;
        Status = status;
        UsuarioId = usuarioId;
        LivroId = livroId;
    }
    public EmprestimoDTO(Emprestimo emprestimo)
    {
        EmprestimoId = emprestimo.EmprestimoId;
        DataEmprestimo = emprestimo.DataEmprestimo;
        DataDevolucao = emprestimo.DataDevolucao;
        Status = emprestimo.Status;
        UsuarioId = emprestimo.UsuarioId;
        LivroId = emprestimo.LivroId;
    }

    public int EmprestimoId { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataDevolucao { get; set; }
    public StatusEmprestimo Status { get; set; }
    public int UsuarioId { get; set; }
    public int LivroId { get; set; }
}