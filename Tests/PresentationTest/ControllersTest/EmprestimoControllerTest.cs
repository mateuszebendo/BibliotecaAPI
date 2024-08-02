using BibliotecaAPI.Application.DTOs;
using BibliotecaAPI.Application.Interfaces;
using BibliotecaAPI.Domain.DomainInterfaces;
using BibliotecaAPI.Domain.Enums;
using BibliotecaAPI.presentation.controllers;
using BibliotecaAPI.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BibliotecaAPI.Tests.PresentationTest.ControllersTest;

public class EmprestimoControllerTest
{
    private readonly Mock<IEmprestimoService> _emprestimoServiceMock;
    private readonly EmprestimoController _controller;

    public EmprestimoControllerTest()
    {
        _emprestimoServiceMock = new Mock<IEmprestimoService>();
        _controller = new EmprestimoController(_emprestimoServiceMock.Object);
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
        _emprestimoServiceMock
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
        _emprestimoServiceMock
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