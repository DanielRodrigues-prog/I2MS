var builder = WebApplication.CreateBuilder(args);

// ? SEGURANÇA: Carregar variáveis de ambiente
// Se a senha está em variável de ambiente, substitui no connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

if (!string.IsNullOrEmpty(dbPassword))
{
    // Substitui o placeholder {DB_PASSWORD} pela senha real
    connectionString = connectionString?.Replace("{DB_PASSWORD}", dbPassword);

    // Atualiza a configuração com a string de conexão interpolada
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}
else if (builder.Environment.IsProduction())
{
    // Em produção, a senha DEVE estar em variável de ambiente
    throw new InvalidOperationException("? ERRO CRÍTICO: Variável de ambiente DB_PASSWORD não configurada! Configure no servidor/container antes de rodar.");
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
