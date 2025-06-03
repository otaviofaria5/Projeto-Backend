using Microsoft.Extensions.Options;
using Model;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Text.RegularExpressions;
namespace Services
{
    public class AutoresServices : ControllerBase
    {
        private readonly IMongoCollection<Autores> _autoresCollection;

        public AutoresServices(IOptions<AutoresDatabaseSettings> autoresService)
        {
            var mongoClient = new MongoClient(autoresService.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(autoresService.Value.DatabaseName);
            _autoresCollection = mongoDatabase.GetCollection<Autores>(autoresService.Value.CollectionName);
        }

        public async Task<List<Autores>> GetAsync()
        {
            return await _autoresCollection.Find(x => true).ToListAsync();
        }

        public async Task<List<Autores>> PesquisarAutorAsync(string letraInicial)
        {
            var filter = Builders<Autores>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(letraInicial))
            {
                var regex = new BsonRegularExpression($"^{Regex.Escape(letraInicial)}", "i");
                filter = Builders<Autores>.Filter.Regex(a => a.Nome, regex);
            }

            return await _autoresCollection.Find(filter).ToListAsync();
        }

        public async Task<IActionResult> CadastrarAutorAsync(string Nome, string Biografia, string Nacionalidade)
        {
            var autores = new Autores
            {
                Nome = Nome,
                Biografia = Biografia,
                Nacionalidade = Nacionalidade
            };

            await _autoresCollection.InsertOneAsync(autores);
            return Ok(autores);
        }

        public async Task<IActionResult> AtualizarAutorAsync(string Id, string Nome, string Biografia, string Nacionalidade)
        {
            var autores = await _autoresCollection
                .Find(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (autores == null) return NotFound();

            autores.Nome = Nome;
            autores.Biografia = Biografia;
            autores.Nacionalidade = Nacionalidade;
            await _autoresCollection.ReplaceOneAsync(x => x.Id == Id, autores);

            return Ok(autores);
        }

        public async Task<bool> ExcluirAutorAsync(string Id)
        {
            var autores = await _autoresCollection
                .Find(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (autores == null) return false;

            await _autoresCollection.DeleteOneAsync(x => x.Id == Id);

            return true;
        }
    }
}
