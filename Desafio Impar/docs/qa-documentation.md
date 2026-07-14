# QA Documentation - VehicleCRM System

## 📋 Objetivo

Validar todas as funcionalidades do sistema VehicleCRM, garantindo a qualidade, confiabilidade e conformidade com os requisitos de negócio. Esta documentação serve como guia para agentes de IA executarem testes de forma autônoma e sistemática.

---

## ⚠️ IMPORTANTE: Tipos de Testes Disponíveis

```
┌─────────────────────────────────────────────────────────────────┐
│                     TESTES UNITÁRIOS                             │
│  ✅ xUnit + NSubstitute + FluentAssertions                      │
│                                                                  │
│  O que é testado:                                               │
│     • Lógica de domínio (Entities, Value Objects, Exceptions)   │
│     • Handlers de comandos (com mocks)                          │
│     • Validators (FluentValidation)                             │
│                                                                  │
│  Como executar:                                                 │
│     dotnet test                                                 │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                TESTES MANUAIS (Responsabilidade do QA)           │
│  ⚠️ NÃO possuem código de teste - executar manualmente         │
│                                                                  │
│  O que deve ser testado:                                        │
│     • API Endpoints (via Swagger/Postman/Insomnia)              │
│     • Persistência no Banco de Dados (via SQL Client)           │
│     • Integridade Referencial                                   │
│     • Transações e Rollbacks                                    │
│     • Docker/Infraestrutura                                     │
│                                                                  │
│  Ferramentas necessárias:                                       │
│     Postman, Swagger UI, SSMS, Azure Data Studio, Docker        │
└─────────────────────────────────────────────────────────────────┘
```

> **🚨 Atenção Agentes de IA**: Este projeto **NÃO possui testes de integração**.  
> Para validar o sistema completamente:
> 1. ✅ Execute `dotnet test` para validar lógica de negócio
> 2. ⚠️ **Instrua o QA** a executar testes manuais de API e persistência (veja seção "Testes Manuais" abaixo)
> 3. 📝 Documente os resultados dos testes manuais

---

## 🎯 Estratégia de Qualidade

### Tipos de Testes no Projeto

#### ✅ Testes Unitários
Validação isolada de lógica de negócio usando mocks (NSubstitute):
  - Entidades (Vehicle, Customer, SaleOpportunity)
  - Value Objects (Email, Phone)
  - Exceções de domínio
  - Handlers de comandos (usando mocks/NSubstitute)
  - Validators (FluentValidation)
  - Serviços de domínio

#### ⚠️ Testes Manuais (Responsabilidade do QA)
O projeto não possui código de teste para estas áreas. O QA deve testar manualmente:
- **API Endpoints**: Validação dos controllers e respostas HTTP (via Swagger/Postman)
- **Persistência de Dados**: Verificação de gravação/leitura no banco real (via SQL Client)
- **Integridade Referencial**: Validação de relacionamentos entre entidades
- **Transações**: Verificação de commits e rollbacks
- **Docker/Infraestrutura**: Validação de deployment e configurações

> **📝 Nota**: O projeto **NÃO possui testes de integração**.  
> Para testar API e banco de dados, o QA deve **executar a aplicação** (localmente ou via Docker)  
> e usar ferramentas como Postman, Swagger UI e SQL Client.

### Ferramentas Utilizadas
- **xUnit**: Framework de testes unitários
- **NSubstitute**: Biblioteca para criação de mocks e stubs
- **FluentAssertions**: Asserções fluentes e legíveis
- **.NET 10**: Plataforma de execução

### Critérios de Aceitação

**Testes Unitários:**
- ✅ Todos devem passar com sucesso
- ✅ Cobertura mínima de código: 80%
- ✅ Zero erros de compilação
- ✅ Zero avisos críticos

**Testes Manuais:**
- ✅ Todos os endpoints testados via Postman/Swagger
- ✅ Persistência validada via SQL Client
- ✅ Resultados documentados

---

## 🧪 Casos de Teste

### 🚗 Veículos (Vehicles)

