using library_api.Application.DTOs;
using library_api.Application.Entities;
using library_api.Application.Interfaces;
using library_api.Domain.Enums;
using library_api.Domain.Services;
using library_api.presentation.controllers;
using library_api.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace library_api.Tests.PresentationTest.ControllersTest;

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