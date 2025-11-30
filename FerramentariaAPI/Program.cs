var builder = WebApplication.CreateBuilder(args);

// ? SEGURANÇA: Carregar variáveis de ambiente (OBRIGATÓRIO)
// Se a senha está em variável de ambiente, substitui no connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

// ?? VALIDAÇÃO IMPORTANTE
if (string.IsNullOrEmpty(dbPassword))
{
    if (builder.Environment.IsProduction())
 {
        // Em produção, FALHA COM ERRO
        throw new InvalidOperationException(
        "? ERRO CRÍTICO: Variável de ambiente DB_PASSWORD não configurada!\n" +
            "Configure no servidor/container ANTES de rodar a aplicação.\n" +
        "Azure: App Service ? Configuration ? New Setting (DB_PASSWORD)\n" +
            "Docker: docker run -e DB_PASSWORD=\"sua_senha\" ...\n" +
   "Local: $env:DB_PASSWORD=\"sua_senha\"");
    }
    else
    {
     // Em desenvolvimento, AVISA mas permite continuar (com CUIDADO!)
        Console.WriteLine("??  AVISO: DB_PASSWORD não configurada em desenvolvimento!");
        Console.WriteLine("   Use: $env:DB_PASSWORD=\"sua_senha_aqui\"");
        Console.WriteLine("   A connection string está usando placeholder {DB_PASSWORD}");
        Console.WriteLine("   Se o banco não conectar, configure a variável de ambiente.");
    }
}
else
{
    // ? Senha existe - substitui o placeholder
    if (connectionString?.Contains("{DB_PASSWORD}") == true)
    {
      connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);
      Console.WriteLine("? Senha DB_PASSWORD carregada com sucesso");
    }
    else
    {
        Console.WriteLine("??  Connection string não tem placeholder {DB_PASSWORD}");
    }
    
    // Atualiza a configuração
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