#### Operações CRUD
- ✓ **Criar veículo** - `POST /api/vehicles`
  - Com dados válidos
  - Com diferentes status (Disponível, Reservado, Vendido)
  - Validar persistência no banco

- ✓ **Editar veículo** - `PUT /api/vehicles/{id}`
  - Atualizar marca, modelo, ano, preço
  - Verificar regras de edição por status
  - Validar que veículos vendidos não podem ser editados
  - Validar que veículos com oportunidades ativas têm restrições

- ✓ **Excluir veículo** - `DELETE /api/vehicles/{id}`
  - Excluir veículo disponível
  - Validar que veículos com oportunidades não podem ser excluídos
  - Verificar remoção física do banco

- ✓ **Listar veículos** - `GET /api/vehicles`
  - Listar todos os veículos
  - Filtrar por status
  - Filtrar por critérios de busca
  - Validar paginação
  - Verificar ordenação

- ✓ **Buscar veículo por ID** - `GET /api/vehicles/{id}`
  - Buscar veículo existente
  - Buscar veículo inexistente (404)

#### Regras de Negócio
- ✓ Veículo vendido não pode ser editado
- ✓ Veículo com oportunidades ativas não pode ser excluído
- ✓ Status do veículo deve ser válido (Disponível, Reservado, Vendido)
- ✓ Preço deve ser maior que zero
- ✓ Ano de fabricação deve ser válido

#### Exceções Específicas
- ✓ `VehicleCannotBeEditedException` - Tentativa de editar veículo vendido
- ✓ `VehicleHasSaleOpportunitiesException` - Tentativa de excluir veículo com oportunidades
- ✓ `VehicleNotAvailableException` - Tentativa de usar veículo indisponível
- ✓ `VehicleSoldException` - Operações inválidas em veículo vendido

---

### 👥 Clientes (Customers)

#### Operações CRUD
- ✓ **Criar cliente** - `POST /api/customers`
  - Com dados válidos
  - Com diferentes interesses principais
  - Validar email único
  - Validar formato de telefone

- ✓ **Editar cliente** - `PUT /api/customers/{id}`
  - Atualizar nome, email, telefone
  - Atualizar interesse principal
  - Validar unicidade de email
  - Verificar validação de email

- ✓ **Excluir cliente** - `DELETE /api/customers/{id}`
  - Excluir cliente sem oportunidades
  - Validar que clientes com oportunidades não podem ser excluídos
  - Verificar remoção física do banco

- ✓ **Listar clientes** - `GET /api/customers`
  - Listar todos os clientes
  - Filtrar por critérios de busca
  - Validar paginação
  - Verificar ordenação

- ✓ **Buscar cliente por ID** - `GET /api/customers/{id}`
  - Buscar cliente existente
  - Buscar cliente inexistente (404)

#### Regras de Negócio
- ✓ Email deve ser único no sistema
- ✓ Email deve ter formato válido
- ✓ Telefone deve ter formato válido
- ✓ Nome é obrigatório
- ✓ Cliente com oportunidades não pode ser excluído

#### Value Objects
- ✓ **Email** - Validação de formato
- ✓ **Phone** - Validação de formato brasileiro

#### Exceções Específicas
- ✓ `CustomerHasSaleOpportunitiesException` - Tentativa de excluir cliente com oportunidades
- ✓ `DuplicateCustomerEmailException` - Tentativa de usar email duplicado

---

### 💼 Oportunidades de Venda (SaleOpportunities)

#### Operações CRUD
- ✓ **Criar oportunidade** - `POST /api/saleopportunities`
  - Com dados válidos
  - Validar associação com cliente existente
  - Validar associação com veículo disponível
  - Verificar status inicial (Aberta)
  - Validar unicidade (cliente + veículo)

- ✓ **Editar oportunidade** - `PUT /api/saleopportunities/{id}`
  - Atualizar status (Aberta, Negociando, Concluída, Perdida)
  - Atualizar observações
  - Validar transições de status

- ✓ **Excluir oportunidade** - `DELETE /api/saleopportunities/{id}`
  - Excluir oportunidade em aberto
  - Validar que oportunidades finalizadas não podem ser excluídas
  - Verificar remoção física do banco

