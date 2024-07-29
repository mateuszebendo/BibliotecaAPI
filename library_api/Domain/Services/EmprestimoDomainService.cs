using library_api.Application.DTOs;
using library_api.Application.Entities;
using library_api.Domain.Enums;

namespace library_api.Domain.Services;

public class EmprestimoDomainService
{
    private readonly EmprestimoService _emprestimoService;
    private readonly UsuarioService _usuarioService;
    private readonly LivroService _livroService;

    public EmprestimoDomainService(EmprestimoService emprestimoService, UsuarioService usuarioService, LivroService livroService)
    {
        _emprestimoService = emprestimoService;
        _usuarioService = usuarioService;
        _livroService = livroService;
    }

    public async Task<EmprestimoDTO> CriaNovoEmprestimo(EmprestimoDTO emprestimoDto)
    {
        try
        { 
            List<EmprestimoDTO> emprestimoDtos = await _emprestimoService.RecuperaTodosEmprestimosAtivos();
            UsuarioDTO usuarioDto = await _usuarioService.RecuperaUsuarioPorId(emprestimoDto.UsuarioId);
            LivroDTO livroDto = await _livroService.RecuperaLivroPorId(emprestimoDto.LivroId);
            
            int numeroDeEmprestimos = 0;
            foreach (var emprestimo in emprestimoDtos)
            {
                if (emprestimo.UsuarioId == emprestimoDto.UsuarioId)
                {
                    numeroDeEmprestimos++;
                }
            }

            if (numeroDeEmprestimos >= 3)
            {
                throw new Exception("Limite de empréstimos antigido pelo usuário!");
            }

            if (usuarioDto.Status == StatusUsuario.Bloqueado)
            {
                throw new Exception("Usuário bloqueado!");
            }

            if (livroDto.disponibilidade != StatusLivro.Disponivel)
            {
                throw new Exception("Livro não disponível");
            }

            livroDto.disponibilidade = StatusLivro.Emprestado;
            await _livroService.AtualizaLivro(livroDto, emprestimoDto.LivroId);

            return await _emprestimoService.CriaNovoEmprestimo(emprestimoDto);

        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
}