# 🚗 VehicleCRM - Sistema de Gerenciamento de Veículos

Sistema CRM (Customer Relationship Management) desenvolvido para gerenciamento de veículos, permitindo cadastro, consulta, atualização e exclusão de informações de veículos de forma eficiente e moderna.

## 🛠️ Stack Tecnológica

- **Backend**: .NET 10
- **Frontend**: React
- **Banco de Dados**: SQL Server
- **Containerização**: Docker & Docker Compose

## 📋 Pré-requisitos

Para executar este projeto em sua máquina, você precisará ter instalado:

- [Docker](https://www.docker.com/products/docker-desktop) (versão 20.10 ou superior)
- [Docker Compose](https://docs.docker.com/compose/install/) (versão 2.0 ou superior)

> **Nota**: O Docker Desktop para Windows e macOS já inclui o Docker Compose. Usuários Linux podem precisar instalá-lo separadamente.

## 🚀 Como Executar

### 1. Configuração do Ambiente

Antes de iniciar a aplicação, é necessário configurar as variáveis de ambiente:
Na pasta que tem o arquivo `docker-compose.yml`, execute o comando abaixo para criar o arquivo `.env` a partir do modelo fornecido:

```bash
# Copie o arquivo de exemplo
cp .env.example .env
```
ou 
```bash
# Copie o arquivo de exemplo
copy .env.example .env
```
### 2. Edite o arquivo `.env`

Abra o arquivo `.env` e altere o campo `DB_PASSWORD` com uma senha forte para o banco de dados:

```env
DB_PASSWORD=SuaSenhaForte123!
```

> **⚠️ Importante**: A senha deve conter pelo menos:
> - 8 caracteres
> - Letras maiúsculas e minúsculas
> - Números
> - Caracteres especiais

### 3. Inicie os Serviços

Execute o comando abaixo na raiz do projeto (onde está o arquivo `docker-compose.yml`):

```bash
docker compose up --build
```

### 4. Aguarde a Inicialização

O Docker Compose irá:
- Baixar as imagens necessárias (primeira execução)
- Criar os containers
- Executar as migrations do banco de dados automaticamente
- Popular o banco com dados iniciais (seed)

Aguarde alguns instantes até que todos os serviços estejam prontos.

## 🌐 Acessando a Aplicação

Após a inicialização, você poderá acessar:

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **Frontend** | [http://localhost:3000](http://localhost:3000) | Interface do usuário |
| **Backend API** | [http://localhost:5000](http://localhost:5000) | API REST |
| **Swagger** | [http://localhost:5000/swagger](http://localhost:5000/swagger) | Documentação interativa da API |

## 🗄️ Banco de Dados

- As **migrations** são executadas automaticamente durante a inicialização do backend
- O **seed** de dados iniciais também é aplicado automaticamente
- Não é necessário nenhum passo manual para configurar o banco de dados

## 📝 Premissas Adotadas

Durante o desenvolvimento deste projeto, foram adotadas as seguintes premissas:

- **Marca e Cor**: Implementadas como campos de texto livre, sem validação de cadastros pré-definidos
- **Sem cadastro de marcas**: Não foi criado um módulo específico para gerenciar marcas de veículos
- **Sem cadastro de modelos**: Não foi criado um módulo específico para gerenciar modelos de veículos
- **Regra de negócio**: Foram adotadas regras de negócio que não estavam no documento, mas de modo a garantir a integridade e consistência dos dados(essas regras podem ser consultadas no documento semantic-documentation.md, que fica na pasta docs)
- **Foco no escopo**: O desenvolvimento priorizou atender aos requisitos solicitados de forma funcional e eficiente

## 🔮 Melhorias Futuras

Funcionalidades e melhorias planejadas para versões futuras:

- [ ] **Cadastro de Marcas**: Sistema completo de gerenciamento de marcas de veículos
- [ ] **Cadastro de Modelos**: Relacionamento entre marcas e modelos
- [ ] **Autenticação JWT**: Sistema de autenticação e autorização com tokens
- [ ] **Upload de Imagens**: Suporte para upload e gerenciamento de fotos dos veículos
- [ ] **Sistema de Logs**: Registro detalhado de operações(ex: visualização no kibana) e auditoria
- [ ] **Exportação de Dados**: Relatórios em PDF/Excel
- [ ] **Notificações**: Sistema de alertas e notificações
- [ ] **Multi-tenancy**: Suporte para múltiplas empresas

## 🛑 Parando a Aplicação

Para parar todos os serviços:

```bash
docker-compose down
```

Para parar e remover os volumes (⚠️ isso apagará os dados do banco):

```bash
docker-compose down -v
```

## 🐛 Troubleshooting

### Problema: Porta já em uso

Se você receber um erro informando que a porta já está em uso, você pode:

1. Parar o serviço que está usando a porta
2. Alterar a porta no arquivo `.env`:
   ```env
   API_PORT=5001
   FRONTEND_PORT=3001
   ```

### Problema: Erro de conexão com o banco de dados

- Verifique se a senha no arquivo `.env` está correta
- Certifique-se de que o container do SQL Server está em execução: `docker ps`
- Aguarde alguns segundos a mais, pois o SQL Server pode levar tempo para inicializar

### Problema: Migrations não foram executadas

- Verifique os logs do backend: `docker-compose logs backend`
- Reinicie o container do backend: `docker-compose restart backend`

## 📚 Documentação Adicional

Este projeto conta com documentação detalhada na pasta `docs/`:

| Documento | Descrição |
|-----------|-----------|
| **[semantic-documentation.md](Desafio Impar/docs/semantic-documentation.md)** | Documentação semântica do sistema com entidades, regras de negócio e premissas |
| **[qa-documentation.md](Desafio Impar/docs/qa-documentation.md)** | Estratégia de qualidade, guia de testes e validações manuais |
| **[ai-test-executions.md](Desafio Impar/docs/ai-test-executions.md)** | Registro de validações realizadas com apoio de Inteligência Artificial (GitHub Copilot) |

> 💡 **Nota para Desenvolvedores**: A documentação semântica é especialmente útil para entender as regras de negócio e decisões arquiteturais do projeto.

## 📄 Licença

Este projeto foi desenvolvido como parte de um desafio técnico.

---

Desenvolvido com ❤️ para o Desafio Ímpar
