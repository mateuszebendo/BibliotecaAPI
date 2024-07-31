using library_api.Application.DTOs;
using library_api.Application.Interfaces;
using library_api.Domain.DomainInterfaces;
using library_api.Domain.Enums;
using library_api.presentation.controllers;
using library_api.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;
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
        EmprestimoDTO emprestimoDto = new EmprestimoDTO()
        {
            EmprestimoId = 3,
            DataEmprestimo = DateTime.Today,
            DataDevolucao = DateTime.Today,
            Status = StatusEmprestimo.Ativo,
            UsuarioId = 1,
            LivroId = 1
        };
        _livroEmprestimoServiceMock
            .Setup(service => service.CriaNovoEmprestimo(It.IsAny<EmprestimoDTO>()))
            .ReturnsAsync(emprestimoDto);
        
        //Act
        IActionResult resultado = await _controller.Post(new EmprestimoRequest(emprestimoDto));
        
        //Assert
        var resultadoOK = Assert.IsType<OkObjectResult>(resultado);
        EmprestimoReturn emprestimoRetornado = Assert.IsType<EmprestimoReturn>(resultadoOK.Value);
        
        Assert.Equal(emprestimoRetornado.EmprestimoId, emprestimoDto.EmprestimoId);
    }
    
    [Fact]
    public async Task DevolveLivro_RetornaOK_QuandoDevolve()
    {
        //Arrange
        EmprestimoDTO emprestimoDto = new EmprestimoDTO()
        {
            EmprestimoId = 3,
            DataEmprestimo = DateTime.Today,
            DataDevolucao = DateTime.Today,
            Status = StatusEmprestimo.Ativo,
            UsuarioId = 1,
            LivroId = 1
        };
        _livroEmprestimoServiceMock
            .Setup(service => service.ConcluiEmprestimo(It.IsAny<int>()))
            .ReturnsAsync(true);
        
        //Act
        IActionResult resultado = await _controller.ConcluiEmprestimo(3);

        // Assert
        var resultadoOK = Assert.IsType<OkObjectResult>(resultado);
        string mensagem = Assert.IsType<string>(resultadoOK.Value);

        Assert.Equal("Emprestimo concluido com sucesso", mensagem);
    }
}