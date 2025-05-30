﻿using Microsoft.Extensions.Options;
using Model;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<List<Autores>> PesquisarAutorAsync(string nome)
        {
            var filter = Builders<Autores>.Filter.Empty;
            if (!string.IsNullOrEmpty(nome))
            {
                filter &= Builders<Autores>.Filter.Regex(x => x.nome, new MongoDB.Bson.BsonRegularExpression(nome, "i"));
            }
            return await _autoresCollection.Find(filter).ToListAsync();
        }

        public async Task<IActionResult> CadastrarAutorAsync(string nome, string biografia, string nacionalidade)
        {
            var autores = new Autores
            {
                nome = nome,
                biografia = biografia,
                nacionalidade = nacionalidade
            };

            await _autoresCollection.InsertOneAsync(autores);
            return Ok(autores);
        }

        public async Task<IActionResult> AtualizarAutorAsync(string id, string nome, string biografia, string nacionalidade)
        {
            var autores = await _autoresCollection
                .Find(x => x.id == id)
                .FirstOrDefaultAsync();

            if (autores == null) return NotFound();

            autores.nome = nome;
            autores.biografia = biografia;
            autores.nacionalidade = nacionalidade;
            await _autoresCollection.ReplaceOneAsync(x => x.id == id, autores);
           
            return Ok(autores);
        }

        public async Task<bool> ExcluirAutorAsync(string id)
        {
            var autores = await _autoresCollection
                .Find(x => x.id == id)
                .FirstOrDefaultAsync();

            if (autores == null) return false;
           
            await _autoresCollection.DeleteOneAsync(x => x.id == id);
            
            return true;
        }

      
    }
}
