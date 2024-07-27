using Dapper;
using library_api.Application.DTOs;
using library_api.Domain.Repositories;
using library_api.Infrastructure.DataBase;
using library_api.Presentation.Models;
using Microsoft.Data.SqlClient;
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
    
    public Task<LivroDTO> getLivroByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> putLivroAsync(LivroRequest request, int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> deletaLivroByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}