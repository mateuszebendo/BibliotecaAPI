using BibliotecaAPI.Application.DTOs;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Application.Interfaces;
using BibliotecaAPI.Domain.DomainInterfaces;
using BibliotecaAPI.Domain.Enums;
using BibliotecaAPI.Domain.Messaging.Producers;
using BibliotecaAPI.Domain.Repositories;

namespace BibliotecaAPI.Application.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository _emprestimoRepository;
    private readonly ILivroRepository _livroRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmprestimoProducer _emprestimoProducer;
    private readonly ILivroProducer _livroProducer;
    private readonly IUsuarioProducer _usuarioProducer;
    private readonly IEmprestimoDomainService _emprestimoService;

    public EmprestimoService(IEmprestimoRepository emprestimoRepository, 
                            ILivroRepository livroRepository,
                            IUsuarioRepository usuarioRepository,
                            ILivroProducer livroProducer, 
                            IEmprestimoProducer emprestimoProducer, 
                            IUsuarioProducer usuarioProducer,
                            IEmprestimoDomainService emprestimoService)
    {
        _emprestimoRepository = emprestimoRepository;
        _livroRepository = livroRepository;
        _usuarioRepository = usuarioRepository;
        _emprestimoProducer = emprestimoProducer;
        _emprestimoService = emprestimoService;
        _livroProducer = livroProducer;
        _usuarioProducer = usuarioProducer;
    }

    public async Task<EmprestimoDTO> CriaNovoEmprestimo(EmprestimoDTO emprestimoDto)
    {
        try
        {
            if (emprestimoDto.DataEmprestimo.ToString().Length == 0)
            {
                throw new ApplicationException("Falha ao adicionar o registro de emprestimo.");
            } 
            
            Emprestimo novoEmprestimo = new Emprestimo(emprestimoDto.EmprestimoId, emprestimoDto.DataEmprestimo, emprestimoDto.DataDevolucao, emprestimoDto.Status, emprestimoDto.UsuarioId, emprestimoDto.LivroId);
            IEnumerable<Emprestimo> listaEmprestimo = await _emprestimoRepository.GetEmprestimosAtivosAsync();
            var (usuario, livro) = await _emprestimoRepository.GetUsuarioAndLivroByIdAsync(emprestimoDto.UsuarioId, emprestimoDto.LivroId); 
            _emprestimoService.CriaNovoEmprestimoLogica(listaEmprestimo, novoEmprestimo, livro, usuario);

            livro.Disponibilidade = StatusLivro.Emprestado;
            _livroRepository.PutLivroAsync(livro, livro.LivroId);
            
            _emprestimoProducer.EnviaAvisoEmprestimoRealizadoComSucesso(novoEmprestimo);
            novoEmprestimo = await _emprestimoRepository.PostEmprestimoAsync(novoEmprestimo);
            
            return new EmprestimoDTO(novoEmprestimo);
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }

    public async Task<List<EmprestimoDTO>> RecuperaTodosEmprestimos()
    {
        try
        {
            IEnumerable<Emprestimo> listaEmprestimos = await _emprestimoRepository.GetEmprestimosAsync();
            if (listaEmprestimos.Count() == 0)
            {
                throw new InvalidOperationException("Nenhum emprestimo encontrado");
            }

            List<EmprestimoDTO> listaEmprestimosDto = new List<EmprestimoDTO>();
            foreach (var emprestimo in listaEmprestimos)
            {
                listaEmprestimosDto.Add(new EmprestimoDTO(emprestimo));
            }

            return listaEmprestimosDto;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro inesperado.", error);
        }
    }
    
    public async Task<List<EmprestimoDTO>> RecuperaTodosEmprestimosAtivos()
    {
        try
        {
            IEnumerable<Emprestimo> listaEmprestimos = await _emprestimoRepository.GetEmprestimosAtivosAsync();

            List<EmprestimoDTO> listaEmprestimosDTO = new List<EmprestimoDTO>();
            foreach (var emprestimo in listaEmprestimos)
            {
                listaEmprestimosDTO.Add(new EmprestimoDTO(emprestimo));
            }

            return listaEmprestimosDTO;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro inesperado.", error);
        }
    }
    
    public async Task<EmprestimoDTO> RecuperaEmprestimoPorId(int id)
    {
        try
        {
            EmprestimoDTO emprestimoResgatado = new EmprestimoDTO(await _emprestimoRepository.GetEmprestimoByIdAsync(id));
            if (emprestimoResgatado.DataEmprestimo.ToString().Length == 0)
            {
                throw new ApplicationException("Falha ao resgatar o emprestimo do banco de dados.");
            }

            return emprestimoResgatado;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
    
    public async Task<EmprestimoDTO> AtualizaEmprestimo(EmprestimoDTO emprestimoDto, int id)
    {
        try
        {
            Emprestimo emprestimoAtualizado = new Emprestimo(emprestimoDto.EmprestimoId, emprestimoDto.DataEmprestimo, emprestimoDto.DataDevolucao, emprestimoDto.Status, emprestimoDto.UsuarioId, emprestimoDto.LivroId);
            var sucessoNaRequisicao = await _emprestimoRepository.PutEmprestimoAsync(emprestimoAtualizado, id);
            if (sucessoNaRequisicao)
            {
                return new EmprestimoDTO(await _emprestimoRepository.GetEmprestimoByIdAsync(id));
            } 
            throw new ApplicationException("Falha ao atualizar emprestimo.");
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
    
    public async Task<bool> ConcluiEmprestimo(int id)
    {
        try
        {
            Emprestimo emprestimoAntigo = await _emprestimoRepository.GetEmprestimoByIdAsync(id);
            var (usuario, livro) =
                await _emprestimoRepository.GetUsuarioAndLivroByIdAsync(emprestimoAntigo.UsuarioId,
                    emprestimoAntigo.LivroId);

            livro.Disponibilidade = StatusLivro.Disponivel;
            await _livroRepository.PutLivroAsync(livro, livro.LivroId);
            _livroProducer.EnviaAvisoLivroDisponivel(livro);

            (emprestimoAntigo, usuario) = _emprestimoService.ConcluiEmprestimoLogica(emprestimoAntigo, usuario);

            if(usuario.Status == StatusUsuario.Bloqueado)
            {
                await _usuarioRepository.PutUsuarioAsync(usuario, emprestimoAntigo.UsuarioId);
                _usuarioProducer.EnviaAvisoUsuarioBloqueado(usuario);
            }
            
            emprestimoAntigo.Status = StatusEmprestimo.Atrasado;
            _emprestimoProducer.EnviaAvisoEmprestimoFinalizado(emprestimoAntigo);
            await _emprestimoRepository.PutEmprestimoAsync(emprestimoAntigo, emprestimoAntigo.EmprestimoId);
            return true;
        } catch (ArgumentException error)
        {
            Console.Write("Ocorreu um erro com os dados fornecidos.\n" + error);
            return false;
        }
    }    
    
    public async Task<EmprestimoDTO> DesabilitaEmprestimoPorId(int id)
    {
        try
        {
            var sucessoNaRequisicao = await _emprestimoRepository.ConcluiEmprestimoByIdAsync(id);
            if (sucessoNaRequisicao)
            {
                EmprestimoDTO emprestimoDesabilitadoDto = new EmprestimoDTO(await _emprestimoRepository.GetEmprestimoByIdAsync(id));
                return emprestimoDesabilitadoDto;
            } 
            throw new ApplicationException("Falha ao desabilitar emprestimo.");
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
}