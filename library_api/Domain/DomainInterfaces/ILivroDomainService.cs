using library_api.Application.DTOs;

namespace library_api.Domain.DomainInterfaces;

public interface ILivroDomainService
{
    Task<LivroDTO> CriaNovoLivro(LivroDTO livroDto);
}