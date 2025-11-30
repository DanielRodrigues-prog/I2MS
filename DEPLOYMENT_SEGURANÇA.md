# ?? GUIA DE DEPLOYMENT - SEGURANÇA EM PRODUÇÃO

## ? QUICK START (5 MINUTOS)

### Seu servidor/máquina de produção

```bash
# 1. Configure a variável de ambiente
export DB_PASSWORD="SenhaForte123!@#"

# 2. Deploy da API
cd FerramentariaAPI
dotnet publish -c Release
cd bin/Release/net8.0/publish
dotnet FerramentariaAPI.dll
```

---

## ?? DOCKER (Recomendado)

### Dockerfile para API

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet publish "FerramentariaAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# ? SEGURANÇA: Não copia appsettings.json real
# A senha vem de variável de ambiente

ENTRYPOINT ["dotnet", "FerramentariaAPI.dll"]
```

### Executar Container

```bash
# Build
docker build -t ferramentaria-api:latest .

# Run com variável de ambiente
docker run -e DB_PASSWORD="SenhaForte123!@#" \
  -p 5000:80 \
  ferramentaria-api:latest

# Docker Compose
docker-compose up
```

### docker-compose.yml

```yaml
version: '3.8'
services:
  api:
    build: ./FerramentariaAPI
    ports:
      - "5000:80"
    environment:
      - DB_PASSWORD=${DB_PASSWORD}
      - ASPNETCORE_ENVIRONMENT=Production
    depends_on:
    - sqlserver
  
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: Y
   SA_PASSWORD: ${SA_PASSWORD}
    ports:
      - "1433:1433"
```

**Executar:**
```bash
export DB_PASSWORD="SenhaForte123!@#"
export SA_PASSWORD="SQLAdminPwd123!@#"
docker-compose up
```

---

## ?? AZURE APP SERVICE

### 1. Criar App Service

```bash
# Login
az login

# Criar resource group
az group create --name myResourceGroup --location eastus

# Criar App Service Plan
az appservice plan create \
  --name myAppServicePlan \
  --resource-group myResourceGroup \
  --sku B1 \
  --is-linux

# Criar Web App
az webapp create \
  --resource-group myResourceGroup \
  --plan myAppServicePlan \
  --name ferramentaria-api \
  --runtime "DOTNET|8.0"
```

### 2. Configurar Variáveis de Ambiente

**Via CLI:**
```bash
az webapp config appsettings set \
  --resource-group myResourceGroup \
  --name ferramentaria-api \
  --settings DB_PASSWORD="SenhaForte123!@#"
```

**Via Portal Azure:**
1. App Service ? Configuration
2. New application setting
3. Name: `DB_PASSWORD`
4. Value: `SenhaForte123!@#`

### 3. Deploy

```bash
# Build
dotnet publish -c Release -o ./publish

# Deploy
az webapp deployment source config-zip \
  --resource-group myResourceGroup \
  --name ferramentaria-api \
  --src-path ./publish.zip
```

---

## ?? CHECKLIST DE SEGURANÇA PRÉ-PRODUÇÃO

- [ ] ? `DB_PASSWORD` configurada no servidor
- [ ] ? `appsettings.json` NÃO contém senha real
- [ ] ? HTTPS/SSL ativado
- [ ] ? Firewall bloqueando acesso direto ao SQL Server
- [ ] ? Backups do banco habilitados
- [ ] ? Logs aplicação ativados (Serilog)
- [ ] ? Rate limiting configurado (Throttling)
- [ ] ? CORS restrito apenas aos domínios necessários
- [ ] ? Senhas do Git removidas do histórico
- [ ] ? `.gitignore` atualizado

---

## ?? VERIFICAÇÕES PÓS-DEPLOYMENT

```bash
# 1. Testar health check da API
curl https://seu-servidor/api/ferramentas/ComCalibracao

# 2. Verificar se erro de SQL Injection é capturado
curl https://seu-servidor/api/movimentacao/VerificarMecanico/1' OR '1'='1

# 3. Testar logging de erros
tail -f /var/log/ferramentaria.log

# 4. Monitorar conexão com banco
sqlcmd -S seu-servidor.database.windows.net -U admin -P $DB_PASSWORD -Q "SELECT 1"
```

---

## ?? TROUBLESHOOTING

### Problema: "DB_PASSWORD não configurada"

```
? Error: Variável de ambiente DB_PASSWORD não configurada
```

**Solução:**
```bash
# Windows
set DB_PASSWORD=sua_senha
dotnet run

# Linux
export DB_PASSWORD=sua_senha
dotnet run
```

### Problema: "Connection timeout"

```
? Error: Connection timeout
```

**Verificar:**
1. Firewall bloqueando porta 1433?
2. Usuário SQL está correto?
3. Senha está correta?

```bash
# Testar conexão
sqlcmd -S servidor.database.windows.net -U admin -P $DB_PASSWORD
```

### Problema: "Erro ao carregar histórico"

**Checar logs:**
```bash
# Se estiver em Docker
docker logs nome-do-container

# Se estiver em servidor
cat /var/log/ferramentaria.log | grep -i erro
```

---

## ?? MONITORAMENTO EM PRODUÇÃO

### Application Insights (Azure)

```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddSingleton<ITelemetryInitializer, HttpRequestTelemetryInitializer>();
```

### Serilog (Logging local)

```csharp
// Program.cs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
 .WriteTo.File("logs/ferramentaria-.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();
```

---

## ?? GERENCIAR MÚLTIPLAS SENHAS

### Azure Key Vault

```bash
# Criar Key Vault
az keyvault create --name myKeyVault --resource-group myResourceGroup

# Adicionar senha
az keyvault secret set --vault-name myKeyVault --name DBPassword --value "SenhaForte123!@#"

# Program.cs
var keyVaultUrl = new Uri("https://mykeyvault.vault.azure.net");
var credential = new DefaultAzureCredential();
var client = new SecretClient(keyVaultUrl, credential);
var secret = client.GetSecret("DBPassword");
```

---

## ?? PERFORMANCE

### Connection Pooling

```csharp
// appsettings.json - Já otimizado na connection string
"Max Pool Size=100;Min Pool Size=5;Connection Lifetime=300;"
```

### Caching

```csharp
// Para futuro: Adicionar Redis para cache
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Configuration.GetConnectionString("Redis");
});
```

---

## ?? DOCUMENTAÇÃO

- [Microsoft - Secure .NET deployments](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/security)
- [OWASP - Authentication](https://owasp.org/www-community/attacks/authentication)
- [Azure - Best Practices](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures)

---

**Status:** ? PRONTO PARA PRODUÇÃO  
**Última atualização:** 2025
