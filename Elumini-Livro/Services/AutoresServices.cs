using Microsoft.Extensions.Options;
using Model;
using MongoDB.Driver;

namespace Services
{
    public class AutoresServices
{
     private readonly IMongoCollection<Autores> _autoresCollection;
        public AutoresServices(IOptions<AutoresDatabaseSettings> autoresService)
        {
            var mongoClient = new MongoClient(autoresService.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(autoresService.Value.DatabaseName);
            _autoresCollection = mongoDatabase.GetCollection<Autores>(autoresService.Value.CollectionName);
        }

        public async Task<List<Autores>> GetAsync() =>
            await _autoresCollection.Find(x => true).ToListAsync();

        public async Task<Autores> GetAsync(string id) =>
            await _autoresCollection.Find(x => x.id == id).FirstOrDefaultAsync();
    }
}
