using Model;
using Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LivrosDatabaseSettings>
    (builder.Configuration.GetSection("DevNetStoreDatabaseLivros"));

builder.Services.AddSingleton<LivrosServices>();

builder.Services.Configure<UsuarioDatabaseSettings>
    (builder.Configuration.GetSection("DevNetStoreDatabaseUsuarios"));

builder.Services.AddSingleton<UsuarioServices>();

builder.Services.Configure<AutoresDatabaseSettings>
    (builder.Configuration.GetSection("DevNetStoreDatabaseAutores"));

builder.Services.AddSingleton<AutoresServices>();

builder.Services.Configure<EmprestimosDatabaseSettings>
    (builder.Configuration.GetSection("DevNetStoreDatabaseEmprestimos"));

builder.Services.AddSingleton<EmprestimosServices>();

builder.Services.Configure<AvaliacoesDatabaseSettings>
    (builder.Configuration.GetSection("DevNetStoreDatabaseAvaliacoes"));

builder.Services.AddSingleton<AvaliacoesServices>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodos", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseCors("PermitirTodos");






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
