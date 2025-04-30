using Projeto_Backend.Model;
using Projeto_Backend.Services;
using static Projeto_Backend.Services.UsuariosService;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LivrosDatabaseSettings>
    (builder.Configuration.GetSection("DevNetStoreDatabaseLivros"));

builder.Services.AddSingleton<LivrosServices>();

builder.Services.Configure<UsuarioDatabaseSettings>
    (builder.Configuration.GetSection("DevNetStoreDatabaseUsuario"));

builder.Services.AddSingleton<UsuarioServices>();





// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
