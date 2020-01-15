# Campeonato de Filmes

## Pré requisitos

- .NET Core SDK 3.1 (https://dotnet.microsoft.com/download/dotnet-core)
- yarn (https://yarnpkg.com/lang/en/docs/install/)

## Inicialização

    git clone https://github.com/fc1943s/CopaFilmes.git
    cd CopaFilmes
    
    dotnet tool restore
    dotnet paket restore
    yarn --cwd CopaFilmes.Frontend install


## Execução

#### Backend

    dotnet run --project CopaFilmes.Backend


#### Frontend

    yarn --cwd CopaFilmes.Frontend run webpack-dev-server


#### Backend + Frontend

    yarn --cwd CopaFilmes.Frontend run concurrently 'dotnet run --project ../CopaFilmes.Backend' 'yarn --cwd CopaFilmes.Frontend run webpack-dev-server'

#### Testes

    dotnet test


## Notas

Para acessar o frontend: http://localhost:8082

A API responde no endereço http://localhost:8085


Todos os projetos podem ser visualizados na solution `CopaFilmes.sln`.
