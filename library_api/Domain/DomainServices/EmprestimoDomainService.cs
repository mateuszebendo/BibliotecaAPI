using library_api.Application.DTOs;
using library_api.Application.Entities;
using library_api.Application.Interfaces;
using library_api.Domain.DomainInterfaces;
using library_api.Domain.Enums;
using library_api.Domain.Repositories;
using library_api.Infrastructure.Messaging.Consumers;
using library_api.Infrastructure.Messaging.Producers;

namespace library_api.Domain.Services;

public class EmprestimoDomainService : IEmprestimoDomainService
{
    private readonly IEmprestimoService _emprestimoService;
    private readonly IUsuarioService _usuarioService;
    private readonly ILivroService _livroService;
    private readonly EmprestimoProducer _emprestimoProducer;
    private readonly EmprestimoConsumer _emprestimoConsumer;
    private readonly UsuarioProducer _usuarioProducer;
    private readonly UsuarioConsumer _usuarioConsumer;
    private readonly LivroProducer _livroProducer;
    private readonly LivroConsumer _livroConsumer;
    private readonly AdminConsumer _adminConsumer;

    public EmprestimoDomainService(IEmprestimoService emprestimoService, 
                                    IUsuarioService usuarioService, 
                                    ILivroService livroService,
                                    EmprestimoProducer emprestimoProducer,
                                    EmprestimoConsumer emprestimoConsumer,
                                    UsuarioProducer usuarioProducer, 
                                    UsuarioConsumer usuarioConsumer,
                                    LivroProducer livroProducer, 
                                    LivroConsumer livroConsumer,
                                    AdminConsumer adminConsumer)
    {
        _emprestimoService = emprestimoService;
        _usuarioService = usuarioService;
        _livroService = livroService;
        _emprestimoProducer = emprestimoProducer;
        _emprestimoConsumer = emprestimoConsumer;
        _usuarioProducer = usuarioProducer;
        _usuarioConsumer = usuarioConsumer;
        _livroProducer = livroProducer;
        _livroConsumer = livroConsumer;
        _adminConsumer = adminConsumer;
    }

    public void IniciarConsumo()
    {
        _emprestimoConsumer.StartEmprestimoConsuming();
        _usuarioConsumer.StartUsuarioConsuming();
        _livroConsumer.StartLivroConsuming();
        _adminConsumer.StartAdminConsuming();
    }

    public async Task<EmprestimoDTO> CriaNovoEmprestimo(EmprestimoDTO emprestimoDto)
    {
        try
        { 
            List<EmprestimoDTO> emprestimos = await _emprestimoService.RecuperaTodosEmprestimosAtivos();
            UsuarioDTO usuario = await _usuarioService.RecuperaUsuarioPorId(emprestimoDto.UsuarioId);
            LivroDTO livro = await _livroService.RecuperaLivroPorId(emprestimoDto.LivroId);
            
            int numeroDeEmprestimos = 0;
            foreach (var emprestimo in emprestimos)
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

            if (usuario.Status != StatusUsuario.Ativo)
            {
                throw new Exception($"Usuário {usuario.Status.ToString()}!");
            }

            if (livro.disponibilidade != StatusLivro.Disponivel)
            {
                throw new Exception("Livro não disponível");
            }

            livro.disponibilidade = StatusLivro.Emprestado;
            await _livroService.AtualizaLivro(livro, emprestimoDto.LivroId);

            _emprestimoProducer.EnviaAvisoEmprestimoRealizadoComSucesso(emprestimoDto);
            return await _emprestimoService.CriaNovoEmprestimo(emprestimoDto);

        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }

    public async Task<bool> ConcluiEmprestimo(int id)
    {
        try
        {
            EmprestimoDTO emprestimoAntigo = await _emprestimoService.RecuperaEmprestimoPorId(id);

            LivroDTO livro = await _livroService.RecuperaLivroPorId(emprestimoAntigo.LivroId);
            livro.disponibilidade = StatusLivro.Disponivel;
            await _livroService.AtualizaLivro(livro, livro.livroId);
            _livroProducer.EnviaAvisoLivroDisponivel(livro);
            

            if (DateTime.Compare(emprestimoAntigo.DataDevolucao, DateTime.Today) < 0)
            {
                UsuarioDTO usuario = await _usuarioService.RecuperaUsuarioPorId(emprestimoAntigo.UsuarioId);
                usuario.Status = StatusUsuario.Bloqueado;
                await _usuarioService.AtualizaUsuario(usuario, emprestimoAntigo.UsuarioId);
                _usuarioProducer.EnviaAvisoUsuarioBloqueado(usuario);
                emprestimoAntigo.Status = StatusEmprestimo.Atrasado;
                _emprestimoProducer.EnviaAvisoEmprestimoFinalizado(emprestimoAntigo);
                await _emprestimoService.AtualizaEmprestimo(emprestimoAntigo, emprestimoAntigo.EmprestimoId);
                return true;
            }

            emprestimoAntigo.Status = StatusEmprestimo.Concluido;
            await _emprestimoService.AtualizaEmprestimo(emprestimoAntigo, emprestimoAntigo.EmprestimoId);
            
            _emprestimoProducer.EnviaAvisoEmprestimoFinalizado(emprestimoAntigo);
            return true;
        } catch (ArgumentException error)
        {
            Console.Write("Ocorreu um erro com os dados fornecidos.\n" + error);
            return false;
        }
    }
}