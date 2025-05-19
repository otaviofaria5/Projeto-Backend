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

        public async Task<List<Livros>> GetAsync()
        {
            return await _livrosCollection.Find(x => true).ToListAsync();
        
        }
    }
}
