using library_api.Application.DTOs;
using library_api.Application.Interfaces;
using library_api.Domain.DomainInterfaces;
using library_api.presentation.controllers;
using Moq;

namespace library_api.Tests.PresentationTest.ControllersTest;

public class EmprestimoControllerTest
{
    private readonly Mock<IEmprestimoService> _emprestimoServiceMock;
    private readonly Mock<IEmprestimoDomainService> _livroEmprestimoServiceMock;
    private readonly EmprestimoController _controller;

    public EmprestimoControllerTest()
    {
        _emprestimoServiceMock = new Mock<IEmprestimoService>();
        _livroEmprestimoServiceMock = new Mock<IEmprestimoDomainService>();
        _controller = new EmprestimoController(_emprestimoServiceMock.Object, _livroEmprestimoServiceMock.Object);
    }
    
    [Fact]
    public async Task RealizaEmprestimo_RetornaOK_QuandoRealizado()
    {
        //Arrange
        // EmprestimoDTO emprestimoDto = new EmprestimoDTO()
        // {
        //     
        // }
        
        //Act
        // var resultado = await _controller.Post(new LivroRequest(novoLivroDTO));
        
        //Assert
        // var resultadoOK = Assert.IsType<OkObjectResult>(resultado);
        // LivroReturn livrosRetornado = Assert.IsType<LivroReturn>(resultadoOK.Value);
        //
        // Assert.Equal(livrosRetornado.Nome, novoLivroDTO.nome);
        // Assert.Equal(livrosRetornado.Editora, novoLivroDTO.editora);
        // Assert.Equal(livrosRetornado.Autor, novoLivroDTO.autor);
        // Assert.Equal(livrosRetornado.Genero, novoLivroDTO.genero.ToString());
        // Assert.Equal(livrosRetornado.Disponibilidade, novoLivroDTO.disponibilidade.ToString());
    }
}