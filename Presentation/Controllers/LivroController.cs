using BibliotecaAPI.Application.DTOs;
using BibliotecaAPI.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.presentation.controllers;

[ApiController]
[Route("livros")]

public class LivroController : ControllerBase
{
    private readonly Application.Interfaces.ILivroService _livroService;

    public LivroController( Application.Interfaces.ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpPost("cria-livro")]
    public async Task<IActionResult> Post([FromBody] LivroRequest request)
    {
        LivroDTO livroDto = new LivroDTO(request.Nome, request.Editora, request.Autor, request.Genero, request.Disponibilidade);
        
        var livroCriado = await _livroService.CriaNovoLivro(livroDto);

        if (livroCriado == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar livro.");
        }

        LivroReturn livroRetornado = new LivroReturn(livroCriado);

        return Ok(livroRetornado);
    }

    [HttpGet("recupera-livros")]   
    public async Task<IActionResult> Get()
    {
        List<LivroReturn> listaLivroRequest = new List<LivroReturn>();

        foreach (var livroDto in await _livroService.RecuperaTodosLivros())
        {
            listaLivroRequest.Add(new LivroReturn(livroDto));
        }
        return Ok(listaLivroRequest);
    }
    
    [HttpGet("recupera-livros-ativos")]   
    public async Task<IActionResult> GetLivrosAtivos()
    {
        List<LivroReturn> listaLivroRequest = new List<LivroReturn>();

        foreach (var livroDto in await _livroService.RecuperaTodosLivrosAtivos())
        {
            listaLivroRequest.Add(new LivroReturn(livroDto));
        }
        return Ok(listaLivroRequest);
    }
    
    [HttpGet("recupera-livro/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        LivroDTO livroResgatado = await _livroService.RecuperaLivroPorId(id);
        LivroReturn livro = new LivroReturn(livroResgatado);
    
        return Ok(livro);
    }
    
    [HttpPut("atualiza-livro/{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] LivroRequest request)
    {
        LivroDTO livroDto = new LivroDTO(request.Nome, request.Editora, request.Autor, request.Genero, request.Disponibilidade);
        LivroDTO livroAtualizado = await _livroService.AtualizaLivro(livroDto, id);
        
        return Ok(new LivroReturn(livroAtualizado));
    }
    
    [HttpDelete("desabilita-livro/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        LivroReturn livroDeletado = new LivroReturn(await _livroService.DesabilitaLivroPorId(id));
    
        return Ok(livroDeletado);
    }
}    