# 🌿 Projeto Horta Comunitaria
API para a gestão e monitorização de hortas comunitárias.
> **O que é uma Horta Comunitária?** > São espaços urbanos ou rurais, disponibilizados pela prefeitura, onde grupos de pessoas cultivam alimentos de forma coletiva. Este projeto visa unir sustentabilidade e tecnologia ao digitalizar essa gestão, permitindo que produtores locais organizem seus estoques e facilitando para que a comunidade reserve alimentos frescos de forma eficiente

## 📌 Visão Geral do Projeto
![Status do Projeto](https://img.shields.io/badge/status-em%20desenvolvimento-green)
[![Backend](https://img.shields.io/badge/Backend-ASP.NET%20Core%208.0-blue)](https://dotnet.microsoft.com/)
[![Database](https://img.shields.io/badge/Database-MySQL-orange)](https://www.mysql.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-green)](#)

O sistema facilita a logística de hortas urbanas. O fluxo baseia-se na reserva de itens frescos (sem necessidade de login para o cliente) e na retirada física validada por um código de segurança.

### Principais Funcionalidades
- **Gestão de Estoque:** Controle granular por produto e vendedor.
- **Reservas Inteligentes:** Fluxo de reserva com cálculo automático de taxas e expiração.
- **Segurança:** Autenticação via JWT com **Silent Refresh** (Access & Refresh Token).
<!-- - **Mensageria:** RabbitMQ para notificar vendedores sobre novas reservas.
- **Background Jobs:** Redis + Worker para geração de relatórios complexos em Excel. -->

## 🏗️ Arquitetura e Estrutura (DDD)

O projeto segue os princípios da **Clean Architecture**, garantindo que as regras de negócio sejam independentes de frameworks externos.

```text
/src
/Horta.Domain          # Entidades, Value Objects, Interfaces e Regras de Negócio Puras
/Horta.Infrastructure  # Implementação de Repositórios (EF Core), Identity
/Horta.Application     # Casos de Uso (Commands/Queries), DTOs, Mapeamentos e Mediators
/Horta.API             # Controllers, Middlewares, Filtros e Configurações de DI
/Horta.Test            # Testes Unitários e de Integração
```
<!-- /Horta.Test            # Testes Unitários e de Integração (xUnit, Moq, FluentAssertions) -->

## 🔐 Fluxo de Autenticação

Implementamos um modelo de tokens duplos para maior segurança, gerenciado via endpoints:

1. **Access Token (JWT):** Válido por 15 min. Autoriza as chamadas à API.
2. **Refresh Token:** Enviado para o endpoint `/refresh` para gerar um novo par de tokens quando o access token expira.

| Token | Duração | Armazenamento (Front-end) |
| :--- | :--- | :--- |
| Access Token | 15 min | SessionStorage / Application State |
| Refresh Token | 7 dias | LocalStorage / Persisted State |

## 🔁 Fluxo de Reserva

1. **Seleção:** Usuário escolhe produtos de múltiplos vendedores.
2. **Reserva:** O sistema abate o estoque e gera um `CodigoSeguranca`.
3. **Notificação:** O RabbitMQ dispara o evento para os vendedores.
4. **Confirmação:** O vendedor valida o código na entrega, alterando o status para `Confirmada`.

<img width="1421" height="892" alt="FluxoReserva" src="https://github.com/user-attachments/assets/03c56f12-1e91-43ed-93f3-924275b0bf0e" />


## 🔁 Fluxo do Dashboard

1. **Cadastra:** Vendedor cadastra produto e quantidade em estoque.
2. **Controle:** Vendedor ativa e desativa produto para o clientes vizualizar ou não.
3. **Acessa Dashboard:** O vendedor pode verificar os dados de suas reservas por status e relatório anual.
4. **Relatório:** O vendedor pode gerar um relatório mensal ou anual de suas reservas.

<img width="1398" height="923" alt="fluxoVendedor" src="https://github.com/user-attachments/assets/3f4e66a0-da6b-4316-bf5e-8b21b538fd83" />


## 🧪 Estratégia de Testes

A camada **Horta.Test** é fundamental para garantir a confiabilidade das regras de negócio (Domain) e dos fluxos de aplicação.

- **Testes Unitários:** Focados nas entidades de domínio e cálculos de taxas.
- **Testes de Integração:** Validam a persistência no banco e a comunicação com serviços externos.

## 📩 Notificações Multicanal

No futuro, novos consumidores serão adicionados para permitir:
* **WhatsApp:** Envio automático do `CodigoSeguranca` e endereço de retirada.
* **E-mail:** Confirmação detalhada com o resumo dos itens de múltiplos vendedores.

**Como rodar os testes:**
```bash
dotnet test
```

## 🚀 Como Executar

### Pré-requisitos
- .NET SDK 8.0

3. **Rodar a API:**
   ```bash
   dotnet run --project src/Horta.API
   ```

A API estará disponível em: `https://localhost:5001/swagger` (ou a porta definida no seu launchSettings.json).

---
  Desenvolvido por [Guilherme](https://github.com/AlmostZF)
