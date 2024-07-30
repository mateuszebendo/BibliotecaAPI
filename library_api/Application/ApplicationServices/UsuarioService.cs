using library_api.Application.DTOs;
using library_api.Application.Entities;
using library_api.Application.Interfaces;
using library_api.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace library_api.Domain.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }
    
    public async Task<UsuarioDTO> CriaNovoUsuario(UsuarioDTO usuarioDto)
    {
        try
        {
            Usuario novoUsuario = new Usuario(usuarioDto);
            var usuarioCriado = await _usuarioRepository.PostUsuarioAsync(novoUsuario);
            if (usuarioCriado.Nome.IsNullOrEmpty())
            {
                throw new ApplicationException("Falha ao adicionar o registro de Usuario.");
            }  
            return new UsuarioDTO(usuarioCriado);
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }

    public async Task<List<UsuarioDTO>> RecuperaTodosUsuarios()
    {
        try
        {
            IEnumerable<Usuario> listaUsuarios = await _usuarioRepository.GetUsuarioAsync();
            if (listaUsuarios.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Nenhum Usuario encontrado");
            }

            List<UsuarioDTO> listaUsuariosDTO = new List<UsuarioDTO>();
            foreach (var usuario in listaUsuarios)
            {
                listaUsuariosDTO.Add(new UsuarioDTO(usuario));
            }

            return listaUsuariosDTO;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro inesperado.", error);
        }
    }
    
    public async Task<UsuarioDTO> RecuperaUsuarioPorId(int id)
    {
        try
        {
            UsuarioDTO usuarioResgatado = new UsuarioDTO(await _usuarioRepository.GetUsuarioByIdAsync(id));
            if (usuarioResgatado.Nome.IsNullOrEmpty())
            {
                throw new ApplicationException("Falha ao resgatar o Usuario do banco de dados.");
            }

            return usuarioResgatado;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
    
    public async Task<UsuarioDTO> AtualizaUsuario(UsuarioDTO usuarioDto, int id)
    {
        try
        {
            Usuario usuarioAtualizado = new Usuario(usuarioDto);
            var sucessoNaRequisicao = await _usuarioRepository.PutUsuarioAsync(usuarioAtualizado, id);
            if (sucessoNaRequisicao)
            {
                return new UsuarioDTO(await _usuarioRepository.GetUsuarioByIdAsync(id));
            } 
            throw new ApplicationException("Falha ao atualizar Usuario.");
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
    
    public async Task<UsuarioDTO> DesabilitaUsuarioPorId(int id)
    {
        try
        {
            var sucessoNaRequisicao = await _usuarioRepository.DeleteLogicoUsuarioByIdAsync(id);
            if (sucessoNaRequisicao)
            {
                UsuarioDTO usuarioDesabilitadoDto = new UsuarioDTO(await _usuarioRepository.GetUsuarioByIdAsync(id));
                return usuarioDesabilitadoDto;
            } 
            throw new ApplicationException("Falha ao desabilitar Usuario.");
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
}