- ✓ **Listar oportunidades** - `GET /api/saleopportunities`
  - Listar todas as oportunidades
  - Filtrar por status
  - Filtrar por cliente
  - Filtrar por veículo
  - Validar paginação
  - Verificar ordenação

- ✓ **Buscar oportunidade por ID** - `GET /api/saleopportunities/{id}`
  - Buscar oportunidade existente
  - Buscar oportunidade inexistente (404)

#### Regras de Negócio
- ✓ Veículo deve estar disponível para nova oportunidade
- ✓ Não pode haver oportunidades duplicadas (mesmo cliente + veículo ativo)
- ✓ Oportunidades finalizadas (Concluída/Perdida) não podem ser excluídas
- ✓ Status deve ser válido (Aberta, Negociando, Concluída, Perdida)
- ✓ Cliente e veículo devem existir

#### Exceções Específicas
- ✓ `CannotDeleteFinalizedOpportunityException` - Tentativa de excluir oportunidade finalizada
- ✓ `DuplicateSaleOpportunityException` - Tentativa de criar oportunidade duplicada
- ✓ `VehicleInActiveOpportunityException` - Veículo já possui oportunidade ativa

---

### 📊 Dashboard

#### Métricas e KPIs
- ✓ **Quantidade de veículos** - `GET /api/dashboard`
  - Total de veículos cadastrados
  - Verificar contagem precisa

- ✓ **Quantidade de clientes** - `GET /api/dashboard`
  - Total de clientes cadastrados
  - Verificar contagem precisa

- ✓ **Quantidade de oportunidades** - `GET /api/dashboard`
  - Total de oportunidades cadastradas
  - Verificar contagem precisa

- ✓ **Valor total de veículos vendidos** - `GET /api/dashboard`
  - Soma dos valores de veículos vendidos
  - Verificar cálculo correto

- ✓ **Status de veículos** - `GET /api/dashboard`
  - Contagem por status (Disponível, Reservado, Vendido)
  - Verificar distribuição correta

- ✓ **Status de oportunidades** - `GET /api/dashboard`
  - Contagem por status (Aberta, Negociando, Concluída, Perdida)
  - Verificar distribuição correta

#### Resposta Esperada
```json
{
  "cards": {
	"vehicles": 0,
	"customers": 0,
	"opportunities": 0,
	"soldVehiclesTotalValue": 0.0
  },
  "vehicleStatus": [
	{ "status": "Disponível", "count": 0 },
	{ "status": "Reservado", "count": 0 },
	{ "status": "Vendido", "count": 0 }
  ],
  "opportunityStatus": [
	{ "status": "Aberta", "count": 0 },
	{ "status": "Negociando", "count": 0 },
	{ "status": "Concluída", "count": 0 },
	{ "status": "Perdida", "count": 0 }
  ]
}
```

---

## ✅ Validações

### Campos Obrigatórios

#### Veículos
- ✓ Marca (Brand) - não pode ser vazio
- ✓ Modelo (Model) - não pode ser vazio
- ✓ Ano de fabricação (ManufactureYear) - deve ser válido
- ✓ Preço (Price) - deve ser maior que zero
- ✓ Status de venda (SaleStatus) - deve ser válido

#### Clientes
- ✓ Nome (Name) - não pode ser vazio
- ✓ Email - não pode ser vazio e deve ter formato válido
- ✓ Telefone (Phone) - não pode ser vazio e deve ter formato válido
- ✓ Interesse principal (MainInterest) - deve ser válido

#### Oportunidades
- ✓ ID do cliente (CustomerId) - deve existir
- ✓ ID do veículo (VehicleId) - deve existir
- ✓ Status - deve ser válido
- ✓ Data de criação - deve ser preenchida automaticamente

### Valores Inválidos

#### Validação de Email
- ✓ Email sem @ deve falhar
- ✓ Email sem domínio deve falhar
- ✓ Email com formato inválido deve falhar
- ✓ Email duplicado deve falhar

