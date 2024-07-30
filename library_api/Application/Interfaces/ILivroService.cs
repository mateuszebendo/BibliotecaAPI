using library_api.Application.DTOs;

namespace library_api.Application.Interfaces;

public interface ILivroService
{
    void IniciarConsumo();
    Task<LivroDTO> CriaNovoLivro(LivroDTO livroDto);
    Task<List<LivroDTO>> RecuperaTodosLivros();
    Task<LivroDTO> RecuperaLivroPorId(int id);
    Task<LivroDTO> AtualizaLivro(LivroDTO livroDto, int id);
    Task<LivroDTO> DesabilitaLivroPorId(int id);
}