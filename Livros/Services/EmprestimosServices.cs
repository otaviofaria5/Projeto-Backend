using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Model;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class EmprestimosServices : ControllerBase
    {
        private readonly IMongoCollection<Emprestimos> _emprestimosCollection;
        private readonly IMongoCollection<Usuario> _usuariosCollection;
        private readonly IMongoCollection<Livros> _livrosCollection;

        public EmprestimosServices(IOptions<EmprestimosDatabaseSettings> emprestimosService)
        {
            var mongoClient = new MongoClient(emprestimosService.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(emprestimosService.Value.DatabaseName);
            _emprestimosCollection = mongoDatabase.GetCollection<Emprestimos>(emprestimosService.Value.CollectionName);
            _usuariosCollection = mongoDatabase.GetCollection<Usuario>("Usuarios");
            _livrosCollection = mongoDatabase.GetCollection<Livros>("Livros");
        }

        public async Task<List<Emprestimos>> GetAsync()
        {
            return await _emprestimosCollection.Find(x => true).ToListAsync();
        }


        public async Task<List<dynamic>> ObterHistoricoAsync()
        {
            var emprestimos = await _emprestimosCollection
                .Find(_ => true)
                .ToListAsync();

            var livros = await _livrosCollection
                .Find(_ => true)
                .ToListAsync();

            var usuarios = await _usuariosCollection
                .Find(_ => true)
                .ToListAsync();

            var resultado = emprestimos.Select(e =>
            {
                var livro = livros.FirstOrDefault(l => l.Id == e.LivroId);
                var usuario = usuarios.FirstOrDefault(u => u.Id == e.UsuarioId);

                return new
                {
                    Livro = livro?.Titulo ?? "Desconhecido",
                    Usuario = usuario?.Nome ?? "Desconhecido",
                    DataEmprestimo = e.DataEmprestimo?.ToString("dd/MM/yyyy HH:mm") ?? "N/A",
                    DataDevolucaoPrevista = e.DataDevolucaoPrevista?.ToString("dd/MM/yyyy HH:mm") ?? "N/A",
                    DataDevolucaoReal = e.DataDevolucaoReal?.ToString("dd/MM/yyyy HH:mm") ?? "Ainda não devolvido",
                    Devolvido = e.DataDevolucaoReal != null
                };
            }).ToList<dynamic>();

            return resultado;
        }
        public async Task<List<dynamic>> ObterLivrosEmprestadosAsync()
        {
            var emprestimos = await _emprestimosCollection
                .Find(e => e.DataDevolucaoReal == null)
                .ToListAsync();

            var result = new List<dynamic>();

            foreach (var emprestimo in emprestimos)
            {
                var livro = await _livrosCollection.Find(l => l.Id == emprestimo.LivroId).FirstOrDefaultAsync();
                var usuario = await _usuariosCollection.Find(u => u.Id == emprestimo.UsuarioId).FirstOrDefaultAsync();

                if (livro != null && usuario != null)
                {
                    result.Add(new
                    {
                        Livro = livro.Titulo,
                        Usuario = usuario.Nome,
                        dataEmprestimo = emprestimo.DataEmprestimo,
                        dataDevolucaoPrevista = emprestimo.DataDevolucaoPrevista
                    });
                }
            }

            return result;
        }

        public async Task<IActionResult> RegistrarEmprestimoAsync(string Nome, string Titulo, DateTimeOffset DataEmprestimo, DateTimeOffset DataDevolucaoPrevista)
        {
            var usuario = await _usuariosCollection
               .Find(u => u.Nome == Nome)
               .FirstOrDefaultAsync();

            if (usuario == null) return new BadRequestObjectResult("Usuário não encontrado.");

            var livro = await _livrosCollection
               .Find(l => l.Titulo == Titulo)
               .FirstOrDefaultAsync();

            if (livro == null || !livro.Disponivel) return new BadRequestObjectResult("Livro indisponível.");

            var emprestimo = new Emprestimos
            {
                UsuarioId = usuario.Id,
                LivroId = livro.Id,
                DataEmprestimo = DataEmprestimo,
                DataDevolucaoPrevista = DataDevolucaoPrevista,
                DataDevolucaoReal = null,   
                Devolvido = false
            };

            livro.Disponivel = false;

            await _emprestimosCollection.InsertOneAsync(emprestimo);
            await _livrosCollection.ReplaceOneAsync(l => l.Id == livro.Id, livro);

            return new OkObjectResult($"Empréstimo registrado com sucesso.\n" +
                                      $"Usuário: {usuario.Nome}\n" +
                                      $"Livro: {livro.Titulo}\n" +
                                      $"Data Empréstimo: {emprestimo.DataEmprestimo:dd/MM/yyyy HH:mm}\n" +
                                      $"Data Devolução Prevista: {emprestimo.DataDevolucaoPrevista:dd/MM/yyyy HH:mm}\n" +
                                      $"Data Devolução Real: Ainda não devolvido");
        }

        public async Task<IActionResult> RegistrarDevolucaoAsync(string Nome, string Livro)
        {
            var livro = await _livrosCollection
                .Find(l => l.Titulo != null && l.Titulo.Equals(Livro, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();

            if (livro == null) return new BadRequestObjectResult("Livro não encontrado.");

            var usuario = await _usuariosCollection
                .Find(u => u.Nome != null && u.Nome.Equals(Nome, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();

            if (usuario == null) return new BadRequestObjectResult("Usuário não encontrado.");

            var emprestimo = await _emprestimosCollection
                .Find(e => e.LivroId == livro.Id && e.UsuarioId == usuario.Id && e.DataDevolucaoReal == null)
                .FirstOrDefaultAsync();

            if (emprestimo == null) return new BadRequestObjectResult("Empréstimo inválido ou já devolvido.");

            emprestimo.DataEmprestimo ??= DateTimeOffset.UtcNow;
            emprestimo.DataDevolucaoPrevista ??= emprestimo.DataEmprestimo.Value.AddDays(14);

            emprestimo.DataDevolucaoReal = DateTimeOffset.UtcNow;
            livro.Disponivel = true;

            await _emprestimosCollection.ReplaceOneAsync(e => e.Id == emprestimo.Id, emprestimo);
            await _livrosCollection.ReplaceOneAsync(l => l.Id == livro.Id, livro);

            return new OkObjectResult($"Devolução registrada com sucesso.\n" +
                                       $"Usuário: {usuario.Nome}\n" +
                                       $"Livro: {livro.Titulo}\n" +
                                       $"Data Empréstimo: {emprestimo.DataEmprestimo:dd/MM/yyyy HH:mm}\n" +
                                       $"Data Devolução Prevista: {emprestimo.DataDevolucaoPrevista:dd/MM/yyyy HH:mm}\n" +
                                       $"Data Devolução Real: {emprestimo.DataDevolucaoReal:dd/MM/yyyy HH:mm}");
        }



    }
}