#### Validação de Telefone
- ✓ Telefone com menos de 10 dígitos deve falhar
- ✓ Telefone com mais de 11 dígitos deve falhar
- ✓ Telefone com caracteres inválidos deve falhar
- ✓ Telefone sem DDD deve falhar

#### Validação de Preço
- ✓ Preço zero deve falhar
- ✓ Preço negativo deve falhar
- ✓ Preço com mais de 2 casas decimais deve ser arredondado

#### Validação de Ano
- ✓ Ano menor que 1900 deve falhar
- ✓ Ano maior que ano atual + 1 deve falhar

### Erros da API

#### Códigos HTTP Esperados
- ✓ **200 OK** - Operação bem-sucedida (GET, PUT)
- ✓ **201 Created** - Recurso criado com sucesso (POST)
- ✓ **204 No Content** - Recurso excluído com sucesso (DELETE)
- ✓ **400 Bad Request** - Dados inválidos ou validação falhou
- ✓ **404 Not Found** - Recurso não encontrado
- ✓ **500 Internal Server Error** - Erro do servidor

#### Middleware de Exceções
- ✓ `GlobalExceptionHandler` - captura e formata todas as exceções
- ✓ Exceções de domínio retornam detalhes apropriados
- ✓ Exceções de validação retornam campos inválidos
- ✓ Exceções não tratadas retornam erro genérico (sem detalhes internos)

---

## 🗄️ Banco de Dados

### Persistência

#### Testes de Gravação
- ✓ **Criar** - Inserir registro no banco e verificar ID gerado
- ✓ **Atualizar** - Modificar registro existente e verificar mudanças
- ✓ **Excluir** - Remover registro e verificar ausência
- ✓ **Buscar** - Recuperar registro por ID e verificar dados

#### Integridade Referencial
- ✓ Cliente referenciado por oportunidade não pode ser excluído
- ✓ Veículo referenciado por oportunidade não pode ser excluído
- ✓ Exclusão de oportunidade não afeta cliente ou veículo
- ✓ Chaves estrangeiras são validadas

#### Transações (UnitOfWork)
- ✓ Operações com sucesso são commitadas
- ✓ Operações com erro são revertidas (rollback)
- ✓ Múltiplas operações em uma transação são atômicas

#### Índices e Performance
- ✓ Email de cliente tem índice único
- ✓ Consultas por status são otimizadas
- ✓ Paginação funciona corretamente

---

## 🐳 Docker

### Docker Compose
- ✓ **Comando de build** - `docker-compose up --build`
  - Construir todas as imagens
  - Iniciar todos os serviços (API + Banco)
  - Verificar logs de inicialização
  - Confirmar que API está respondendo

- ✓ **Health Check**
  - API está acessível na porta configurada
  - Banco de dados está acessível
  - Migrações foram aplicadas

- ✓ **Volumes**
  - Dados do banco persistem entre restarts
  - Logs são acessíveis

- ✓ **Network**
  - Contêineres conseguem se comunicar
  - API consegue acessar banco de dados

### Dockerfile
- ✓ Build multi-stage funciona
- ✓ Imagem final é otimizada (tamanho reduzido)
- ✓ Dependências são restauradas corretamente
- ✓ Aplicação inicia sem erros

---

## 🧪 Execução dos Testes

### 1️⃣ Testes Automatizados (Unitários)

#### Comandos para Agentes de IA

**Executar Todos os Testes Unitários**
```bash
dotnet test
```

#### Executar Testes de um Projeto Específico
```bash
dotnet test tests/VehicleCRM.Test/VehicleCRM.Test.csproj
```

#### Executar com Cobertura de Código
```bash
dotnet test --collect:"XPlat Code Coverage"
```

#### Executar Testes de uma Categoria
```bash
# Testes de Domínio
dotnet test --filter "FullyQualifiedName~VehicleCRM.Test.Domain"

# Testes de Application
dotnet test --filter "FullyQualifiedName~VehicleCRM.Test.Application"
```

#### Executar Teste Específico
```bash
dotnet test --filter "FullyQualifiedName~CreateVehicleCommandHandlerTests"
```

#### Visualizar Resultados
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Localização dos Testes

