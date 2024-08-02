using BibliotecaAPI.Application.DTOs;

namespace BibliotecaAPI.Application.Interfaces;

public interface ILivroService
{
    Task<LivroDTO> CriaNovoLivro(LivroDTO livroDto);
    Task<List<LivroDTO>> RecuperaTodosLivros();
    Task<List<LivroDTO>> RecuperaTodosLivrosAtivos();
    Task<LivroDTO> RecuperaLivroPorId(int id);
    Task<LivroDTO> AtualizaLivro(LivroDTO livroDto, int id);
    Task<LivroDTO> DesabilitaLivroPorId(int id);
}