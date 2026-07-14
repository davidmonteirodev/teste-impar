# Documentação Semântica — VehicleCRM

Documento criado para orientar um agente de IA no entendimento do sistema, dos requisitos, das premissas, das regras de negócio e da evolução do projeto.

---

## Objetivo

Sistema CRM (Customer Relationship Management) para gerenciamento de vendas de veículos, permitindo o controle completo do ciclo de vendas desde o cadastro de veículos até a conclusão de oportunidades de venda.

---

## Entidades

### Veículo (Vehicle)

- **Marca** (Brand): Texto livre
- **Modelo** (Model): Texto livre
- **Ano** (Year): Número inteiro
- **Quilometragem** (Mileage): Número inteiro
- **Preço** (Price): Decimal
- **Cor** (Color): Texto livre
- **Status** (Status): Enum VehicleSaleStatus
  - `Available` (Disponível)
  - `Reserved` (Reservado)
  - `Sold` (Vendido)

### Cliente (Customer)

- **Nome** (Name): Texto
- **Email** (Email): Value Object com validação
- **Telefone** (Phone): Value Object com validação
- **Interesse Principal** (MainInterest): Enum CustomerMainInterest
  - `Suv`
  - `Hatch`
  - `Sedan`
  - `Utility`
  - `UsedCar` (Carro usado)
  - `NewCar` (Carro novo)

### Oportunidade de Venda (SaleOpportunity)

- **ClienteId** (CustomerId): Referência ao cliente
- **VeículoId** (VehicleId): Referência ao veículo
- **Status** (Status): Enum SaleOpportunityStatus
  - `NewLead` (Novo lead)
  - `InNegotiation` (Em negociação)
  - `ProposalSent` (Proposta enviada)
  - `Sold` (Vendido)
  - `Lost` (Perdido)
- **Valor Proposto** (ProposedValue): Decimal
- **Observações** (Notes): Texto opcional

---

## Fluxo Principal

```
Cadastrar veículo
	↓
Cadastrar cliente
	↓
Criar oportunidade de venda
	↓
Atualizar status da oportunidade
	(NewLead → InNegotiation → ProposalSent → Sold/Lost)
	↓
Venda concluída
	(Status do veículo atualizado automaticamente)
```

---

## Regras de Negócio

### Veículo

1. Um veículo pode ter **múltiplas oportunidades** de venda associadas a ele
2. Veículos com status `Sold` **não podem ser editados**
3. Um veículo **não pode ser excluído** se possuir oportunidades de venda associadas
4. Um veículo sempre é criado com status `Available`
5. O status do veículo é atualizado automaticamente com base nas oportunidades ativas

### Cliente

1. O **email deve ser único** no sistema
2. Um cliente pode ter **múltiplas oportunidades** de venda
3. Um cliente **não pode ser excluído** se possuir oportunidades de venda associadas
4. O email **não pode ser alterado** após cadastrado (validação no domínio)

### Oportunidade de Venda

1. Cada oportunidade pertence a **um único cliente** e **um único veículo**
2. Oportunidades finalizadas (`Sold` ou `Lost`) **não podem ser editadas**
3. Oportunidades finalizadas **não podem ser excluídas**
4. **Não é permitido ter duas oportunidades ativas** para a mesma combinação de cliente e veículo
5. Um veículo só pode ter **uma oportunidade ativa** por vez

#### Regras de Transição de Status

6. **Não é permitido** voltar de `InNegotiation` para `NewLead`
7. **Não é permitido** voltar de `ProposalSent` para `NewLead` ou `InNegotiation`
8. Estados finais (`Sold` e `Lost`) são irreversíveis

#### Regras de Edição

9. **Não é permitido trocar o veículo** quando a oportunidade está em `InNegotiation` ou `ProposalSent`
10. **Não é permitido trocar o cliente** quando a oportunidade está em `InNegotiation` ou `ProposalSent`

#### Validações de Veículo

11. O veículo deve estar **disponível** (`Available`) para criar uma nova oportunidade
12. Um veículo **vendido** (`Sold`) não pode ter novas oportunidades criadas
13. Um veículo com **oportunidade ativa** não pode ser usado em nova oportunidade

---

## Premissas

### Dados

- **Marca e Cor** são campos de texto livre (sem cadastro pré-definido)
- **Não existe cadastro separado** de marcas de veículos
- **Não existe cadastro separado** de modelos de veículos

