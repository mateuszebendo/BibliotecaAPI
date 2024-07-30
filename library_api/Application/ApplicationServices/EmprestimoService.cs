using library_api.Application.DTOs;
using library_api.Application.Entities;
using library_api.Application.Interfaces;
using library_api.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace library_api.Domain.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository _emprestimoRepository;

    public EmprestimoService(IEmprestimoRepository emprestimoRepository)
    {
        _emprestimoRepository = emprestimoRepository;
    }

    public async Task<EmprestimoDTO> CriaNovoEmprestimo(EmprestimoDTO emprestimoDto)
    {
        try
        {
            Emprestimo novoEmprestimo = new Emprestimo(emprestimoDto);
            var emprestimoCriado = await _emprestimoRepository.PostEmprestimoAsync(novoEmprestimo);
            if (emprestimoCriado.DataEmprestimo.ToString().IsNullOrEmpty())
            {
                throw new ApplicationException("Falha ao adicionar o registro de emprestimo.");
            }  
            return new EmprestimoDTO(emprestimoCriado);
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }

    public async Task<List<EmprestimoDTO>> RecuperaTodosEmprestimos()
    {
        try
        {
            IEnumerable<Emprestimo> listaEmprestimos = await _emprestimoRepository.GetEmprestimosAsync();
            if (listaEmprestimos.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Nenhum emprestimo encontrado");
            }

            List<EmprestimoDTO> listaEmprestimosDTO = new List<EmprestimoDTO>();
            foreach (var emprestimo in listaEmprestimos)
            {
                listaEmprestimosDTO.Add(new EmprestimoDTO(emprestimo));
            }

            return listaEmprestimosDTO;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro inesperado.", error);
        }
    }
    
    public async Task<List<EmprestimoDTO>> RecuperaTodosEmprestimosAtivos()
    {
        try
        {
            IEnumerable<Emprestimo> listaEmprestimos = await _emprestimoRepository.GetEmprestimosAtivosAsync();

            List<EmprestimoDTO> listaEmprestimosDTO = new List<EmprestimoDTO>();
            foreach (var emprestimo in listaEmprestimos)
            {
                listaEmprestimosDTO.Add(new EmprestimoDTO(emprestimo));
            }

            return listaEmprestimosDTO;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro inesperado.", error);
        }
    }
    
    public async Task<EmprestimoDTO> RecuperaEmprestimoPorId(int id)
    {
        try
        {
            EmprestimoDTO emprestimoResgatado = new EmprestimoDTO(await _emprestimoRepository.GetEmprestimoByIdAsync(id));
            if (emprestimoResgatado.DataEmprestimo.ToString().IsNullOrEmpty())
            {
                throw new ApplicationException("Falha ao resgatar o emprestimo do banco de dados.");
            }

            return emprestimoResgatado;
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
    
    public async Task<EmprestimoDTO> AtualizaEmprestimo(EmprestimoDTO emprestimoDto, int id)
    {
        try
        {
            Emprestimo emprestimoAtualizado = new Emprestimo(emprestimoDto);
            var sucessoNaRequisicao = await _emprestimoRepository.PutEmprestimoAsync(emprestimoAtualizado, id);
            if (sucessoNaRequisicao)
            {
                return new EmprestimoDTO(await _emprestimoRepository.GetEmprestimoByIdAsync(id));
            } 
            throw new ApplicationException("Falha ao atualizar emprestimo.");
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
    
    public async Task<EmprestimoDTO> DesabilitaEmprestimoPorId(int id)
    {
        try
        {
            var sucessoNaRequisicao = await _emprestimoRepository.ConcluiEmprestimoByIdAsync(id);
            if (sucessoNaRequisicao)
            {
                EmprestimoDTO emprestimoDesabilitadoDto = new EmprestimoDTO(await _emprestimoRepository.GetEmprestimoByIdAsync(id));
                return emprestimoDesabilitadoDto;
            } 
            throw new ApplicationException("Falha ao desabilitar emprestimo.");
        } catch (ArgumentException error)
        {
            throw new ApplicationException("Ocorreu um erro com os dados fornecidos.", error);
        }
    }
}