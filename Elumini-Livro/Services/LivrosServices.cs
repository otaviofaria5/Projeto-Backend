using Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Services
{
    public class LivrosServices
    {
        private readonly IMongoCollection<Livros> _livrosCollection;

        public LivrosServices(IOptions<LivrosDatabaseSettings> livroService)
        {
      
            var mongoClient = new MongoClient(livroService.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(livroService.Value.DatabaseName);
            _livrosCollection = mongoDatabase.GetCollection<Livros>(livroService.Value.CollectionName);
        }

        public async Task<List<Livros>> GetAsync() =>
          await _livrosCollection.Find(x => true).ToListAsync();
        public async Task<Livros> GetAsync(string id) =>
           await _livrosCollection.Find(x => x.id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Livros livros) =>
            await _livrosCollection.InsertOneAsync(livros);
        public async Task UpdateAsync(string id, Livros livros) =>
           await _livrosCollection.ReplaceOneAsync(x => x.id == id, livros);
        public async Task RemoveAsync(string id) =>
            await _livrosCollection.DeleteOneAsync(x => x.id == id);
    }
}
