using library_api.Application.DTOs;
using library_api.Application.Entities;
using library_api.Application.Interfaces;
using library_api.Domain.Enums;
using library_api.Domain.Repositories;
using library_api.Infrastructure.Messaging.Consumers;
using library_api.Infrastructure.Messaging.Producers;
using library_api.Presentation.Requests;
using Microsoft.IdentityModel.Tokens;

namespace library_api.Domain.Services;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _livroRepository;
    private readonly LivroProducer _livroProducer;
    private readonly LivroConsumer _livroConsumer;


    public LivroService(ILivroRepository livroRepository, LivroProducer livroProducer, LivroConsumer livroConsumer)
    {
        _livroRepository = livroRepository;
        _livroProducer = livroProducer;
        _livroConsumer = livroConsumer;
    }
    
    public void IniciarConsumo()
    {
        _livroConsumer.StartLivroConsuming();
    }

    public async Task<LivroDTO> CriaNovoLivro(LivroDTO livroDto)
    {
        try
        {
            Livro novoLivro = new Livro(livroDto.nome, livroDto.editora, livroDto.autor,
                livroDto.genero, StatusLivro.Disponivel);
            var livroCriado = await _livroRepository.PostLivroAsync(novoLivro);
            if (livroCriado.Nome.IsNullOrEmpty())
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
            if (listaLivros.IsNullOrEmpty())
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
            if (listaLivros.IsNullOrEmpty())
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
            if (livroResgatado.nome.IsNullOrEmpty())
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
            Livro livroAtualizado = new Livro(livroDto.nome, livroDto.editora, livroDto.autor,
                livroDto.genero, livroDto.disponibilidade);
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