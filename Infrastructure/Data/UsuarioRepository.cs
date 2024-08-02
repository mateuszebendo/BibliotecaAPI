using Dapper;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.Repositories;
using BibliotecaAPI.Infrastructure.DataBase;
using Npgsql;

namespace BibliotecaAPI.Infrastructure.Data;

public class UsuarioRepository: IUsuarioRepository
{
    private readonly string _connectionString;
    
    public UsuarioRepository()
    {
        _connectionString = DatabaseConfig.ConnectionString;
    }
    
    public async Task<Usuario> PostUsuarioAsync(Usuario usuario)
    {
        try
        {
            string sqlQuery =
                @"INSERT INTO usuarios(nome, email, telefone, cargo, senha, status)
                    VALUES (@nome, @email, @telefone, @cargo, @senha, @status)
                    RETURNING usuarioId";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var usuarioId = await connection.ExecuteScalarAsync<int>(sqlQuery, usuario);              
                return await GetUsuarioByIdAsync(usuarioId);
            }
        }
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }
    
    public async Task<IEnumerable<Usuario>> GetUsuarioAsync()
    {
        try
        {
            string sqlQuery = @"SELECT * FROM usuarios WHERE status != 'Inativo'";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Usuario>(sqlQuery);
            }
        } 
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }
    
    public async Task<Usuario> GetUsuarioByIdAsync(int id)
    {
        try
        {
            string sqlQuery = @"SELECT * FROM usuarios WHERE usuarioId = @Id";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Usuario>(sqlQuery, new {Id = id});
            }
        } 
        catch (PostgresException error)
        {
            throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
        }
    }

    public async Task<bool> PutUsuarioAsync(Usuario usuario, int id)
    {
        try
        {
            string sqlQuery = @"UPDATE usuarios
                                SET nome = @nome, 
                                    email = @email, 
                                    telefone = @telefone, 
                                    cargo = @cargo, 
                                    senha = @senha, 
                                    status = @status
                                WHERE usuarioId = @Id";
            
            var parametros = new DynamicParameters();
            parametros.Add("nome", usuario.Nome);
            parametros.Add("email", usuario.Email);
            parametros.Add("telefone", usuario.Telefone);
            parametros.Add("cargo", usuario.Cargo.ToString());
            parametros.Add("senha", usuario.Senha);
            parametros.Add("status", usuario.Status.ToString());
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

    public async Task<bool> DeleteLogicoUsuarioByIdAsync(int id)
    {
        try
        {
            string sqlQuery = @"UPDATE usuarios 
                                SET status = 'Inativo'
                                WHERE usuarioId = @Id";

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