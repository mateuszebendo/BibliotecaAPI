using library_api.Application.DTOs;
using library_api.Application.Interfaces;
using library_api.Domain.DomainInterfaces;
using library_api.Infrastructure.Messaging.Producers;

namespace library_api.Domain.Services;

public class LivroDomainService : ILivroDomainService
{
    private readonly ILivroService _livroService;
    private readonly LivroProducer _livroProducer;

    public LivroDomainService(ILivroService livroService, LivroProducer livroProducer)
    {
        _livroService = livroService;
        _livroProducer = livroProducer;
    }
    
    public async Task<LivroDTO> CriaNovoLivro(LivroDTO livroDto)
    {
        IEnumerable<LivroDTO> livrosExistentes = await _livroService.RecuperaTodosLivrosAtivos();

        bool existeLivro = false;
        foreach (var livro in livrosExistentes)
        {
            if (livro.nome == livroDto.nome)
            {
                existeLivro = true;
            }
        }

        if (!existeLivro)
        {
            _livroProducer.EnviaAvisoLivroLancado(livroDto.nome);
        }

        return await _livroService.CriaNovoLivro(livroDto);
    }
}