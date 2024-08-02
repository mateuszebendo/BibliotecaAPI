// using System.Text;
// using System.Text.Json;
// using BibliotecaAPI.Application.DTOs;
// using BibliotecaAPI.Domain.Enums;
// using BibliotecaAPI.Domain.Messaging.Producers;
// using BibliotecaAPI.Infrastructure.Messaging.Producers;
// using Moq;
// using RabbitMQ.Client;
//
// namespace BibliotecaAPI.Tests.InfrastructureTest.MessagingTest.ProducerTest;
//
// public class LivroProducerTest
// {
//     private readonly Mock<ILivroProducer> _livroProducerMock;
//     private readonly LivroProducer _livroProducer;
//     private readonly IModel _channel;
//     
//     public LivroProducerTest()
//     {
//         _livroProducerMock = new Mock<ILivroProducer>();
//         _livroProducer = new LivroProducer(_channel);
//     }
//
//     [Fact]
//     public void EnviaAvisoLivroLancado_DevePublicarMensagem()
//     {
//         // Arrange
//         LivroDTO novoLivroDTO = new LivroDTO()
//         {
//             nome = "Livro Teste 1",
//             editora = "Editora Exemplo 1",
//             autor = "Autor Exemplo 1",
//             genero = GeneroLivro.Ficcao,
//             disponibilidade = StatusLivro.Disponivel  
//         };
//         var expectedBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(novoLivroDTO));
//         string actualExchange = null;
//         string actualRoutingKey = null;
//         IBasicProperties actualProps = null;
//         byte[] actualBody = null;
//
//         _livroProducerMock
//             .Setup(producer => producer.EnviaAvisoLivroDisponivel(It.IsAny<LivroDTO>()))
//             .Callback<LivroDTO>(dto =>
//             {
//                 actualExchange = "livro-alertas";
//                 actualRoutingKey = "novo-livro-lancado";
//                 actualProps = null;
//                 actualBody = expectedBody;
//             });
//
//         // Act
//         _livroProducer.EnviaAvisoLivroDisponivel(novoLivroDTO);
//
//         // Assert
//         Assert.Equal("livro-alertas", actualExchange);
//         Assert.Equal("novo-livro-lancado", actualRoutingKey);
//         Assert.Null(actualProps);
//         Assert.Equal(expectedBody, actualBody);
//     }
//
//
// }
