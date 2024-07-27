using library_api.Bus;
using library_api.Domain;
using library_api.Domain.Repositories;
using library_api.Presentation.Models;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace library_api.presentation.controllers;

[ApiController]
[Route("livros")]

public class LivroController : ControllerBase
{
    private readonly ILivroRepository _livroRepository;
    private readonly IBus _bus;

    public LivroController(ILivroRepository livroRepository, IBus bus)
    {
        _livroRepository = livroRepository;
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LivroRequest request)
    {
        if (string.IsNullOrEmpty(request.nome))
        {
            return BadRequest("Preencha o campo nome!");
        }

        var eventRequest = new LivroCadastroEvent(request.nome);

        await _bus.Publish(eventRequest);

        var foiAdicionado = await _livroRepository.postLivroAsync(request);

        return foiAdicionado ? Ok("Livro adicionado com sucesso!") : BadRequest("Erro ao adicionar livro");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var livros = await _livroRepository.getLivroAsync();

        return livros.Any() ? Ok(livros) : NoContent();
    }
    
    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        var livro = await _livroRepository.getLivroByIdAsync(id);

        return livro != null ? Ok(livro) : NotFound("Livro não encontrado");
    }
    
    [HttpPut("id")]
    public async Task<IActionResult> Put(int id, [FromBody] LivroRequest request)
    {
        var livro = await _livroRepository.putLivroAsync(request, id);

        return livro ? Ok("Livro atualizado com sucesso!") : NotFound("Livro não encontrado");
    }
    
    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int id)
    {
        var livro = await _livroRepository.deletaLivroByIdAsync(id);

        return livro ? Ok("Livro deletado com sucesso") : NotFound("Livro não encontrado");
    }
}    