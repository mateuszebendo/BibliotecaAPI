using library_api.Application.DTOs;
using library_api.Bus;
using library_api.Domain;
using library_api.Domain.Services;
using library_api.Presentation.Requests;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace library_api.presentation.controllers;

[ApiController]
[Route("livros")]

public class LivroController : ControllerBase
{
    private readonly LivroService _livroService;
    private readonly IBus _bus;

    public LivroController(LivroService livroService, IBus bus)
    {
        _livroService = livroService;
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LivroRequest request)
    {
        // var eventRequest = new LivroCadastroEvent(request.nome);

        // await _bus.Publish(eventRequest);
        
        if (request == null)
        {
            return BadRequest("Dados do livro não podem ser nulos.");
        }

        LivroDTO livroDto = new LivroDTO(request);


        var livroCriado = await _livroService.CriaNovoLivro(livroDto);

        if (livroCriado == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar livro.");
        }

        return CreatedAtAction(nameof(Post), new { id = livroCriado.livroId }, new LivroReturn(livroCriado));
    }

    [HttpGet]   
    public async Task<IActionResult> Get()
    {
        List<LivroReturn> listaLivroRequest = new List<LivroReturn>();

        foreach (var livroDto in await _livroService.RecuperaTodosLivros())
        {
            listaLivroRequest.Add(new LivroReturn(livroDto));
        }
        return Ok(listaLivroRequest);
    }
    
    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        LivroReturn livro = new LivroReturn(await _livroService.RecuperaLivroPorId(id));
    
        return Ok(livro);
    }
    
    [HttpPut("id")]
    public async Task<IActionResult> Put(int id, [FromBody] LivroRequest request)
    {
        LivroDTO livroAtualizado = await _livroService.AtualizaLivro(new LivroDTO(request), id);
        
        return Ok(new LivroReturn(livroAtualizado));
    }
    
    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int id)
    {
        LivroReturn livroDeletado = new LivroReturn(await _livroService.DesabilitaLivroPorId(id));
    
        return Ok(livroDeletado);
    }
}    