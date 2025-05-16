using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Model;
using MongoDB.Bson;

namespace Services
{
    public class EmprestimosServices
    {
        private readonly IMongoCollection<Emprestimos> _emprestimosCollection;

        public EmprestimosServices(IOptions<EmprestimosDatabaseSettings> emprestimosService)
        {
            var mongoClient = new MongoClient(emprestimosService.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(emprestimosService.Value.DatabaseName);
            _emprestimosCollection = mongoDatabase.GetCollection<Emprestimos>(emprestimosService.Value.CollectionName);
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
            emprestimos.dataDevolucaoReal = null;
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


        public async Task RegristarEmprestimoAsync(string usuarioId, string livroId)
        {
           
            var usuariosCollection = _emprestimosCollection.Database.GetCollection<Usuario>("Usuarios");
            var livrosCollection = _emprestimosCollection.Database.GetCollection<Livros>("Livros");

            var usuario = await usuariosCollection.Find(x => x.Id == usuarioId).FirstOrDefaultAsync();
            var livro = await livrosCollection.Find(x => x.Id == livroId).FirstOrDefaultAsync();

            if (usuario == null || livro == null || !livro.Disponivel)
                return;

            var emprestimo = new Emprestimos
            {
                usuarioId = usuarioId,
                livroId = livroId,
                dataEmprestimo = DateTimeOffset.UtcNow,
                dataDevolucaoPrevista = DateTimeOffset.UtcNow.AddDays(7),
                dataDevolucaoReal = null
            };

            livro.Disponivel = false;

            await _emprestimosCollection.InsertOneAsync(emprestimo);
            await livrosCollection.ReplaceOneAsync(x => x.id == livroId, livro);
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var result = await _emprestimosCollection.DeleteOneAsync(x => x.id == id);
            return result.DeletedCount > 0;
        }
    }
}
