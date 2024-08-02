using BibliotecaAPI.Application.DTOs;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Application.Interfaces;
using BibliotecaAPI.Domain.DomainInterfaces;
using BibliotecaAPI.Domain.Enums;
using BibliotecaAPI.Domain.Messaging.Producers;
using BibliotecaAPI.Domain.Repositories;
using BibliotecaAPI.Infrastructure.Messaging.Producers;

namespace BibliotecaAPI.Application.Services;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _livroRepository;
    private readonly ILivroDomainService _livroDomainService;
    private readonly ILivroProducer _livroProducer;


    public LivroService(ILivroRepository livroRepository, 
                        ILivroProducer livroProducer,
                        ILivroDomainService livroDomainService)
    {
        _livroRepository = livroRepository;
        _livroProducer = livroProducer;
        _livroDomainService = livroDomainService;
    }

    public async Task<LivroDTO> CriaNovoLivro(LivroDTO livroDto)
    {
        try
        {
            Livro novoLivro = new Livro(livroDto.Nome, livroDto.Editora, livroDto.Autor,
                livroDto.Genero, StatusLivro.Disponivel);
            IEnumerable<Livro> listaLivros = await _livroRepository.GetLivroAsync();

            if (!(_livroDomainService.VerificaSeJaLivroExiste(novoLivro, listaLivros)))
            {
                _livroProducer.EnviaAvisoLivroLancado(novoLivro.Nome);
            }
            var livroCriado = await _livroRepository.PostLivroAsync(novoLivro);
            if (livroCriado.Nome.Length == 0)
            {
                throw new ApplicationException("Falha ao adicionar o registro de livro.");
            }  
            return new LivroDTO(livroCriado);
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }

    public async Task<List<LivroDTO>> RecuperaTodosLivros()
    {
        try
        {
            IEnumerable<Livro> listaLivros = await _livroRepository.GetLivroAsync();
            if (listaLivros.LongCount() == 0)
            {
                throw new InvalidOperationException("Nenhum livro encontrado");
            }

            List<LivroDTO> listaLivrosDTO = new List<LivroDTO>();
            foreach (var livro in listaLivros)
            {
                listaLivrosDTO.Add(new LivroDTO(livro));
            }

            return listaLivrosDTO;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro inesperado.", error);
        }
    }
    
    public async Task<List<LivroDTO>> RecuperaTodosLivrosAtivos()
    {
        try
        {
            IEnumerable<Livro> listaLivros = await _livroRepository.GetLivroAsync();
            if (listaLivros.LongCount() == 0)
            {
                throw new InvalidOperationException("Nenhum livro encontrado");
            }

            List<LivroDTO> listaLivrosDTO = new List<LivroDTO>();
            foreach (var livro in listaLivros)
            {
                listaLivrosDTO.Add(new LivroDTO(livro));
            }

            return listaLivrosDTO;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro inesperado.", error);
        }
    }
    
    public async Task<LivroDTO> RecuperaLivroPorId(int id)
    {
        try
        {
            LivroDTO livroResgatado = new LivroDTO(await _livroRepository.GetLivroByIdAsync(id));
            if  (livroResgatado.Nome.Length == 0)
            {
                throw new ApplicationException("Falha ao resgatar o livro do banco de dados.");
            }

            return livroResgatado;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
    
    public async Task<LivroDTO> AtualizaLivro(LivroDTO livroDto, int id)
    {
        try
        {
            Livro livroAtualizado = new Livro(livroDto.Nome, livroDto.Editora, livroDto.Autor,
                livroDto.Genero, livroDto.Disponibilidade);
            var sucessoNaRequisicao = await _livroRepository.PutLivroAsync(livroAtualizado, id);
            if (sucessoNaRequisicao)
            {
                return new LivroDTO(await _livroRepository.GetLivroByIdAsync(id));
            } 
            throw new ApplicationException("Falha ao atualizar livro.");
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
    
    public async Task<LivroDTO> DesabilitaLivroPorId(int id)
    {
        try
        {
            var sucessoNaRequisicao = await _livroRepository.DeleteLogicoLivroByIdAsync(id);
            if (sucessoNaRequisicao)
            {
                LivroDTO livroDesabilitadoDto = new LivroDTO(await _livroRepository.GetLivroByIdAsync(id));
                return livroDesabilitadoDto;
            } 
            throw new ApplicationException("Falha ao desabilitar livro.");
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
}