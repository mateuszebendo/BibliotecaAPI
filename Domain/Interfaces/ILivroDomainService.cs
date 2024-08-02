using BibliotecaAPI.Application.Entities;

namespace BibliotecaAPI.Domain.DomainInterfaces;

public interface ILivroDomainService
{
    bool VerificaSeJaLivroExiste(Livro livroDto, IEnumerable<Livro> livrosExistentes );
}