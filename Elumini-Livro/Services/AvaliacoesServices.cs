using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Model;

namespace Services
{
    public class AvaliacoesServices
{
        private readonly IMongoCollection<Avaliacoes> _avaliacoesCollection;

        public AvaliacoesServices(IOptions<AvaliacoesDatabaseSettings> avaliacoesService)
        {
            var mongoClient = new MongoClient(avaliacoesService.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(avaliacoesService.Value.DatabaseName);
            _avaliacoesCollection = mongoDatabase.GetCollection<Avaliacoes>(avaliacoesService.Value.CollectionName);
        }

        public async Task<List<Avaliacoes>> GetAsync()
        { 
           return await _avaliacoesCollection.Find(x => true).ToListAsync();
        }

    }
}
