using System.ComponentModel.DataAnnotations;
using library_api.Domain.Enums;

namespace library_api.Application.Entities;

using System.ComponentModel.DataAnnotations;

public sealed class Livro
{
    public Livro(){}
    public Livro(string nome, string editora, string autor, GeneroLivro genero)
    {
        Nome = nome;
        Editora = editora;
        Autor = autor;
        Genero = genero;
        Disponibilidade = StatusLivro.Disponivel;
    }
    public Livro(int livroId, string nome, string editora, string autor, GeneroLivro genero)
    {
        LivroId = livroId;
        Nome = nome;
        Editora = editora;
        Autor = autor;
        Genero = genero;
        Disponibilidade = StatusLivro.Disponivel;
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
