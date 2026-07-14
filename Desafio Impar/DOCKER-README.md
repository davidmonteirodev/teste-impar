# Docker Compose - VehicleCRM

Este docker-compose.yml orquestra toda a stack da aplicação VehicleCRM com inicialização sequencial e checagens de saúde.

## Ordem de Inicialização

1. **SQL Server**: Inicializa primeiro e aguarda estar saudável
2. **Backend API**: Inicia após o SQL Server estar pronto, executa as migrations automaticamente
3. **Frontend React**: Inicia após o Backend estar saudável (migrations concluídas)

## Serviços

### SQL Server
- Porta: `1433`
- Usuário: `sa`
- Senha: `SuaSenha@123`
- Health check: Verifica conexão com o banco

### Backend (API .NET)
- Porta: `5000` (mapeada para 8080 no container)
- Health check endpoint: `http://localhost:5000/health`
- Aguarda SQL Server estar saudável
- Executa migrations automaticamente na inicialização

### Frontend (React)
- Porta: `3000` (mapeada para 80 no container)
- Aguarda Backend estar saudável (após migrations)
- API URL: `http://localhost:5000`

## Como Usar

### Iniciar toda a stack:
```bash
docker-compose up -d
```

### Ver logs em tempo real:
```bash
docker-compose logs -f
```

### Ver logs de um serviço específico:
```bash
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f sqlserver
```

### Parar todos os serviços:
```bash
docker-compose down
```

### Parar e remover volumes (limpa banco de dados):
```bash
docker-compose down -v
```

### Rebuild dos containers:
```bash
docker-compose up -d --build
```

## Troubleshooting

### Backend não inicia
- Verifique se o SQL Server está saudável: `docker-compose ps`
- Veja os logs: `docker-compose logs sqlserver`

### Frontend não inicia
- Verifique se o backend está saudável: `curl http://localhost:5000/health`
- Veja os logs: `docker-compose logs backend`

### Migrations não executam
- O backend executa migrations automaticamente na inicialização
- Verifique logs do backend: `docker-compose logs backend`
- Verifique conexão com SQL Server

## Health Checks

Todos os serviços possuem health checks configurados:
- **SQL Server**: Verifica conexão via sqlcmd a cada 10s
- **Backend**: Verifica endpoint /health via curl a cada 10s
- **Frontend**: Sem health check (nginx sempre responde)

Os containers dependentes só iniciam quando o health check do container anterior retorna "healthy".
