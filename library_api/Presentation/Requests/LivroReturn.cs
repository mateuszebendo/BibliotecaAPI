using library_api.Application.DTOs;

namespace library_api.Presentation.Requests;

public record LivroReturn
{
    
    public LivroReturn() { }
    public LivroReturn(LivroDTO livroDto)
    {
        LivroId = livroDto.livroId;
        Nome = livroDto.nome;
        Editora = livroDto.editora;
        Genero = livroDto.genero.ToString();
        Autor = livroDto.autor;
        Disponibilidade = livroDto.disponibilidade.ToString();
    }
    
    public int LivroId { get; init; }
    
    public string Nome { get; init; }

    public string Editora { get; init; }

    public string Genero { get; init; }

    public string Autor { get; init; }
    
    public string Disponibilidade { get; init; }
}