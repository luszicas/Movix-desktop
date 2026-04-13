# 🎬 Movix — Full Solution (Web, Desktop & API)

## 📌 Sobre o Projeto

O **Movix** é uma solução completa de catálogo e streaming de vídeos construída sob uma arquitetura de software moderna e escalável.

Este repositório contém o ecossistema completo, integrando uma aplicação administrativa robusta (Desktop) e um portal de entretenimento (Web), ambos alimentados por uma Web API centralizada.

O projeto demonstra a capacidade de unir engenharia de backend, segurança de dados e múltiplas interfaces em uma única solução sincronizada.

---

## 🏗️ Arquitetura da Solução

A solução foi estruturada em **6 projetos organizados em camadas**, garantindo separação de responsabilidades e facilidade de manutenção:

- **Movix.Api**  
  Gateway central (REST API) que expõe os serviços para todos os clientes.

- **Movix.Desktop**  
  Painel administrativo em Windows Forms (C#) com interface Guna UI.

- **Movix.Web**  
  Portal do usuário final desenvolvido em ASP.NET Core MVC.

- **Movix.Domain**  
  Núcleo do sistema com entidades (`Filme`, `Usuario`) e regras de negócio.

- **Movix.Application**  
  Camada de orquestração, casos de uso e DTOs.

- **Movix.Infrastructure**  
  Persistência de dados com Entity Framework Core e suporte ao SQL Server.

---

## 🚀 Funcionalidades Principais

### 🖥️ Gestão Administrativa (Desktop)

- 🔐 **Autenticação Segura**  
  Login via API com validação de credenciais e Roles.

- 🎞️ **CRUD de Catálogo**  
  Cadastro, edição e exclusão de filmes com suporte a upload de arquivos locais ou URLs.

- 👥 **Gestão de Usuários**  
  Controle completo de perfis e status de atividade (Ativo/Inativo).

---

### 🌐 Experiência do Usuário (Web)

- 🎬 **Catálogo Dinâmico**  
  Visualização de filmes e lançamentos em tempo real.

- 💳 **Sessão de Planos**  
  Área dedicada para escolha de assinaturas (Básico, Standard e Platinum).

- ▶️ **Player de Trailers**  
  Integração para visualização de trailers diretamente no portal.

---

## ⚙️ Tecnologias Utilizadas

- **Linguagem:** C# / .NET 9 
- **Backend:** ASP.NET Core Web API  
- **ORM:** Entity Framework Core  
- **Frontend Web:** ASP.NET Core MVC (Views HTML/CSS)  
- **Desktop:** Windows Forms + Guna UI Framework  
- **Banco de Dados:** SQL Server  
- **Segurança:** ASP.NET Core Identity (Gerenciamento de Roles e Hash de Senhas)

---

## 🔌 Integração e Fluxo de Dados

O sistema utiliza uma "ponte digital" baseada no protocolo HTTP:

1. **Requisição**  
   O Desktop e a Web enviam pedidos via `HttpClient`.

2. **Transporte**  
   Os dados trafegam em formato JSON leve e seguro.

3. **Processamento**  
   A API processa a lógica, consulta o banco via EF Core e devolve a resposta.

4. **Sincronização**  
   Uma alteração feita no Desktop reflete instantaneamente na interface Web, pois ambos compartilham a mesma base de dados.

---

## 👨‍💻 Autor

**Lucas**  
Desenvolvedor Full Stack (.NET / C#)

---

⭐ **Movix:** Integrando performance desktop com a conectividade da web.
