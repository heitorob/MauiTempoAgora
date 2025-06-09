# MauiTempoAgora

## Introdu��o

MauiTempoAgora � uma aplica��o desenvolvida com .NET MAUI que permite consultar previs�es do tempo em tempo real, com base na cidade informada pelo usu�rio. Al�m disso, � poss�vel armazenar, pesquisar e excluir registros de previs�es anteriores em um hist�rico local.

## Funcionalidades

- Buscar previs�es do tempo de cidades;
- Armazenar os dados das previs�es em um hist�rico;
- Pesquisar previs�es anteriores pelo nome da cidade;
- Apagar registros do hist�rico.

## Instala��o

### Clone o reposit�rio
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

#### Observa��o

Esta aplica��o usa a API da OpenWeatherMap. Certifique-se de substituir a chave na classe DataService pela sua chave pessoal se desejar fazer muitas consultas:
```
string chave = "<sua_chave_aqui>";
```

## Estrutura do Projeto

### Banco de Dados

O projeto utiliza SQLite para persist�ncia local das previs�es. A classe SQLiteDatabaseHelper gerencia opera��es como inser��o, remo��o e busca:

- `Insert(Tempo p)`: Insere previs�o;
- `Delete(int id)`: Remove previs�o por ID;
- `GetAll()`: Retorna todas as previs�es;
- `Search(string q)`: Pesquisa previs�es por cidade.

### Modelos de Dados

A classe Tempo representa a estrutura dos dados de previs�o:

- Cidade, latitude e longitude;
- Temperatura m�xima e m�nima;
- Visibilidade e velocidade do vento;
- Descri��es clim�ticas;
- Hor�rios de nascer e p�r do sol;
- Data da consulta.

### Servi�os

A classe DataService � respons�vel por:

- Realizar requisi��o HTTP � API OpenWeatherMap;
- Obter e converter os dados JSON em objetos Tempo.

### Interface do Usu�rio (UI)

A interface principal (MainPage.xaml) permite:

- Inserir o nome da cidade e buscar dados;
- Exibir os resultados obtidos com detalhes;
- Visualizar hist�rico em uma ListView organizada por colunas;
- Pesquisar no hist�rico com SearchBar;
- Excluir previs�es salvas diretamente da lista.

## Licen�a

MIT License