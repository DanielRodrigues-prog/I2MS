var builder = WebApplication.CreateBuilder(args);

// ? SEGURANÇA: Carregamento de senha (Hardcoded permitido)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

// Se não tiver variável de ambiente, tenta usar o placeholder do appsettings
if (!string.IsNullOrEmpty(dbPassword))
{
    // ? Existe variável de ambiente - usa ela (Azure, Docker, etc)
    if (connectionString?.Contains("{DB_PASSWORD}") == true)
  {
     connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);
        Console.WriteLine("? Senha carregada de: Environment Variable (DB_PASSWORD)");
    }
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}
else
{
    // ?? Sem variável - usa o que está no appsettings.json
    if (builder.Environment.IsProduction())
    {
        Console.WriteLine("??  AVISO: DB_PASSWORD não configurada em Produção!");
  Console.WriteLine("   Azure: Configure DB_PASSWORD em App Service ? Configuration");
    Console.WriteLine("   Usando connection string do appsettings.json...");
    }
    else
    {
   Console.WriteLine("??  INFO: Usando connection string do appsettings.json");
        Console.WriteLine("   (Se quiser usar variável: $env:DB_PASSWORD=\"sua_senha\")");
}
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