#### Testes de Domínio
- `tests/VehicleCRM.Test/Domain/Vehicles/` - Testes de entidades e exceções de veículos
- `tests/VehicleCRM.Test/Domain/Customers/` - Testes de entidades e value objects de clientes
- `tests/VehicleCRM.Test/Domain/SaleOpportunities/` - Testes de entidades e serviços de oportunidades

#### Testes de Application
- `tests/VehicleCRM.Test/Application/Features/Vehicles/` - Testes de comandos de veículos
- `tests/VehicleCRM.Test/Application/Features/Customers/` - Testes de comandos de clientes
- `tests/VehicleCRM.Test/Application/Features/SaleOpportunities/` - Testes de comandos de oportunidades

---

### 2️⃣ Testes Manuais (QA Manual)

> **⚠️ IMPORTANTE**: Como o projeto não possui testes de integração automatizados, 
> os testes de API, persistência e infraestrutura devem ser executados **MANUALMENTE**.

#### Preparação do Ambiente

**Opção 1: Executar Localmente**
```bash
# Navegar até o diretório da API
cd src/VehicleCRM.API

# Configurar connection string no appsettings.Development.json
# Executar a aplicação
dotnet run
```

**Opção 2: Executar via Docker** (se disponível)
```bash
# Na raiz do projeto (onde está o docker-compose.yml)
docker-compose up --build

# Aguardar API inicializar
# API estará disponível em: http://localhost:porta
```

#### Ferramentas Recomendadas para Testes Manuais

1. **Postman** ou **Insomnia**: Para testar endpoints da API
2. **Swagger UI**: Interface web automática (disponível em `/swagger`)
3. **SQL Client**: Para validar dados no banco (SSMS, Azure Data Studio, DBeaver)
4. **Docker Desktop**: Para monitorar containers (se usar Docker)

#### Roteiro de Testes Manuais de API

##### 🚗 Veículos

**1. Criar Veículo**
```http
POST http://localhost:5000/api/vehicles
Content-Type: application/json

{
  "brand": "Toyota",
  "model": "Corolla",
  "year": 2023,
  "price": 85000,
  "color": "Branco",
  "mileage": 15000
}
```
✅ **Validar**: Status 201, resposta contém ID gerado

**2. Listar Veículos**
```http
GET http://localhost:5000/api/vehicles
```
✅ **Validar**: Status 200, lista contém o veículo criado

**3. Buscar Veículo por ID**
```http
GET http://localhost:5000/api/vehicles/{id}
```
✅ **Validar**: Status 200, dados correspondem ao veículo criado

**4. Atualizar Veículo**
```http
PUT http://localhost:5000/api/vehicles/{id}
Content-Type: application/json

{
  "brand": "Toyota",
  "model": "Corolla XEI",
  "year": 2023,
  "price": 90000,
  "color": "Branco",
  "mileage": 15000
}
```
✅ **Validar**: Status 200, dados foram atualizados

**5. Excluir Veículo**
```http
DELETE http://localhost:5000/api/vehicles/{id}
```
✅ **Validar**: Status 204, veículo não aparece mais na listagem

##### 👥 Clientes

**1. Criar Cliente**
```http
POST http://localhost:5000/api/customers
Content-Type: application/json

{
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "phone": "11987654321",
  "mainInterest": "Sedan"
}
```
✅ **Validar**: Status 201, resposta contém ID gerado

**2. Listar Clientes**
```http
GET http://localhost:5000/api/customers
```
✅ **Validar**: Status 200, lista contém o cliente criado

**3. Atualizar Cliente**
```http
PUT http://localhost:5000/api/customers/{id}
Content-Type: application/json

{
  "name": "João Silva Santos",
  "email": "joao.silva@email.com",
  "phone": "11999887766",
  "mainInterest": "SUV"
}
```
✅ **Validar**: Status 200, dados foram atualizados

**4. Excluir Cliente**
```http
DELETE http://localhost:5000/api/customers/{id}
```
✅ **Validar**: Status 204, cliente não aparece mais na listagem

##### 💼 Oportunidades

