# living-room-api

## API ASP .NET para manipulação de informacoes entre pessoas e seus eletrodomesticos com autenticação jwt e persistência em servidor SQL.

### Para rodar o projeto

~~~bash
git clone
~~~
~~~
dotnet restore
~~~
~~~
dotnet run
~~~

### Documentação e teste

Para fins de documentação e teste da API, a aplicação conta com o __Swagger__, uma interface que mostra informações de todas as requisições possíveis com instruções de como executá-las da forma correta, além de possibilitar estas execuções.
Para acessar a documentação da API basta acessar _https://localhost:5001/swagger_.

### Como consumir

No primeiro acesso à API, há apenas uma requisição autorizada:
```Post /api/Users/authenticate```.

Informe no corpo da requisição o objeto JSON com as informações de usuário padrão:
~~~json
{ 
  Identificacao: "admin-1234",
  Nome: "admin",
  Email: "admin@example.com",
  Senha: "admin123"
}

### Hospedagem
~~~

A respota do servidor será um token de autenticação JWT Bearer.
Agora, sempre que for fazer uma requisição, insira o campo authorization em seu header com o valor 'Bearer \[espaço\] \[seu-token\]' e você será considerado autorizado.
O token de autenticação expira em uma hora, garantindo a segurança do usuário.
