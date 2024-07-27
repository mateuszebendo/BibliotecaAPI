namespace library_api.Application.DTOs;

public class LivroDTO
{
    public int livro_id { get; set; }
    public string nome { get; set; }
    public string editora { get; set; }
    public string genero { get; set; }
    public string autor { get; set; }
}