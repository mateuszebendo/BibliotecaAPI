using library_api.Application.DTOs;
using library_api.Application.Interfaces;
using library_api.Domain.DomainInterfaces;
using library_api.Domain.Services;
using library_api.Presentation.Requests;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace library_api.presentation.controllers;

[ApiController]
[Route("emprestimo")]

public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;
    private readonly IEmprestimoDomainService _emprestimoDomainService;

    public EmprestimoController(IEmprestimoService emprestimoService, IEmprestimoDomainService emprestimoDomainService)
    {
        _emprestimoService = emprestimoService;
        _emprestimoDomainService = emprestimoDomainService;
    }

    [HttpPost("cria-emprestimo")]
    public async Task<IActionResult> Post([FromBody] EmprestimoRequest request)
    {
        // var eventRequest = new EmprestimoCadastroEvent(request.nome);

        // await _bus.Publish(eventRequest);
        
        if (request == null)
        {
            return BadRequest("Dados do emprestimo não podem ser nulos.");
        }

        EmprestimoDTO emprestimoDto = new EmprestimoDTO(request);


        var emprestimoCriado = await _emprestimoDomainService.CriaNovoEmprestimo(emprestimoDto);

        if (emprestimoCriado == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar emprestimo.");
        }

        return CreatedAtAction(nameof(Post), new { id = emprestimoCriado.EmprestimoId }, new EmprestimoReturn(emprestimoCriado));
    }

    [HttpGet("recupera-emprestimos")]   
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
    [Route("recupera-emprestimos-ativos")]
    public async Task<IActionResult> GetAllEmprestimoAtivos()
    {
        List<EmprestimoReturn> listaEmprestimoRequest = new List<EmprestimoReturn>();

        foreach (var emprestimoDto in await _emprestimoService.RecuperaTodosEmprestimosAtivos())
        {
            listaEmprestimoRequest.Add(new EmprestimoReturn(emprestimoDto));
        }
        return Ok(listaEmprestimoRequest);
    }
    
    [HttpGet("recupera-emprestimo/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        EmprestimoReturn emprestimo = new EmprestimoReturn(await _emprestimoService.RecuperaEmprestimoPorId(id));
    
        return Ok(emprestimo);
    }
    
    [HttpPut("atualiza-emprestimo")]
    public async Task<IActionResult> Put(int id, [FromBody] EmprestimoRequest request)
    {
        EmprestimoDTO emprestimoAtualizado = await _emprestimoService.AtualizaEmprestimo(new EmprestimoDTO(request), id);
        
        return Ok(new EmprestimoReturn(emprestimoAtualizado));
    }
    
    [HttpPut("conclui-emprestimo/{id}")]
    public async Task<IActionResult> ConcluiEmprestimo(int id)
    {
        var resultado = await _emprestimoDomainService.ConcluiEmprestimo(id);
        
        return resultado ? Ok("Emprestimo concluido com sucesso") : BadRequest("Não foi possível concluir o emprestimo!");
    }
    
    [HttpDelete("desabilita-emprestimo")]
    public async Task<IActionResult> Delete(int id)
    {
        EmprestimoReturn emprestimoDeletado = new EmprestimoReturn(await _emprestimoService.DesabilitaEmprestimoPorId(id));
    
        return Ok(emprestimoDeletado);
    }
}    