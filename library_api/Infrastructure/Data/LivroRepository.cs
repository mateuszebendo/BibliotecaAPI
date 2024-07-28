using Dapper;
using library_api.Application.DTOs;
using library_api.Domain.Repositories;
using library_api.Infrastructure.DataBase;
using library_api.Presentation.Models;
using Npgsql;

namespace library_api.Infrastructure.Data;

public class LivroRepository : ILivroRepository
{
    private readonly string _connectionString;
    
    public LivroRepository()
    {
        _connectionString = DatabaseConfig.ConnectionString;
    }
    
    public async Task<bool> postLivroAsync(LivroRequest request)
    {
        string sqlQuery = @"insert into livro(nome, editora, genero, autor) values (@nome, @editora, @genero, @autor)";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.ExecuteAsync(sqlQuery, request) > 0;
        }
    }
    
    public async Task<IEnumerable<LivroDTO>> getLivroAsync()
    {
        string sqlQuery = @"select * from livro";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<LivroDTO>(sqlQuery);
        }
    }
    
    public async Task<LivroDTO> getLivroByIdAsync(int id)
    {
        string sqlQuery = @"select * from livro where livro_id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<LivroDTO>(sqlQuery, new {Id = id});
        }
    }

    public async Task<bool> putLivroAsync(LivroRequest request, int id)
    {
        string sqlQuery = @"update livro
                            set nome=@nome, editora=@editora, genero=@genero, autor=@autor
                            where livro_id=@Id";
        var parametros = new DynamicParameters();
        parametros.Add("nome", request.nome);
        parametros.Add("editora", request.editora);
        parametros.Add("genero", request.genero);
        parametros.Add("autor", request.autor);
        parametros.Add("Id", id);
        
        using(var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.ExecuteAsync(sqlQuery, parametros) > 0;
        }
    }

    public async Task<bool> deletaLivroByIdAsync(int id)
    {
        string sqlQuery = @"delete from livro where livro_id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.ExecuteAsync(sqlQuery, new {Id = id}) > 0;
        }
    }
}