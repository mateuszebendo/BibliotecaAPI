using library_api.Application.DTOs;
using library_api.Bus;
using library_api.Domain;
using library_api.Domain.Services;
using library_api.Presentation.Requests;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace library_api.presentation.controllers;

[ApiController]
[Route("emprestimo")]

public class EmprestimoController : ControllerBase
{
    private readonly EmprestimoService _emprestimoService;
    private readonly IBus _bus;

    public EmprestimoController(EmprestimoService emprestimoService, IBus bus)
    {
        _emprestimoService = emprestimoService;
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] EmprestimoRequest request)
    {
        // var eventRequest = new EmprestimoCadastroEvent(request.nome);

        // await _bus.Publish(eventRequest);
        
        if (request == null)
        {
            return BadRequest("Dados do emprestimo não podem ser nulos.");
        }

        EmprestimoDTO emprestimoDto = new EmprestimoDTO(request);


        var emprestimoCriado = await _emprestimoService.CriaNovoEmprestimo(emprestimoDto);

        if (emprestimoCriado == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar emprestimo.");
        }

        return CreatedAtAction(nameof(Post), new { id = emprestimoCriado.EmprestimoId }, new EmprestimoReturn(emprestimoCriado));
    }

    [HttpGet]   
    public async Task<IActionResult> Get()
    {
        List<EmprestimoReturn> listaEmprestimoRequest = new List<EmprestimoReturn>();

        foreach (var emprestimoDto in await _emprestimoService.RecuperaTodosEmprestimos())
        {
            listaEmprestimoRequest.Add(new EmprestimoReturn(emprestimoDto));
        }
        return Ok(listaEmprestimoRequest);
    }
    
    [HttpGet]  
    [Route("ativos")]
    public async Task<IActionResult> GetAllEmprestimoAtivos()
    {
        List<EmprestimoReturn> listaEmprestimoRequest = new List<EmprestimoReturn>();

        foreach (var emprestimoDto in await _emprestimoService.RecuperaTodosEmprestimosAtivos())
        {
            listaEmprestimoRequest.Add(new EmprestimoReturn(emprestimoDto));
        }
        return Ok(listaEmprestimoRequest);
    }
    
    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        EmprestimoReturn emprestimo = new EmprestimoReturn(await _emprestimoService.RecuperaEmprestimoPorId(id));
    
        return Ok(emprestimo);
    }
    
    [HttpPut("id")]
    public async Task<IActionResult> Put(int id, [FromBody] EmprestimoRequest request)
    {
        EmprestimoDTO emprestimoAtualizado = await _emprestimoService.AtualizaEmprestimo(new EmprestimoDTO(request), id);
        
        return Ok(new EmprestimoReturn(emprestimoAtualizado));
    }
    
    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int id)
    {
        EmprestimoReturn emprestimoDeletado = new EmprestimoReturn(await _emprestimoService.DesabilitaEmprestimoPorId(id));
    
        return Ok(emprestimoDeletado);
    }
}    