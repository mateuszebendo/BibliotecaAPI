using library_api.Application.DTOs;
using library_api.Domain.Enums;

namespace library_api.Presentation.Requests;

using System.ComponentModel.DataAnnotations;

public record LivroRequest
{
    
    public LivroRequest() { }
    public LivroRequest(LivroDTO livroDto)
    {
        Nome = livroDto.nome;
        Editora = livroDto.editora;
        Genero = livroDto.genero;
        Autor = livroDto.autor;
        Disponibilidade = livroDto.disponibilidade;
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
