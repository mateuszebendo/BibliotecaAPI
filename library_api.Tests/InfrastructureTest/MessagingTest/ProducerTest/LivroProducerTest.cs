using System.Text;
using System.Text.Json;
using library_api.Application.DTOs;
using library_api.Domain.Enums;
using library_api.Infrastructure.Messaging.Producers;
using Moq;
using RabbitMQ.Client;

namespace library_api.Tests.InfrastructureTest.MessagingTest.ProducerTest;

public class LivroProducerTest
{
    
    private readonly Mock<IModel> _mockChannel;
    private readonly LivroProducer _producer;

    public LivroProducerTest()
    {
        _mockChannel = new Mock<IModel>();
        _producer = new LivroProducer(_mockChannel.Object);
    }


    [Fact]
        public void EnviaAvisoLivroDisponivel_ShouldPublishMessage()
        {
            // Arrange
            LivroDTO livroDto = new LivroDTO
            {
                nome = "Livro Teste 1",
                editora = "Editora Exemplo 1",
                autor = "Autor Exemplo 1",
                genero = GeneroLivro.Ficcao,
                disponibilidade = StatusLivro.Disponivel  
            };
            var expectedBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(livroDto));
            string actualExchange = null;
            string actualRoutingKey = null;
            IBasicProperties actualProps = null;
            byte[] actualBody = null;

            // Setup the mock to capture parameters
            _mockChannel
                .Setup(channel => channel.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()))
                .Callback<string, string, IBasicProperties, byte[]>((exchange, routingKey, props, body) =>
                {
                    actualExchange = exchange;
                    actualRoutingKey = routingKey;
                    actualProps = props;
                    actualBody = body;
                });

            // Act
            _producer.EnviaAvisoLivroDisponivel(livroDto);

            // Assert
            Assert.Equal("livro-alertas", actualExchange);
            Assert.Equal("novo-livro-disponivel", actualRoutingKey);
            Assert.Null(actualProps);
            Assert.Equal(expectedBody, actualBody);
        }
        
        [Fact]
        public void EnviaAvisoLivroLancado_ShouldPublishMessage()
        {
            // Arrange
            var message = "Novo Livro Teste";
            var expectedBody = Encoding.UTF8.GetBytes(message);
            string actualExchange = null;
            string actualRoutingKey = null;
            IBasicProperties actualProps = null;
            byte[] actualBody = null;

            // Setup the mock to capture parameters
            _mockChannel
                .Setup(channel => channel.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()))
                .Callback<string, string, IBasicProperties, byte[]>((exchange, routingKey, props, body) =>
                {
                    actualExchange = exchange;
                    actualRoutingKey = routingKey;
                    actualProps = props;
                    actualBody = body;
                });

            // Act
            _producer.EnviaAvisoLivroLancado(message);

            // Assert
            Assert.Equal("livro-alertas", actualExchange);
            Assert.Equal("novo-livro-lancado", actualRoutingKey);
            Assert.Null(actualProps);
            Assert.Equal(expectedBody, actualBody);
    }
}