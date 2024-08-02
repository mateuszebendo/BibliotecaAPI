using BibliotecaAPI.Application.DTOs;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Application.Interfaces;
using BibliotecaAPI.Domain.DomainInterfaces;
using BibliotecaAPI.Domain.Enums;
using BibliotecaAPI.presentation.controllers;
using BibliotecaAPI.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BibliotecaAPI.Tests.PresentationTest.ControllersTest;

public class LivroControllerTest
{
    private readonly Mock<ILivroService> _livroServiceMock;
    private readonly LivroController _controller;

    public LivroControllerTest()
    {
        _livroServiceMock = new Mock<ILivroService>();
        _controller = new LivroController(_livroServiceMock.Object);
    }

    [Fact]
    public async Task CriaNovoLivro_RetornaOkELivroResult_QuandoConsegue()
    {
        //Arrange
        LivroDTO novoLivroDTO = new LivroDTO()
        {
            Nome = "Livro Teste 1",
            Editora = "Editora Exemplo 1",
            Autor = "Autor Exemplo 1",
            Genero = GeneroLivro.Ficcao,
            Disponibilidade = StatusLivro.Disponivel  
        };
        _livroServiceMock.Setup(service => service.CriaNovoLivro(It.IsAny<LivroDTO>()))
        .ReturnsAsync(novoLivroDTO);
        
        //Act
        var resultado = await _controller.Post(new LivroRequest(novoLivroDTO));
        
        //Assert
        var resultadoOK = Assert.IsType<OkObjectResult>(resultado);
        LivroReturn livrosRetornado = Assert.IsType<LivroReturn>(resultadoOK.Value);
        
        Assert.Equal(livrosRetornado.Nome, novoLivroDTO.Nome);
        Assert.Equal(livrosRetornado.Editora, novoLivroDTO.Editora);
        Assert.Equal(livrosRetornado.Autor, novoLivroDTO.Autor);
        Assert.Equal(livrosRetornado.Genero, novoLivroDTO.Genero.ToString());
        Assert.Equal(livrosRetornado.Disponibilidade, novoLivroDTO.Disponibilidade.ToString());
    }

    [Fact]
    public async Task ResgatoTodosLivros_RetornaResultadoOk_QuandoConseguir()
    {
        
        //Arrange
        List<LivroDTO> livros = new List<LivroDTO>
        {
            new LivroDTO
            {
                LivroId = 1,
                Nome = "Livro Teste 1",
                Editora = "Editora Exemplo 1",
                Autor = "Autor Exemplo 1",
                Genero = GeneroLivro.Ficcao,
                Disponibilidade = StatusLivro.Disponivel
            },
            new LivroDTO
            {
                LivroId = 2,
                Nome = "Livro Teste 2",
                Editora = "Editora Exemplo 2",
                Autor = "Autor Exemplo 2",
                Genero = GeneroLivro.Romance,
                Disponibilidade = StatusLivro.Emprestado
            },
            new LivroDTO
            {
                LivroId = 3,
                Nome = "Livro Teste 3",
                Editora = "Editora Exemplo 3",
                Autor = "Autor Exemplo 3",
                Genero = GeneroLivro.Fantasia,
                Disponibilidade = StatusLivro.Emprestado
            },
            new LivroDTO
            {
                LivroId = 4,
                Nome = "Livro Teste 4",
                Editora = "Editora Exemplo 4",
                Autor = "Autor Exemplo 4",
                Genero = GeneroLivro.LiteraturaClassica,
                Disponibilidade = StatusLivro.Disponivel
            },
            new LivroDTO
            {
                LivroId = 5,
                Nome = "Livro Teste 5",
                Editora = "Editora Exemplo 5",
                Autor = "Autor Exemplo 5",
                Genero = GeneroLivro.Ficcao,
                Disponibilidade = StatusLivro.Emprestado
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
            Assert.Equal(livros[index].LivroId, livroRetornado.LivroId);
            index++;
        }
    }
    
    [Fact]
    public async Task GetLivro_RetornaResultadoOk_QuandoLivroExiste()
    {
        
        //Arrange
        int LivroId = 1;
        Livro livro = new Livro{LivroId = 1,
                                Nome = "Livro Teste",
                                Editora = "Editora Exemplo",
                                Autor = "Autor Exemplo",
                                Genero = GeneroLivro.Ficcao,
                                Disponibilidade = StatusLivro.Disponivel };
        _livroServiceMock.Setup(service => service.RecuperaLivroPorId(LivroId))
            .ReturnsAsync(new LivroDTO(livro));
        
        //Act
        var resultado = await _controller.Get(LivroId);
        
        //Assert 
        var resultadoOK = Assert.IsType<OkObjectResult>(resultado);
        LivroReturn livroRetornado = Assert.IsType<LivroReturn>(resultadoOK.Value);
        
        Assert.Equal(LivroId, livroRetornado.LivroId);
    }
}