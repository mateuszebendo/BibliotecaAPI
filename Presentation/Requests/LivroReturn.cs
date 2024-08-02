using BibliotecaAPI.Application.DTOs;

namespace BibliotecaAPI.Presentation.Requests;

public record LivroReturn
{
    
    public LivroReturn() { }
    public LivroReturn(LivroDTO livroDto)
    {
        LivroId = livroDto.LivroId;
        Nome = livroDto.Nome;
        Editora = livroDto.Editora;
        Genero = livroDto.Genero.ToString();
        Autor = livroDto.Autor;
        Disponibilidade = livroDto.Disponibilidade.ToString();
    }
    
    public int LivroId { get; init; }
    
    public string Nome { get; init; }

    public string Editora { get; init; }

    public string Genero { get; init; }

    public string Autor { get; init; }
    
    public string Disponibilidade { get; init; }
}