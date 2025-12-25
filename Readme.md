# VocaBot - Discord Music Searcher

> Um Bot de Discord desenvolvido em C# para buscar informações sobre músicas de Vocaloid/UTAU utilizando a API pública do VocaDB.

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Discord](https://img.shields.io/badge/Discord-5865F2?style=for-the-badge&logo=discord&logoColor=white)

## Sobre o Projeto

Este projeto foi desenvolvido como parte dos meus estudos em **Integração de APIs** e **Estrutura de Dados** com C#. 

O objetivo é criar uma ponte entre o usuário no Discord e o banco de dados do **VocaDB**, permitindo consultas rápidas sobre músicas, artistas e links oficiais sem sair do chat.

### Funcionalidades
- [x] Conexão em tempo real via WebSocket (Discord.Net).
- [x] Consumo de API REST (VocaDB) via `HttpClient`.
- [x] Processamento de dados JSON (`Newtonsoft.Json`).
- [x] Comando de busca: `!voca [nome da música]`.

---

## Tecnologias Utilizadas

* **Linguagem:** C#
* **Framework:** .NET 8.0 (Console Application)
* **Bibliotecas (NuGet):**
    * `Discord.Net` (Interação com o Gateway do Discord)
    * `Newtonsoft.Json` (Deserialização da resposta da API)
* **API Externa:** [VocaDB API](https://vocadb.net/api)

---

## Como Rodar o Projeto

### Pré-requisitos
* [.NET SDK](https://dotnet.microsoft.com/download) instalado.
* Uma conta no [Discord Developer Portal](https://discord.com/developers/applications).

---

## Exemplo de Uso

No seu servidor do Discord:

**Usuário:**
> `!voca Rolling Girl`

**VocaBot:**
> Procurando por **Rolling Girl** no VocaDB...
> 
> **Encontrei!**
> **Nome:** Rolling Girl
> **Artistas:** wowaka feat. Hatsune Miku
> **Link:** https://vocadb.net/S/176

---

## Como Funciona (Lógica da API)

O núcleo do projeto é o método `BuscarMusicaNoVocaDB`. Aqui aplicamos conceitos de requisições HTTP assíncronas:

```csharp
// 1. Definição do Endpoint (URL da API)
string url = $"[https://vocadb.net/api/songs?query=](https://vocadb.net/api/songs?query=){termo}&maxResults=1&sort=Popularity";

// 2. Requisição GET (HttpClient)
// O "await" libera a thread enquanto esperamos a resposta da internet (Async/Await)
HttpResponseMessage resposta = await _httpClient.GetAsync(url);

// 3. Parsing (JSON -> Objeto C#)
// Transformamos o texto cru da resposta em um objeto navegável
var dados = JObject.Parse(jsonResposta);
