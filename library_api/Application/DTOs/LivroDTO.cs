using System.ComponentModel.DataAnnotations;
using library_api.Application.Entities;
using library_api.Domain.Enums;
using library_api.Presentation.Requests;

namespace library_api.Application.DTOs;

public class LivroDTO
{
    public LivroDTO(Livro livro)
    {
        livroId = livro.LivroId;
        nome = livro.Nome;
        editora = livro.Editora;
        autor = livro.Autor;
        genero = livro.Genero;
        disponibilidade = livro.Disponibilidade;
    }

    public LivroDTO(LivroRequest livro)
    {
        nome = livro.Nome;
        editora = livro.Editora;
        autor = livro.Autor;
        genero = livro.Genero;
        disponibilidade = livro.Disponibilidade;
    }

    public int livroId { get; set; }
    
    [Required(ErrorMessage = "Nome obrigatório")]
    public string nome { get; set; }
    
    public string editora { get; set; }
    public string autor { get; set; }
    
    [Required]
    [EnumDataType(typeof(GeneroLivro))]
    public GeneroLivro genero { get; set; }
    
    [Required]
    [EnumDataType(typeof(StatusLivro))]
    public StatusLivro disponibilidade { get; set; }
}