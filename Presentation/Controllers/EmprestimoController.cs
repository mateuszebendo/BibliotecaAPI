using BibliotecaAPI.Application.DTOs;
using BibliotecaAPI.Application.Interfaces;
using BibliotecaAPI.Domain.DomainInterfaces;
using BibliotecaAPI.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.presentation.controllers;

[ApiController]
[Route("emprestimo")]

public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;

    public EmprestimoController(IEmprestimoService emprestimoService)
    {
        _emprestimoService = emprestimoService;
    }

    [HttpPost("cria-emprestimo")]
    public async Task<IActionResult> Post([FromBody] EmprestimoRequest request)
    {
        EmprestimoDTO emprestimoDto = new EmprestimoDTO(request.DataDevolucao, request.UsuarioId, request.LivroId);

        var emprestimoCriado = await _emprestimoService.CriaNovoEmprestimo(emprestimoDto);

        if (emprestimoCriado == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar emprestimo.");
        }

        return Ok(new EmprestimoReturn(emprestimoCriado));
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
        EmprestimoDTO emprestimoDto = new EmprestimoDTO(request.DataDevolucao, request.UsuarioId, request.LivroId);
        EmprestimoDTO emprestimoAtualizado = await _emprestimoService.AtualizaEmprestimo(emprestimoDto, id);
        
        return Ok(new EmprestimoReturn(emprestimoAtualizado));
    }
    
    [HttpPut("conclui-emprestimo/{id}")]
    public async Task<IActionResult> ConcluiEmprestimo(int id)
    {
        var resultado = await _emprestimoService.ConcluiEmprestimo(id);
        
        return resultado ? Ok("Emprestimo concluido com sucesso") : BadRequest("Não foi possível concluir o emprestimo!");
    }
    
    [HttpDelete("desabilita-emprestimo")]
    public async Task<IActionResult> Delete(int id)
    {
        EmprestimoReturn emprestimoDeletado = new EmprestimoReturn(await _emprestimoService.DesabilitaEmprestimoPorId(id));
    
        return Ok(emprestimoDeletado);
    }
}    