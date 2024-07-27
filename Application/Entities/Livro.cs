using System.ComponentModel.DataAnnotations;

namespace library_api.Application.Entities;

public class Livro
{
    [Key]
    private int livro_id { get; set; }
    private string nome { get; set; }
    private string editora { get; set; }
    private string genero { get; set; }
    private string autor { get; set; }
}