### Segurança

- **Não existe autenticação** ou autorização implementada
- Sistema é de acesso público sem controle de usuários

### Escopo

- Sistema focado **exclusivamente** no gerenciamento de veículos, clientes e oportunidades
- **Não há sistema de upload** de imagens de veículos
- **Não há sistema de relatórios** ou exportação de dados
- **Não há sistema de notificações**

---

## Arquitetura

### Stack Tecnológica

```
Frontend (React) 
	↓ HTTP/REST
Backend API (.NET 10)
	↓ Entity Framework Core
SQL Server
```

### Arquitetura Backend (Clean Architecture / DDD)

```
VehicleCRM.API (Controllers, DTOs, Mappings)
	↓
VehicleCRM.Application (Use Cases, Validations, Services)
	↓
VehicleCRM.Domain (Entities, Value Objects, Domain Services, Exceptions)
	↓
VehicleCRM.Infrastructure (Repositories, EF Core, Migrations)
```

**VehicleCRM.IoC**: Injeção de Dependências e configuração de serviços

### Padrões Utilizados

- **Repository Pattern**: Acesso a dados abstraído
- **Unit of Work**: Transações coordenadas
- **Domain-Driven Design**: Entidades ricas, Value Objects, Domain Services
- **CQRS implícito**: Separação de comandos (Create, Update, Delete) e queries (Get, Search)
- **Specification Pattern**: `SearchCriteria` para filtros complexos

---

## Estrutura de Projetos

### VehicleCRM.Domain

Contém as regras de negócio puras:
- **Entities**: `Vehicle`, `Customer`, `SaleOpportunity`
- **Value Objects**: `Email`, `Phone`
- **Enums**: `VehicleSaleStatus`, `CustomerMainInterest`, `SaleOpportunityStatus`
- **Domain Services**: `SaleOpportunityDomainService`
- **Exceptions**: Exceções específicas de domínio
- **Repositories (interfaces)**: Contratos para acesso a dados

### VehicleCRM.Application

Contém os casos de uso:
- **Services**: Orquestração de operações
- **DTOs**: Objetos de transferência de dados
- **Validators**: Validações de entrada (FluentValidation)
- **Mappings**: Conversões entre entidades e DTOs

### VehicleCRM.Infrastructure

Implementação técnica:
- **Repositories**: Implementação de acesso a dados
- **EF Core Configurations**: Mapeamento objeto-relacional
- **Migrations**: Versionamento do banco de dados
- **Seed Data**: Dados iniciais para desenvolvimento

### VehicleCRM.API

Ponto de entrada da aplicação:
- **Controllers**: Endpoints REST
- **Middlewares**: Tratamento de erros, CORS
- **Swagger**: Documentação interativa da API

### VehicleCRM.Test

Testes das camadas Application e Domain:
- Testes de unidade

---

## Endpoints Principais

### Veículos
- `GET /api/vehicles` - Listar com filtros
- `GET /api/vehicles/{id}` - Buscar por ID
- `POST /api/vehicles` - Criar
- `PUT /api/vehicles/{id}` - Atualizar
- `DELETE /api/vehicles/{id}` - Excluir

### Clientes
- `GET /api/customers` - Listar com filtros
- `GET /api/customers/{id}` - Buscar por ID
- `POST /api/customers` - Criar
- `PUT /api/customers/{id}` - Atualizar
- `DELETE /api/customers/{id}` - Excluir

### Oportunidades
- `GET /api/sale-opportunities` - Listar com filtros
- `GET /api/sale-opportunities/{id}` - Buscar por ID
- `POST /api/sale-opportunities` - Criar
- `PUT /api/sale-opportunities/{id}` - Atualizar
- `DELETE /api/sale-opportunities/{id}` - Excluir

---

## Validações e Exceções

### Domain Exceptions

O sistema utiliza exceções tipadas para comunicar erros de negócio:

- `VehicleCannotBeEditedException`: Tentativa de editar veículo vendido
- `VehicleHasSaleOpportunitiesException`: Tentativa de excluir veículo com oportunidades
- `VehicleNotAvailableException`: Tentativa de criar oportunidade com veículo indisponível
- `CustomerHasSaleOpportunitiesException`: Tentativa de excluir cliente com oportunidades
- `DuplicateCustomerEmailException`: Email duplicado
- `DuplicateSaleOpportunityException`: Oportunidade duplicada (mesmo cliente + veículo)
- `CannotDeleteFinalizedOpportunityException`: Tentativa de excluir oportunidade finalizada
- `VehicleInActiveOpportunityException`: Veículo já possui oportunidade ativa

