# 📚 Sistema de Empréstimo de Livros

Este projeto é uma API REST construída com ASP.NET Core e MongoDB que permite gerenciar uma biblioteca com funcionalidades como cadastro de livros, usuários, autores, avaliações, empréstimos e devoluções.

---

## 🚀 Funcionalidades

- 📖 Cadastro, edição e remoção de livros
- 👤 Gerenciamento de usuários (CRUD)
- 🖋️ Cadastro e gerenciamento de autores
- 📅 Registro de empréstimos e devoluções
- ⭐ Avaliações de livros por usuários
- 🔍 Filtro de livros disponíveis para empréstimo
- 📈 Histórico de empréstimos por usuário

---

## 🛠️ Tecnologias Utilizadas

- ASP.NET Core Web API
- MongoDB (NoSQL)
- MongoDB.Driver
- Swagger (OpenAPI)
- CORS habilitado

---

## 📁 Estrutura do Projeto

```bash
/Controllers
  - LivrosController.cs
  - UsuariosController.cs
  - AutoresController.cs
  - EmprestimosController.cs
  - AvaliacoesController.cs

/Models
  - Livro.cs
  - Usuario.cs
  - Autor.cs
  - Emprestimo.cs
  - Avaliacao.cs

/Services
  - LivroService.cs
  - UsuarioService.cs
  - AutorService.cs
  - EmprestimoService.cs
  - AvaliacaoService.cs
