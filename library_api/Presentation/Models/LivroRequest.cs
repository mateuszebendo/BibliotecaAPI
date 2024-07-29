namespace library_api.Presentation.Models;

public model LivroRequest
{
    public string nome { get; set; }
    public string editora { get; set; }
    public string genero { get; set; }
    public string autor { get; set; }
    public bool disponivel { get; set; }
}