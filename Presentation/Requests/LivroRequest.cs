using BibliotecaAPI.Application.DTOs;
using BibliotecaAPI.Domain.Enums;

namespace BibliotecaAPI.Presentation.Requests;

using System.ComponentModel.DataAnnotations;

public record LivroRequest
{
    
    public LivroRequest() { }
    public LivroRequest(LivroDTO livroDto)
    {
        Nome = livroDto.Nome;
        Editora = livroDto.Editora;
        Genero = livroDto.Genero;
        Autor = livroDto.Autor;
        Disponibilidade = livroDto.Disponibilidade;
    }
    
    [Required(ErrorMessage = "Nome obrigatório")]
    [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres")]
    public string Nome { get; init; }

    [StringLength(255, ErrorMessage = "A editora deve ter no máximo 255 caracteres")]
    public string Editora { get; init; }

    [Required(ErrorMessage = "Gênero obrigatório")]
    public GeneroLivro Genero { get; init; }

    [StringLength(255, ErrorMessage = "O autor deve ter no máximo 255 caracteres")]
    public string Autor { get; init; }

    [Required(ErrorMessage = "Disponibilidade obrigatória")]
    public StatusLivro Disponibilidade { get; init; }
}
