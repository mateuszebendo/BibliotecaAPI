using library_api.Application.DTOs;

namespace library_api.Presentation.Requests;

public class EmprestimoReturn
{
    public EmprestimoReturn() { }

    public EmprestimoReturn(EmprestimoDTO emprestimoDto)
    {
        EmprestimoId = emprestimoDto.EmprestimoId;
        DataEmprestimo = emprestimoDto.DataEmprestimo;
        DataDevolucao = emprestimoDto.DataDevolucao;
        Status = emprestimoDto.Status.ToString();
        ValorMulta = emprestimoDto.ValorMulta;
        UsuarioId = emprestimoDto.UsuarioId;
        LivroId = emprestimoDto.LivroId;
    }

    public int EmprestimoId { get; init; }

    public DateTime DataEmprestimo { get; init; }

    public DateTime? DataDevolucao { get; init; }

    public string Status { get; init; }

    public int? ValorMulta { get; init; }

    public int UsuarioId { get; init; }

    public int LivroId { get; init; }
}
