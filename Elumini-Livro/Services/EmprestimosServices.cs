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

        public async Task<List<Emprestimos>> GetAsync() =>
            await _emprestimosCollection.Find(x => true).ToListAsync();

        public async Task<List<Emprestimos>> GetPorUsuarioAsync(string usuarioId) =>
            await _emprestimosCollection.Find(x => x.usuarioId == usuarioId).ToListAsync();

        public async Task CriarAsync(Emprestimos emprestimos)
        {
            emprestimos.id = ObjectId.GenerateNewId().ToString(); 
            emprestimos.dataEmprestimo = DateTimeOffset.UtcNow;
            emprestimos.dataDevolucaoPrevista = DateTimeOffset.UtcNow.AddDays(7);
            emprestimos.dataDevolucaoReal = default; 
            await _emprestimosCollection.InsertOneAsync(emprestimos);
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

        public async Task<bool> RegistrarEmprestimoAsync(string nome, string titulo)
        {
            var usuario = await _usuariosCollection
                .Find(x => x.nome.ToLower() == nome.ToLower())
                .FirstOrDefaultAsync();

            var livro = await _livrosCollection
                .Find(x => x.titulo.ToLower() == titulo.ToLower())
                .FirstOrDefaultAsync();

        
            if (usuario == null || livro == null || !livro.Disponivel)
            {
                return false;
            }

            var emprestimo = new Emprestimos
            {
                usuarioId = usuario.id ?? throw new InvalidOperationException("Usuário ID não pode ser nulo."),
                livroId = livro.id ?? throw new InvalidOperationException("Livro ID não pode ser nulo."),
                dataEmprestimo = DateTimeOffset.UtcNow,
                dataDevolucaoPrevista = DateTimeOffset.UtcNow.AddDays(7),
                dataDevolucaoReal = default
            };

            livro.Disponivel = false;

           
            await _emprestimosCollection.InsertOneAsync(emprestimo);
            await _livrosCollection.ReplaceOneAsync(x => x.id == livro.id, livro);

            return true;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var result = await _emprestimosCollection.DeleteOneAsync(x => x.id == id);
            return result.DeletedCount > 0;
        }
    }
}
