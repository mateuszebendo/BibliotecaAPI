using Dapper;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.Repositories;
using BibliotecaAPI.Infrastructure.DataBase;
using Npgsql;

namespace BibliotecaAPI.Infrastructure.Data;

public class LivroRepository : ILivroRepository
{
    private readonly string _connectionString;
    
    public LivroRepository()
    {
        _connectionString = DatabaseConfig.ConnectionString;
    }
    
    public async Task<Livro> PostLivroAsync(Livro livro)
    {
        try
        {
            string sqlQuery =
                @"INSERT INTO livro(nome, editora, genero, autor, disponibilidade) 
                    VALUES (@nome, @editora, @genero, @autor, @disponibilidade)
                    RETURNING livroID";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var livroId = await connection.ExecuteScalarAsync<int>(sqlQuery, livro);              
                return await GetLivroByIdAsync(livroId);
            }
        }
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }
    
    public async Task<IEnumerable<Livro>> GetLivroAsync()
    {
        try
        {
            string sqlQuery = @"SELECT * FROM livro";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Livro>(sqlQuery);
            }
        } 
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }
    
    public async Task<IEnumerable<Livro>> GetLivroAtivosAsync()
    {
        try
        {
            string sqlQuery = @"SELECT * FROM livro WHERE disponibilidade != 'Arquivado'";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Livro>(sqlQuery);
            }
        } 
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }
    
    public async Task<Livro> GetLivroByIdAsync(int id)
    {
        try
        {
            string sqlQuery = @"SELECT * FROM livro WHERE livroId = @Id";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Livro>(sqlQuery, new {Id = id});
            }
        } 
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }

    public async Task<bool> PutLivroAsync(Livro livro, int id)
    {
        try
        {
            string sqlQuery = @"UPDATE livro
                            SET nome=@nome, editora=@editora, genero=@genero, autor=@autor, disponibilidade=@disponibilidade
                            WHERE livroId=@Id";
            
            var parametros = new DynamicParameters();
            parametros.Add("nome", livro.Nome);
            parametros.Add("editora", livro.Editora);
            parametros.Add("genero", livro.Genero.ToString());
            parametros.Add("autor", livro.Autor);
            parametros.Add("disponibilidade", livro.Disponibilidade.ToString());
            parametros.Add("Id", id);

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(sqlQuery, parametros) > 0;
            }
            
        } catch (PostgresException error)
            {
                throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
            }
    }

    public async Task<bool> DeleteLogicoLivroByIdAsync(int id)
    {
        try
        {
            string sqlQuery = @"UPDATE livro 
                            SET disponibilidade = 'Arquivado'
                            WHERE livroId = @Id";

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