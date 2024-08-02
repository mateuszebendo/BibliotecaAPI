using BibliotecaAPI.Application.DTOs;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Application.Interfaces;
using BibliotecaAPI.Domain.Messaging.Producers;
using BibliotecaAPI.Domain.Repositories;
using BibliotecaAPI.Infrastructure.Messaging.Producers;

namespace BibliotecaAPI.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUsuarioProducer _usuarioProducer;

    public UsuarioService(IUsuarioRepository usuarioRepository,
                          IUsuarioProducer usuarioProducer)
    {
        _usuarioRepository = usuarioRepository;
        _usuarioProducer = usuarioProducer;
    }
    public async Task<UsuarioDTO> CriaNovoUsuario(UsuarioDTO usuarioDto)
    {
        try
        {
            Usuario novoUsuario = new Usuario(usuarioDto.UsuarioId, 
                                                usuarioDto.Nome, 
                                                usuarioDto.Email, 
                                                usuarioDto.Senha, 
                                                usuarioDto.Telefone, 
                                                usuarioDto.Cargo, 
                                                usuarioDto.Status);             
            var usuarioCriado = await _usuarioRepository.PostUsuarioAsync(novoUsuario);
            if (usuarioCriado.Nome.Length == 0)
            {
                throw new ApplicationException("Falha ao adicionar o registro de Usuario.");
            }
            _usuarioProducer.EnviaAvisoNovoUsuarioCriado(usuarioCriado);
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
            if (listaUsuarios.LongCount() == 0)
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
            if (usuarioResgatado.Nome.Length == 0)
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
            Usuario usuarioAtualizado = new Usuario(id, 
                                                    usuarioDto.Nome, 
                                                    usuarioDto.Email, 
                                                    usuarioDto.Senha, 
                                                    usuarioDto.Telefone, 
                                                    usuarioDto.Cargo, 
                                                    usuarioDto.Status); 
            var sucessoNaRequisicao = await _usuarioRepository.PutUsuarioAsync(usuarioAtualizado, id);
            if (sucessoNaRequisicao)
            {
                var usuario = new UsuarioDTO(await _usuarioRepository.GetUsuarioByIdAsync(id));
                _usuarioProducer.EnviaUsuarioAlterado(usuarioAtualizado);
                return usuario;
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
                Usuario usuarioDesabilitado = await _usuarioRepository.GetUsuarioByIdAsync(id);
                UsuarioDTO usuarioDto = new UsuarioDTO();
                _usuarioProducer.EnviaAvisoUsuarioDesabilitado(usuarioDesabilitado);
                return usuarioDto;
            } 
            throw new ApplicationException("Falha ao desabilitar Usuario.");
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
}