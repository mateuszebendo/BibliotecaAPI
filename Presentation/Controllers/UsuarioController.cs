using BibliotecaAPI.Application.DTOs;
using BibliotecaAPI.Application.Interfaces;
using BibliotecaAPI.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.presentation.controllers;

[ApiController]
[Route("usuario")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("cria-usuario")]
    public async Task<IActionResult> Post([FromBody] UsuarioRequest request)
    {
        UsuarioDTO usuarioDto = new UsuarioDTO(request.Nome, request.Email, request.Telefone, request.Cargo, request.Senha, request.Status);
        
        var usuarioCriado = await _usuarioService.CriaNovoUsuario(usuarioDto);

        if (usuarioCriado == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar Usuario.");
        }

        return CreatedAtAction(nameof(Post), new { id = usuarioCriado.UsuarioId }, new UsuarioReturn(usuarioCriado));
    }

    [HttpGet("recupera-usuarios")]   
    public async Task<IActionResult> Get()
    {
        List<UsuarioReturn> listaUsuarioRequest = new List<UsuarioReturn>();

        foreach (var usuarioDto in await _usuarioService.RecuperaTodosUsuarios())
        {
            listaUsuarioRequest.Add(new UsuarioReturn(usuarioDto));
        }
        return Ok(listaUsuarioRequest);
    }
    
    [HttpGet("recupera-usuario/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        UsuarioReturn usuario = new UsuarioReturn(await _usuarioService.RecuperaUsuarioPorId(id));
    
        return Ok(usuario);
    }
    
    [HttpPut("atualiza-usuario/{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UsuarioRequest request)
    {
        UsuarioDTO usuarioDto = new UsuarioDTO(request.Nome, request.Email, request.Telefone, request.Cargo, request.Senha, request.Status);
        UsuarioDTO usuarioAtualizado = await _usuarioService.AtualizaUsuario(usuarioDto, id);
        
        return Ok(new UsuarioReturn(usuarioAtualizado));
    }
    
    [HttpDelete("arquiva-usuario/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        UsuarioReturn usuarioDeletado = new UsuarioReturn(await _usuarioService.DesabilitaUsuarioPorId(id));
    
        return Ok(usuarioDeletado);
    }
}