### FluentValidation

Validações de entrada nos DTOs:
- Campos obrigatórios
- Formatos (email, telefone)
- Ranges (ano do veículo, preços)
- Tamanhos de string

---

## Containerização

Sistema totalmente containerizado com Docker:

### Serviços

- **backend**: API .NET 10 (porta 5000)
- **frontend**: React (porta 3000)
- **sqlserver**: SQL Server 2022 (porta 1433)

### Inicialização Automática

1. Migrations são executadas automaticamente na startup do backend
2. Seed data é aplicado automaticamente
3. Não requer configuração manual do banco de dados

---

## Melhorias Futuras Planejadas

### Funcionalidades

- [ ] Cadastro de Marcas de Veículos
- [ ] Cadastro de Modelos de Veículos (relacionado à marca)
- [ ] Sistema de Autenticação JWT
- [ ] Upload e gerenciamento de imagens de veículos
- [ ] Sistema de Logs e Auditoria
- [ ] Exportação de relatórios (PDF/Excel)
- [ ] Sistema de Notificações
- [ ] Multi-tenancy (suporte a múltiplas empresas)

### Técnicas

- [ ] Implementação de CQRS completo com MediatR
- [ ] Event Sourcing para histórico de alterações
- [ ] Cache distribuído (Redis) se houver necessidade
- [ ] Mensageria (RabbitMQ/Azure Service Bus) se houver necessidade
- [ ] Observabilidade completa (OpenTelemetry, Prometheus, Grafana)

---

## Convenções de Código

### Nomenclatura

- **Entidades**: Singular e PascalCase (`Vehicle`, não `Vehicles`)
- **Métodos de fábrica**: `Create` para construtores públicos
- **Métodos de validação**: Prefixo `Ensure` para validações com exceção
- **Métodos booleanos**: Prefixo `Is`, `Has`, `Can` para consultas
- **Repositórios**: Interface `I{Entity}Repository`, implementação `{Entity}Repository`

### Organização

- **Uma classe por arquivo**
- **Namespaces refletem a estrutura de pastas**
- **Dependências sempre de fora para dentro** (API → Application → Domain)
- **Domain nunca depende de Infrastructure**

### Value Objects

- Imutáveis (apenas getters)
- Validação no construtor
- Método estático `Create` para criação

### Entidades

- Construtor protegido (para EF Core)
- Construtor privado com validações
- Método estático `Create` para criação pública
- Propriedades virtuais (para EF Core proxies)
- Setters privados (encapsulamento)

---

## Guia de Extensão

### Adicionar Nova Entidade

1. Criar entidade em `VehicleCRM.Domain/{Entity}/Entities`
2. Criar interface de repositório em `VehicleCRM.Domain/{Entity}/Repositories`
3. Criar enums (se necessário) em `VehicleCRM.Domain/{Entity}/Enums`
4. Criar exceções (se necessário) em `VehicleCRM.Domain/{Entity}/Exceptions`
5. Implementar repositório em `VehicleCRM.Infrastructure/Repositories`
6. Criar configuração EF Core em `VehicleCRM.Infrastructure/Configurations`
7. Adicionar DbSet em `ApplicationDbContext`
8. Criar migration: `dotnet ef migrations add Add{Entity}`
9. Criar DTOs em `VehicleCRM.Application/{Entity}/DTOs`
10. Criar serviço em `VehicleCRM.Application/{Entity}/Services`
11. Criar controller em `VehicleCRM.API/Controllers`
12. Registrar serviços no `DependencyInjection`

### Adicionar Nova Regra de Negócio

1. Implementar no método da entidade (se envolve apenas ela)
2. Criar Domain Service (se envolve múltiplas entidades ou repositórios)
3. Lançar exceção de domínio tipada para violações
4. Adicionar validação na Application Layer se necessário
5. Atualizar testes

---

## Contato e Suporte

Desenvolvido como parte do Desafio Ímpar por David Monteiro.

Para dúvidas sobre o projeto:
- Consulte a documentação interativa em `/swagger`
- Revise os testes em `VehicleCRM.Test`
- Consulte o README.md principal do projeto
