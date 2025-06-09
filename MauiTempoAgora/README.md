# MauiTempoAgora

## Introdução

MauiTempoAgora é uma aplicação desenvolvida com .NET MAUI que permite consultar previsões do tempo em tempo real, com base na cidade informada pelo usuário. Além disso, é possível armazenar, pesquisar e excluir registros de previsões anteriores em um histórico local.

## Funcionalidades

- Buscar previsões do tempo de cidades;
- Armazenar os dados das previsões em um histórico;
- Pesquisar previsões anteriores pelo nome da cidade;
- Apagar registros do histórico.

## Instalação

### Clone o repositório
```
git clone https://github.com/seu-usuario/MauiComprinhasDaShein.git
cd MauiComprinhasDaShein 
```
### Restaure os pacotes
```
dotnet restore
```
### Execute o projeto
```
dotnet build
dotnet run
```

#### Observação

Esta aplicação usa a API da OpenWeatherMap. Certifique-se de substituir a chave na classe DataService pela sua chave pessoal se desejar fazer muitas consultas:
```
string chave = "<sua_chave_aqui>";
```

## Estrutura do Projeto

### Banco de Dados

O projeto utiliza SQLite para persistência local das previsões. A classe SQLiteDatabaseHelper gerencia operações como inserção, remoção e busca:

- `Insert(Tempo p)`: Insere previsão;
- `Delete(int id)`: Remove previsão por ID;
- `GetAll()`: Retorna todas as previsões;
- `Search(string q)`: Pesquisa previsões por cidade.

### Modelos de Dados

A classe Tempo representa a estrutura dos dados de previsão:

- Cidade, latitude e longitude;
- Temperatura máxima e mínima;
- Visibilidade e velocidade do vento;
- Descrições climáticas;
- Horários de nascer e pôr do sol;
- Data da consulta.

### Serviços

A classe DataService é responsável por:

- Realizar requisição HTTP à API OpenWeatherMap;
- Obter e converter os dados JSON em objetos Tempo.

### Interface do Usuário (UI)

A interface principal (MainPage.xaml) permite:

- Inserir o nome da cidade e buscar dados;
- Exibir os resultados obtidos com detalhes;
- Visualizar histórico em uma ListView organizada por colunas;
- Pesquisar no histórico com SearchBar;
- Excluir previsões salvas diretamente da lista.

## Licença

MIT License