using library_api.Application.DTOs;
using library_api.Application.Interfaces;
using library_api.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace library_api.presentation.controllers;

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
        
        if (request == null)
        {
            return BadRequest("Dados do Usuario não podem ser nulos.");
        }

        UsuarioDTO usuarioDto = new UsuarioDTO(request);


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
        UsuarioDTO usuarioAtualizado = await _usuarioService.AtualizaUsuario(new UsuarioDTO(request), id);
        
        return Ok(new UsuarioReturn(usuarioAtualizado));
    }
    
    [HttpDelete("arquiva-usuario/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        UsuarioReturn usuarioDeletado = new UsuarioReturn(await _usuarioService.DesabilitaUsuarioPorId(id));
    
        return Ok(usuarioDeletado);
    }
}