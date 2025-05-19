using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Model;
using MongoDB.Bson;

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

       
         public async Task<List<Emprestimos>> ObterHistoricoEmprestimosAsync(string usuarioId)
        {
            return await _emprestimosCollection
                .Find(e => e.usuarioId == usuarioId)
                .SortByDescending(e => e.dataEmprestimo)
                .ToListAsync();
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
        public async Task RegristarDevolucaoAsync(string id, DateTime dataDevolucao)
        {
            var emprestimo = await _emprestimosCollection.Find(x => x.id == id).FirstOrDefaultAsync(); 
            if (emprestimo != null)
            {
                emprestimo.dataDevolucaoPrevista = dataDevolucao;
                emprestimo.dataEmprestimo = DateTimeOffset.UtcNow;
                emprestimo.dataDevolucaoReal = DateTimeOffset.UtcNow;
                await _emprestimosCollection.ReplaceOneAsync(x => x.id == id, emprestimo); 
            }
        }

        // faltar testar devolução de emprestimos

        public async Task<string> RegistrarEmprestimoAsync(string nome, string titulo)
        {
            var usuario = await _usuariosCollection  
              .Find(x => x.nome != null && x.nome.ToLower() == nome.ToLower())
              .FirstOrDefaultAsync();

            var livro = await _livrosCollection
            .Find(x => x.titulo != null && x.titulo.ToLower() == titulo.ToLower())
            .FirstOrDefaultAsync();

            if (usuario == null) return "Usuário não encontrado.";
            if (livro == null || !livro.Disponivel) return "Livro indisponível.";

            var emprestimo = new Emprestimos
            {
                usuarioId = usuario.id,
                livroId = livro.id,
                dataEmprestimo = DateTimeOffset.UtcNow,
                dataDevolucaoPrevista = DateTimeOffset.UtcNow.AddDays(14),
                dataDevolucaoReal = default
            };

            livro.Disponivel = false;

            await _emprestimosCollection.InsertOneAsync(emprestimo);
            await _livrosCollection.ReplaceOneAsync(x => x.id == livro.id, livro);

            return "Empréstimo registrado com sucesso.";
        }

        public async Task<string> RegistrarDevolucaoAsync(int emprestimoId)
        { 
            var emprestimo = await _emprestimosCollection
                .Find(e => e.id == emprestimoId.ToString())
                .FirstOrDefaultAsync();

            if (emprestimo == null || emprestimo.dataDevolucaoReal != null)
                return "Empréstimo inválido ou já devolvido.";

            var livro = await _livrosCollection
                .Find(l => l.id == emprestimo.livroId)
                .FirstOrDefaultAsync();

            if (livro == null)
                return "Livro associado ao empréstimo não encontrado.";

            emprestimo.dataDevolucaoReal = DateTimeOffset.UtcNow;
            livro.Disponivel = true;

            await _emprestimosCollection.ReplaceOneAsync(e => e.id == emprestimo.id, emprestimo);
            await _livrosCollection.ReplaceOneAsync(l => l.id == livro.id, livro);

            return "Devolução registrada com sucesso.";
        }


        public async Task<bool> RemoveAsync(string id)
        {
            var result = await _emprestimosCollection.DeleteOneAsync(x => x.id == id);
            return result.DeletedCount > 0;

            // falta testar deletar
        }
    }
}
