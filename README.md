# Desafio

O projeto contém chaves e strings de conexão nos arquivos para ser mais fácil de testar, em um ambiente de produção o mais indicado é colocar nas variáveis de ambiente do windows

Para rodar o banco de dados será o seguinte:

A aplicação utiliza o LocalDB do SQLServer, você precisa liberá a porta 1433 (Geralmente já é o padrão)

Verifique se o serviço "SQL Server Browser" está ativo.
Habilite TCP/IP no SQL Server Configuration Manager:

Abra o SQL Server Configuration Manager

Vá em SQL Server Network Configuration > Protocols for MSSQLSERVER

Habilite o TCP/IP

Reinicie o SQL Server.

Verifique se a porta 1433 está aberta no firewall.

Agora você precisará de uma conta SQLServer que tenha acesso ao LocalDB e com permissão de criar database, coloque o usuario e senha no appsettings do backend, na string de conexão default, substituindo o usuario e senha existentes.

Após isso será necessário realizar as migrations, na pasta do projeto backend, no mesmo local onde está o arquivo "backend.csproj", digite o comando "dotnet ef database update", o banco de dados será criado e já será possível utilizar a aplicação.

Para rodar o projeto localmente com docker, deixe o docker desktop aberto, digite no terminal o comando "docker-compose up --build" na pasta raiz do repositório, onde está localizado o arquivo "docker-compose".

Para entrar na aplicação, você pode acessar o backend através do swagger e utilizar o endpoint "AdicionarUsuario", isso criará um usuário padrão, esse endpoint serve apenas para testes e não necessita de auth, o usuario criado será 
Usuario: Usuario
Senha: SenhaTeste123!

Para fazer outros testes pelo swagger, você pode acessar o endpoint "Login", enviar as informações do usuario acima, ele retornará um token.
No canto superior direito do swagger haverá o botão de [authorize], no campo coloque "Bearer (token que foi retornado)" e a partir de agora você estará autenticado para utilizar os outros endpoints

frontend fica em http://localhost:5000/
backend fica em http://localhost:5001/swagger

para verificar a aplicação na azure acesse: https://frontend20250709120804-e4g4ggfmg4hna6aq.canadacentral-01.azurewebsites.net/