**1. Criar Oportunidade**
```http
POST http://localhost:5000/api/saleopportunities
Content-Type: application/json

{
  "customerId": 1,
  "vehicleId": 1,
  "notes": "Cliente interessado em financiamento"
}
```
✅ **Validar**: Status 201, resposta contém ID gerado

**2. Listar Oportunidades**
```http
GET http://localhost:5000/api/saleopportunities
```
✅ **Validar**: Status 200, lista contém a oportunidade criada

**3. Atualizar Status da Oportunidade**
```http
PUT http://localhost:5000/api/saleopportunities/{id}
Content-Type: application/json

{
  "status": "Concluída",
  "notes": "Venda finalizada com sucesso"
}
```
✅ **Validar**: Status 200, status foi atualizado

**4. Excluir Oportunidade**
```http
DELETE http://localhost:5000/api/saleopportunities/{id}
```
✅ **Validar**: Status 204 ou 400 (se finalizada)

##### 📊 Dashboard

**1. Buscar Métricas**
```http
GET http://localhost:5000/api/dashboard
```
✅ **Validar**: 
- Status 200
- Contadores refletem os dados criados
- Totais estão corretos

#### Roteiro de Testes de Persistência (Banco de Dados)

**Ferramentas necessárias**: SQL Server Management Studio, Azure Data Studio ou similar

**1. Validar Inserção de Veículo**
```sql
-- Após criar veículo via API
SELECT * FROM Vehicles ORDER BY Id DESC;
```
✅ **Validar**: Registro existe com todos os campos preenchidos

**2. Validar Atualização de Cliente**
```sql
-- Antes e depois de atualizar via API
SELECT * FROM Customers WHERE Id = {id};
```
✅ **Validar**: Campos foram modificados corretamente

**3. Validar Exclusão de Oportunidade**
```sql
-- Após excluir via API
SELECT * FROM SaleOpportunities WHERE Id = {id};
```
✅ **Validar**: Registro não existe mais (exclusão física)

**4. Validar Integridade Referencial**
```sql
-- Tentar excluir cliente com oportunidades
-- Via API: DELETE /api/customers/{id}
-- Verificar que retorna erro 400/422

-- Confirmar que cliente ainda existe
SELECT * FROM Customers WHERE Id = {id};
```
✅ **Validar**: Cliente não foi excluído

**5. Validar Índice Único de Email**
```sql
-- Tentar criar dois clientes com mesmo email via API
-- Verificar que segundo retorna erro 409 Conflict

-- Confirmar que existe apenas um registro
SELECT * FROM Customers WHERE Email = 'teste@email.com';
```
✅ **Validar**: Apenas um registro existe

**6. Validar Relacionamentos**
```sql
-- Buscar oportunidade com dados relacionados
SELECT 
    so.Id,
    so.Status,
    c.Name AS CustomerName,
    v.Brand + ' ' + v.Model AS VehicleName
FROM SaleOpportunities so
INNER JOIN Customers c ON so.CustomerId = c.Id
INNER JOIN Vehicles v ON so.VehicleId = v.Id;
```
✅ **Validar**: JOINs retornam dados corretos

#### Checklist de Validação Manual

**API Endpoints**
- [ ] Todos os endpoints CREATE retornam 201
- [ ] Todos os endpoints UPDATE retornam 200
- [ ] Todos os endpoints DELETE retornam 204
- [ ] Todos os endpoints GET retornam 200
- [ ] Endpoints retornam 404 para recursos inexistentes
- [ ] Endpoints retornam 400 para dados inválidos

**Persistência**
- [ ] Dados são gravados corretamente no banco
- [ ] Atualizações modificam os registros existentes
- [ ] Exclusões removem os registros
- [ ] Relacionamentos são mantidos
- [ ] Índices únicos são respeitados

**Regras de Negócio**
- [ ] Não é possível excluir cliente com oportunidades
- [ ] Não é possível excluir veículo com oportunidades
- [ ] Não é possível editar veículo vendido
- [ ] Não é possível criar oportunidade duplicada
- [ ] Email deve ser único

