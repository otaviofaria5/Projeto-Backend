using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Model;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class EmprestimosServices
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


        public async Task<List<Emprestimos>> ObterHistoricoPorNomeAsync(string nomeUsuario)
        {
            var usuario = await _usuariosCollection
                .Find(u => u.nome == nomeUsuario)
                .FirstOrDefaultAsync();

            if (usuario == null) return new List<Emprestimos>();
            var emprestimos = await _emprestimosCollection
                .Find(e => e.usuarioId == usuario.id)
                .SortByDescending(e => e.dataEmprestimo)
                .ToListAsync();

            return emprestimos;
        }

      

        public async Task<List<dynamic>> ObterLivrosEmprestadosAsync()
        {
            var emprestimos = await _emprestimosCollection
                .Find(e => e.dataDevolucaoReal == null)
                .ToListAsync();

            var result = new List<dynamic>();

            foreach (var emprestimo in emprestimos)
            {
                var livro = await _livrosCollection.Find(l => l.id == emprestimo.livroId).FirstOrDefaultAsync();
                var usuario = await _usuariosCollection.Find(u => u.id == emprestimo.usuarioId).FirstOrDefaultAsync();

                if (livro != null && usuario != null)
                {
                    result.Add(new
                    {
                        Livro = livro.titulo,
                        Usuario = usuario.nome,
                        DataEmprestimo = emprestimo.dataEmprestimo,
                        DataDevolucaoPrevista = emprestimo.dataDevolucaoPrevista
                    });
                }
            }

            return result;
        }

        public async Task<string> RegistrarDevolucaoAsync(string emprestimoId)
        {
            var emprestimo = await _emprestimosCollection
                .Find(e => e.id == emprestimoId)
                .FirstOrDefaultAsync();

            if (emprestimo == null || emprestimo.dataDevolucaoReal != null)
                return "Empréstimo inválido ou já devolvido.";

            emprestimo.dataDevolucaoReal = DateTime.Now;

            var livro = await _livrosCollection
                .Find(l => l.id == emprestimo.livroId)
                .FirstOrDefaultAsync();

            if (livro != null)
            {
                livro.Disponivel = true;
                await _livrosCollection.ReplaceOneAsync(l => l.id == livro.id, livro);
            }

            await _emprestimosCollection.ReplaceOneAsync(e => e.id == emprestimo.id, emprestimo);

            return "Devolução registrada com sucesso.";
        }

        public async Task<string> RegistrarDevolucaoPorTituloAsync(string tituloLivro)
        {
            var emprestimo = await _emprestimosCollection
                .Find(e => e.livroId != null && e.dataDevolucaoReal == null)
                .FirstOrDefaultAsync();

            if (emprestimo == null)
                return "Empréstimo não encontrado ou livro já devolvido.";

            var livro = await _livrosCollection
             .Find(l => l.id == emprestimo.livroId && l.titulo != null && l.titulo.ToLower() == tituloLivro.ToLower())
             .FirstOrDefaultAsync();


            if (livro == null)
                return "Livro associado ao empréstimo não encontrado.";

            emprestimo.dataDevolucaoReal = DateTimeOffset.UtcNow;
            livro.Disponivel = true;

            await _emprestimosCollection.ReplaceOneAsync(e => e.id == emprestimo.id, emprestimo);
            await _livrosCollection.ReplaceOneAsync(l => l.id == livro.id, livro);

            return "Devolução registrada com sucesso.";
        }
     
        public async Task<string> RegistrarEmprestimoAsync(string nome, string titulo)
        {
            var usuario = await _usuariosCollection.Find(u => u.nome == nome).FirstOrDefaultAsync();
            if (usuario == null) return "Usuário não encontrado.";
            var livro = await _livrosCollection.Find(l => l.titulo == titulo).FirstOrDefaultAsync();
            if (livro == null || !livro.Disponivel) return "Livro indisponível.";
            var emprestimo = new Emprestimos
            {
                usuarioId = usuario.id, 
                livroId = livro.id,   
                dataEmprestimo = DateTime.Now,
                dataDevolucaoPrevista = DateTime.Now.AddDays(14),
                dataDevolucaoReal = null
            };
            livro.Disponivel = false;
            await _emprestimosCollection.InsertOneAsync(emprestimo);
            await _livrosCollection.ReplaceOneAsync(l => l.id == livro.id, livro);
            return "Empréstimo registrado com sucesso.";
        }

        public async Task<bool> ExlcuirEmprestimosAsync(string id)
        {
            var result = await _emprestimosCollection.DeleteOneAsync(x => x.id == id);
            return result.DeletedCount > 0;
        }

       
    }
}
