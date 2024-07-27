using library_api.Domain.Repositories;
using library_api.Presentation.Models;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace library_api.presentation.controllers;
[ApiController]
[Route("[controller]")]

public class LivroController : ControllerBase
{
    private readonly ILivroRepository _livroRepository;
    
    public LivroController(ILivroRepository livroRepository)
    {
        _livroRepository = livroRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post(LivroRequest request)
    {
        if (string.IsNullOrEmpty(request.nome))
        {
            return BadRequest("Preencha o campo nome!");
        }

        var foiAdicionado = await _livroRepository.postLivroAsync(request);

        return foiAdicionado ? 
            Ok("Livro adicionado com sucesso!") : 
            BadRequest("Erro ao adicionar livro");
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var livros = await _livroRepository.getLivroAsync();

        return livros.Any() ? Ok(livros) : NoContent();
    }
    // public static void AddApiEndPoints(this WebApplication app)
    // {
    //     app.MapPost("solicitar-relatorio/", async (string name, IBus bus) =>
    //     {
    //         var solicitacao = new SolicitacaoRelatorio()
    //         {
    //             Id = Guid.NewGuid(),
    //             Nome = name, 
    //             Status = "PENDENTE",
    //             ProcessedTime = null
    //         };
    //
    //         var eventRequest = new RelatorioSolicitadoEvent(solicitacao.Id, solicitacao.Nome);
    //         
    //         Lista.Relatorios.Add(solicitacao);
    //
    //         await bus.Publish(eventRequest);
    //
    //         return Results.Ok(solicitacao);
    //     });
    //
    //     app.MapGet("get-relatorio", () => Lista.Relatorios);
    // }
}