**Exceções e Erros**
- [ ] Exceções de domínio retornam códigos HTTP apropriados
- [ ] Mensagens de erro são claras e úteis
- [ ] Validações são executadas antes da persistência

---

## 📝 Checklist de Entrega

Antes de considerar a entrega completa, verificar:

### ✅ Testes Automatizados
- [ ] Todos os testes unitários passam
- [ ] Cobertura de código >= 80%
- [ ] Zero warnings de compilação

### ✅ Testes Manuais - Funcionalidade
- [ ] Todas as operações CRUD foram testadas manualmente
- [ ] Todas as validações estão funcionando
- [ ] Todas as regras de negócio são respeitadas
- [ ] Dashboard retorna métricas corretas
- [ ] Exceções são tratadas adequadamente

### ✅ Qualidade de Código
- [ ] Todos os testes unitários passam
- [ ] Cobertura de código >= 80%
- [ ] Sem warnings de compilação
- [ ] Código segue padrões do projeto

### ✅ Persistência
- [ ] Dados são gravados corretamente
- [ ] Transações funcionam (commit/rollback)
- [ ] Integridade referencial é mantida
- [ ] Índices únicos funcionam

### ✅ API
- [ ] Todos os endpoints respondem corretamente
- [ ] Códigos HTTP são apropriados
- [ ] Middleware de exceções funciona
- [ ] Documentação (Swagger) está atualizada

### ✅ Infraestrutura
- [ ] Build do projeto funciona
- [ ] Docker Compose sobe corretamente
- [ ] Health checks passam
- [ ] Logs são adequados

---

## 🤖 Instruções para Agentes de IA

### Fluxo de Execução Recomendado

1. **Análise Inicial**
   - Ler esta documentação completamente
   - Identificar áreas de teste relevantes
   - Verificar estrutura do projeto

2. **Preparação do Ambiente**
   - Restaurar dependências: `dotnet restore`
   - Compilar solução: `dotnet build`
   - Verificar ausência de erros

3. **Execução de Testes**
   - Executar todos os testes: `dotnet test`
   - Analisar resultados
   - Identificar falhas

4. **Análise de Falhas**
   - Para cada teste falhando:
	 - Ler o erro detalhado
	 - Identificar a causa raiz
	 - Verificar código relacionado
	 - Propor correção

5. **Validação de Cobertura**
   - Executar com cobertura
   - Identificar gaps
   - Criar testes adicionais se necessário

6. **Testes de Integração**
   - Subir aplicação localmente ou via Docker
   - Testar endpoints via HTTP
   - Validar respostas e códigos de status

7. **Relatório Final**
   - Consolidar resultados
   - Documentar problemas encontrados
   - Propor melhorias

### Padrões de Teste

#### Nomenclatura
```csharp
// Padrão: MethodName_StateUnderTest_ExpectedBehavior
public void CreateVehicle_WithValidData_ShouldCreateSuccessfully()
public void CreateVehicle_WithInvalidPrice_ShouldThrowValidationException()
```

#### Estrutura (AAA Pattern)
```csharp
// Arrange - Preparar dados e mocks (NSubstitute)
var vehicle = new Vehicle(...);
var mockRepo = Substitute.For<IVehicleRepository>();

// Act - Executar ação
var result = await handler.Handle(command);

// Assert - Verificar resultado
result.Should().NotBeNull();
result.Id.Should().BePositive();
```

#### FluentAssertions
```csharp
// Usar asserções fluentes
result.Should().NotBeNull();
result.Should().BeOfType<VehicleResponse>();
result.Id.Should().BePositive();
result.Brand.Should().Be("Toyota");

// Para exceções
await action.Should().ThrowAsync<DomainException>();
```

#### Mocking com NSubstitute
```csharp
// Criar mock
var mockRepo = Substitute.For<IVehicleRepository>();

// Configurar comportamento
mockRepo
    .InsertAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>())
    .Returns(Task.CompletedTask);

// Verificar chamada
await mockRepo.Received(1).InsertAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>());
```

### Comandos Úteis

