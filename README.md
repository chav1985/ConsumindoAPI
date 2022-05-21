# Subindo API e Consumindo em .NET Core

Foi utilizado o [.NET Core 5.0 (Consumidor) e .NET Core 3.1 (API)](https://www.microsoft.com/net/download).

## Instruções para executar o build do projeto

Na raiz da solution, execute os comandos em um terminal para compilar:

```sh
dotnet restore
dotnet build -c Release
```

## Instruções para executar o projeto

Na raiz da solution, execute os comandos em um terminal para subir a API:

```sh
cd ProjetoAPI/bin/Release/netcoreapp3.1
API.exe
```

Na raiz da solution, execute os comandos em um terminal para subir o Consumidor:

```sh
cd ConsumerAPI/bin/Release/net5.0
ConsumerAPI.exe
```