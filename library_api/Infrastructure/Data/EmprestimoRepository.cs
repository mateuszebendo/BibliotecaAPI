using Dapper;
using library_api.Application.Entities;
using library_api.Domain.Repositories;
using library_api.Infrastructure.DataBase;
using Npgsql;

namespace library_api.Infrastructure.Data;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly string _connectionString;
    
    public EmprestimoRepository()
    {
        _connectionString = DatabaseConfig.ConnectionString;
    }
    
    public async Task<Emprestimo> PostEmprestimoAsync(Emprestimo emprestimo)
    {
        try
        {
            string sqlQuery = @"INSERT INTO emprestimo (
                                    dataEmprestimo,
                                    dataDevolucao,
                                    status,
                                    usuarioId,
                                    livroId
                                )
                                VALUES (
                                    @dataEmprestimo,
                                    @dataDevolucao,
                                    'Ativo',
                                    @usuarioId,
                                    @livroId
                                )
                                RETURNING emprestimoId";


            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var emprestimoId = await connection.ExecuteScalarAsync<int>(sqlQuery, emprestimo);              
                return await GetEmprestimoByIdAsync(emprestimoId);
            }
        }
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }
    
    public async Task<IEnumerable<Emprestimo>> GetEmprestimosAsync()
    {
        try
        {
            string sqlQuery = @"SELECT * FROM emprestimo";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Emprestimo>(sqlQuery);
            }
        } 
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }
    
    public async Task<IEnumerable<Emprestimo>> GetEmprestimosAtivosAsync()
    {
        try
        {
            string sqlQuery = @"SELECT * FROM emprestimo WHERE status != 'Concluido'";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Emprestimo>(sqlQuery);
            }
        } 
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }
    
    public async Task<Emprestimo> GetEmprestimoByIdAsync(int id)
    {
        try
        {
            string sqlQuery = @"SELECT * FROM emprestimo WHERE EmprestimoId = @Id";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Emprestimo>(sqlQuery, new {Id = id});
            }
        } 
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }

    public async Task<bool> PutEmprestimoAsync(Emprestimo emprestimo, int id)
    {
        try
        {
            string sqlQuery = @"UPDATE emprestimo
                                SET dataEmprestimo = @dataEmprestimo,
                                    dataDevolucao = @dataDevolucao,
                                    status = @status,
                                    usuarioId = @usuarioId,
                                    livroId = @livroId
                                WHERE emprestimoId = @id";

            var parametros = new DynamicParameters();
            parametros.Add("dataEmprestimo", emprestimo.DataEmprestimo);
            parametros.Add("dataDevolucao", emprestimo.DataDevolucao);
            parametros.Add("status", emprestimo.Status.ToString());
            parametros.Add("usuarioId", emprestimo.UsuarioId);
            parametros.Add("livroId", emprestimo.LivroId);
            parametros.Add("id", id);

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(sqlQuery, parametros) > 0;
            }
            
        } catch (PostgresException error)
            {
                throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
            }
    }

    public async Task<bool> ConcluiEmprestimoByIdAsync(int id)
    {
        try
        {
            string sqlQuery = @"UPDATE emprestimo 
                                SET status = 'Concluido'
                                WHERE emprestimoId = @Id";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(sqlQuery, new {Id = id}) > 0;
            }
        } catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }
}