```bash
# Limpar e rebuild
dotnet clean
dotnet build

# Executar testes com log detalhado
dotnet test --logger "console;verbosity=detailed"

# Ver sumário de testes
dotnet test --logger "console;verbosity=normal"

# Executar apenas testes falhando
dotnet test --filter "TestCategory=Failed"

# Parar na primeira falha
dotnet test -t --abort-on-first-error
```

---

## 📊 Matriz de Cobertura Atual

> **📝 Nota**: Esta matriz reflete apenas os **testes automatizados (unitários)** existentes.  
> Testes de API e persistência devem ser executados **manualmente** pelo QA.

| Módulo | Funcionalidade | Testes Unitários | Testes Manuais Necessários | Status |
|--------|----------------|------------------|----------------------------|--------|
| **Vehicles** | Create (Handler) | ✅ | API + Persistência | Unitário OK |
| **Vehicles** | Update (Handler) | ✅ | API + Persistência | Unitário OK |
| **Vehicles** | Delete (Handler) | ✅ | API + Persistência | Unitário OK |
| **Vehicles** | Validators | ✅ | - | Completo |
| **Vehicles** | Domain Entity | ✅ | - | Completo |
| **Vehicles** | Exceptions | ✅ | - | Completo |
| **Customers** | Create (Handler) | ✅ | API + Persistência | Unitário OK |
| **Customers** | Update (Handler) | ✅ | API + Persistência | Unitário OK |
| **Customers** | Delete (Handler) | ✅ | API + Persistência | Unitário OK |
| **Customers** | Validators | ✅ | - | Completo |
| **Customers** | Domain Entity | ✅ | - | Completo |
| **Customers** | Value Objects | ✅ | - | Completo |
| **Customers** | Exceptions | ✅ | - | Completo |
| **SaleOpportunities** | Create (Handler) | ✅ | API + Persistência | Unitário OK |
| **SaleOpportunities** | Update (Handler) | ✅ | API + Persistência | Unitário OK |
| **SaleOpportunities** | Delete (Handler) | ✅ | API + Persistência | Unitário OK |
| **SaleOpportunities** | Validators | ✅ | - | Completo |
| **SaleOpportunities** | Domain Entity | ✅ | - | Completo |
| **SaleOpportunities** | Domain Service | ✅ | - | Completo |
| **SaleOpportunities** | Exceptions | ✅ | - | Completo |
| **Dashboard** | GetDashboard Query | ❌ | API + Validação Dados | Teste Manual Apenas |
| **API Controllers** | All Endpoints | ❌ | Swagger/Postman | Teste Manual Apenas |
| **Persistence** | Database Operations | ❌ | SQL Client | Teste Manual Apenas |
| **Infrastructure** | Docker/Deployment | ❌ | Docker Commands | Teste Manual Apenas |

**Legenda:**
- ✅ **Completo** - Testes unitários automatizados implementados e passando
- ❌ **Teste Manual Apenas** - Sem testes automatizados, requer execução manual pelo QA
- **Unitário OK** - Lógica está testada (mockada), mas API/persistência real precisam teste manual
- N/A - Não aplicável

---

## 🎯 Métricas de Qualidade

### Objetivos
- **Cobertura de Código**: >= 80%
- **Testes Passando**: 100%
- **Tempo de Execução**: < 2 minutos
- **Código Duplicado**: < 5%
- **Complexidade Ciclomática**: <= 10 por método

### Monitoramento
- Executar testes em CI/CD
- Gerar relatórios de cobertura
- Analisar tendências de qualidade
- Identificar áreas de risco

---

## 📚 Referências

- **xUnit Documentation**: https://xunit.net/
- **FluentAssertions**: https://fluentassertions.com/
- **.NET Testing Best Practices**: https://learn.microsoft.com/en-us/dotnet/core/testing/

---

## 🔄 Histórico de Atualizações

| Data | Versão | Descrição |
|------|--------|-----------|
| 2025 | 1.0 | Versão inicial da documentação QA |

---

**Nota**: Esta documentação deve ser atualizada sempre que novas funcionalidades forem adicionadas ou quando mudanças significativas forem feitas na estratégia de testes.
