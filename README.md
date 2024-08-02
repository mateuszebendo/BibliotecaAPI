<p align="center">
 <a href="#tech">Tecnologias</a> • 
 <a href="#started">Como testar</a> • 
  <a href="#routes">API Endpoints</a> •
 <a href="#contribute">Contribute</a>
</p>

<p align="center">
    <b>API feita em .NET 8 em conjunto com RabbitMQ para gerenciamento de bibliotecas.</b>
</p>

<h2 id="technologies">💻 Tecnologias</h2>

- [.NET 8](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [RabbitMQ](https://www.rabbitmq.com/)
- [PostgresSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/)

<h2 id="started">🚀 Como testar</h2>

Para rodar esse projeto localmente, é necessário que você tenha o as tecnologias citadas acima disponíveis na sua máquina. Após clonar o repositório, configure no appsettings.json o acesso ao seu banco de dados dentro do campo "SqlConnection" (usuário, porta, senha, etc) e acesso ao RabbitMQ em "RabbitMQ". Por fim, é necessária a configuração das portas que o RabbitMQ usará pelo Docker.

<h3>Clonando e iniciando o projeto</h3>

```bash
# Clone este repositório
$ git clone https://github.com/mateuszebendo/BibliotecaAPI.git

# Acesse o repositório
$ cd BibliotecaAPI

# Configure as portas do RabbitMQ no docker (mude as portas conforme o necessário)
$ docker run -d --name BibliotecaAPI -p 15672:15672 -p 5672:5672 rabbitmq:3-management

# Compile o projeto
$ dotnet build

# Acesse o projeto Presentation
$ cd Presentation

# Execute a aplicação
$ dotnet run

# Para testar o endpoints, acesse o swagger dentro do localhost pelo seu navegador ou use uma ferramente como o Postman ou o Insominia.
```

