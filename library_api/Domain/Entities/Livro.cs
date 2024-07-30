using System.ComponentModel.DataAnnotations;
using library_api.Application.DTOs;
using library_api.Domain.Enums;

namespace library_api.Application.Entities;

using System.ComponentModel.DataAnnotations;

public sealed class Livro
{
    public Livro(){}
    public Livro(string nome, string editora, string autor, GeneroLivro genero, StatusLivro disponibilidade)
    {
        Nome = nome;
        Editora = editora;
        Autor = autor;
        Genero = genero;
        Disponibilidade = disponibilidade;
    }
    public Livro(int livroId, string nome, string editora, string autor, GeneroLivro genero, StatusLivro disponibilidade)
    {
        LivroId = livroId;
        Nome = nome;
        Editora = editora;
        Autor = autor;
        Genero = genero;
        Disponibilidade = disponibilidade;
    }
    
    public Livro(LivroDTO livroDto)
    {
        LivroId = livroDto.livroId;
        Nome = livroDto.nome;
        Editora = livroDto.editora;
        Autor = livroDto.autor;
        Genero = livroDto.genero;
        Disponibilidade = livroDto.disponibilidade;
    }


    [Key]
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
