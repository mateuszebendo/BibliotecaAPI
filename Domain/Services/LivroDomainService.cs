using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.DomainInterfaces;

namespace BibliotecaAPI.Domain.Services;

public class LivroDomainService : ILivroDomainService
{
    public bool VerificaSeJaLivroExiste(Livro livro, IEnumerable<Livro> livrosExistentes )
    {
        bool existeLivro = false;
        foreach (var livroNaLista in livrosExistentes)
        {
            if (livroNaLista.Nome == livro.Nome)
            {
                existeLivro = true;
            }
        }

        return existeLivro;
    }
}