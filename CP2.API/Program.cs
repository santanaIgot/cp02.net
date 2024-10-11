using CP2.Data.AppData; // Importando o namespace do ApplicationContext
using CP2.IoC; // Importando o namespace do Bootstrap
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicione os servi�os ao cont�iner.
builder.Services.AddControllers();

// Configura��o do DbContext
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseOracle(builder.Configuration["ConnectionStrings:DefaultConnection"],
        b => b.MigrationsAssembly("CP2.API")); // Altere "CP2.API" para o nome do seu projeto de migra��es
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configura��o do IoC (Invers�o de Controle)
Bootstrap.Start(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure o pipeline de solicita��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
