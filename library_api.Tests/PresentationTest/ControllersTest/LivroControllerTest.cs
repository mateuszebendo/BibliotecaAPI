using library_api.Application.DTOs;
using library_api.Application.Entities;
using library_api.Application.Interfaces;
using library_api.Domain.DomainInterfaces;
using library_api.Domain.Enums;
using library_api.presentation.controllers;
using library_api.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace library_api.Tests.PresentationTest.ControllersTest;

public class LivroControllerTest
{
    private readonly Mock<ILivroService> _livroServiceMock;
    private readonly Mock<ILivroDomainService> _livroDomainServiceMock;
    private readonly LivroController _controller;

    public LivroControllerTest()
    {
        _livroServiceMock = new Mock<ILivroService>();
        _livroDomainServiceMock = new Mock<ILivroDomainService>();
        _controller = new LivroController(_livroServiceMock.Object, _livroDomainServiceMock.Object);
    }

    [Fact]
    public async Task CriaNovoLivro_RetornaOkELivroResult_QuandoConsegue()
    {
        //Arrange
        LivroDTO novoLivroDTO = new LivroDTO()
        {
            nome = "Livro Teste 1",
            editora = "Editora Exemplo 1",
            autor = "Autor Exemplo 1",
            genero = GeneroLivro.Ficcao,
            disponibilidade = StatusLivro.Disponivel  
        };
        _livroDomainServiceMock.Setup(service => service.CriaNovoLivro(novoLivroDTO))
            .ReturnsAsync(novoLivroDTO);
        
        //Act
        var resultado = await _controller.Post(new LivroRequest(novoLivroDTO));
        
        //Assert
        var resultadoOK = Assert.IsType<OkObjectResult>(resultado);
        LivroReturn livrosRetornado = Assert.IsType<LivroReturn>(resultadoOK.Value);
        
        Assert.Equal(livrosRetornado.Nome, novoLivroDTO.nome);
        Assert.Equal(livrosRetornado.Editora, novoLivroDTO.editora);
        Assert.Equal(livrosRetornado.Autor, novoLivroDTO.autor);
        Assert.Equal(livrosRetornado.Genero, novoLivroDTO.genero.ToString());
        Assert.Equal(livrosRetornado.Disponibilidade, novoLivroDTO.disponibilidade.ToString());
    }

    [Fact]
    public async Task ResgatoTodosLivros_RetornaResultadoOk_QuandoConseguir()
    {
        
        //Arrange
        List<LivroDTO> livros = new List<LivroDTO>
        {
            new LivroDTO
            {
                livroId = 1,
                nome = "Livro Teste 1",
                editora = "Editora Exemplo 1",
                autor = "Autor Exemplo 1",
                genero = GeneroLivro.Ficcao,
                disponibilidade = StatusLivro.Disponivel
            },
            new LivroDTO
            {
                livroId = 2,
                nome = "Livro Teste 2",
                editora = "Editora Exemplo 2",
                autor = "Autor Exemplo 2",
                genero = GeneroLivro.Romance,
                disponibilidade = StatusLivro.Emprestado
            },
            new LivroDTO
            {
                livroId = 3,
                nome = "Livro Teste 3",
                editora = "Editora Exemplo 3",
                autor = "Autor Exemplo 3",
                genero = GeneroLivro.Fantasia,
                disponibilidade = StatusLivro.Emprestado
            },
            new LivroDTO
            {
                livroId = 4,
                nome = "Livro Teste 4",
                editora = "Editora Exemplo 4",
                autor = "Autor Exemplo 4",
                genero = GeneroLivro.LiteraturaClassica,
                disponibilidade = StatusLivro.Disponivel
            },
            new LivroDTO
            {
                livroId = 5,
                nome = "Livro Teste 5",
                editora = "Editora Exemplo 5",
                autor = "Autor Exemplo 5",
                genero = GeneroLivro.Ficcao,
                disponibilidade = StatusLivro.Emprestado
            }
        };
        _livroServiceMock.Setup(service => service.RecuperaTodosLivros())
            .ReturnsAsync(livros);
        
        //Act
        var resultado = await _controller.Get();
        
        //Assert 
        var resultadoOK = Assert.IsType<OkObjectResult>(resultado);
        List<LivroReturn> livrosRetornados = Assert.IsType<List<LivroReturn>>(resultadoOK.Value);

        int index = 0;
        foreach (var livroRetornado in livrosRetornados)
        {
            Assert.Equal(livros[index].livroId, livroRetornado.LivroId);
            index++;
        }
    }
    
    [Fact]
    public async Task GetLivro_RetornaResultadoOk_QuandoLivroExiste()
    {
        
        //Arrange
        int livroId = 1;
        Livro livro = new Livro{LivroId = 1,
                                Nome = "Livro Teste",
                                Editora = "Editora Exemplo",
                                Autor = "Autor Exemplo",
                                Genero = GeneroLivro.Ficcao,
                                Disponibilidade = StatusLivro.Disponivel };
        _livroServiceMock.Setup(service => service.RecuperaLivroPorId(livroId))
            .ReturnsAsync(new LivroDTO(livro));
        
        //Act
        var resultado = await _controller.Get(livroId);
        
        //Assert 
        var resultadoOK = Assert.IsType<OkObjectResult>(resultado);
        LivroReturn livroRetornado = Assert.IsType<LivroReturn>(resultadoOK.Value);
        
        Assert.Equal(livroId, livroRetornado.LivroId);
    }
}