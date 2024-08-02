using System.ComponentModel.DataAnnotations;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.Enums;

namespace BibliotecaAPI.Application.DTOs;

public class LivroDTO
{
    public LivroDTO() { }

    public LivroDTO(Livro livro)
    {
        LivroId = livro.LivroId;
        Nome = livro.Nome;
        Editora = livro.Editora;
        Autor = livro.Autor;
        Genero = livro.Genero;
        Disponibilidade = livro.Disponibilidade;
    }
    public LivroDTO(string nome, string editora, string autor, GeneroLivro genero, StatusLivro disponibilidade)
    {
        Nome = nome;
        Editora = editora;
        Autor = autor;
        Genero = genero;
        Disponibilidade = disponibilidade;
    }

    public int LivroId { get; set; }
    
    [Required(ErrorMessage = "Nome obrigatório")]
    public string Nome { get; set; }
    
    public string Editora { get; set; }
    public string Autor { get; set; }
    
    [Required]
    [EnumDataType(typeof(GeneroLivro))]
    public GeneroLivro Genero { get; set; }
    
    [Required]
    [EnumDataType(typeof(StatusLivro))]
    public StatusLivro Disponibilidade { get; set; }
}
