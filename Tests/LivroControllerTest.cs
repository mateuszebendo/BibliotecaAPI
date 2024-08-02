using BibliotecaAPI.presentation.controllers;
using Moq;

namespace BibliotecaAPI.Tests;

public class LivroControllerTest
{
    private readonly Mock<ILivroService> _livroServiceMock;
    private readonly LivroController _controller;

    public LivroControllerTests()
    {
        _livroServiceMock = new Mock<ILivroService>();
        _controller = new LivroController(_livroServiceMock.Object);
    }
    
    [Fact]
    public void Test1()
    {

    }
}