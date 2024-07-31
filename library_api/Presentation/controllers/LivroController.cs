using library_api.Application.DTOs;
using library_api.Application.Interfaces;
using library_api.Domain.DomainInterfaces;
using library_api.Domain.Services;
using library_api.Presentation.Requests;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace library_api.presentation.controllers;

[ApiController]
[Route("livros")]

public class LivroController : ControllerBase
{
    private readonly ILivroService _livroService;
    private readonly ILivroDomainService _livroDomainService;

    public LivroController(ILivroService livroService, ILivroDomainService livroDomainService)
    {
        _livroService = livroService;
        _livroDomainService = livroDomainService;
    }

    [HttpPost("cria-livro")]
    public async Task<IActionResult> Post([FromBody] LivroRequest request)
    {
        
        if (request == null)
        {
            return BadRequest("Dados do livro não podem ser nulos.");
        }

        LivroDTO livroDto = new LivroDTO(request);


        var livroCriado = await _livroDomainService.CriaNovoLivro(livroDto);

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
        LivroDTO livroAtualizado = await _livroService.AtualizaLivro(new LivroDTO(request), id);
        
        return Ok(new LivroReturn(livroAtualizado));
    }
    
    [HttpDelete("desabilita-livro/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        LivroReturn livroDeletado = new LivroReturn(await _livroService.DesabilitaLivroPorId(id));
    
        return Ok(livroDeletado);
    }
}    