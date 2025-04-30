using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Projeto_Backend.Model;

namespace Projeto_Backend.Services
{
    public class UsuariosService
    {
        public class UsuarioServices
        {
            private readonly IMongoCollection<Usuario> _usuarioCollection;

            public UsuarioServices(IOptions<UsuarioDatabaseSettings> usuarioServices)
            {
                var mongoClient = new MongoClient(usuarioServices.Value.ConnectionString);
                var mongoDatabase = mongoClient.GetDatabase(usuarioServices.Value.DatabaseName);

                _usuarioCollection = mongoDatabase.GetCollection<Usuario>
                    (usuarioServices.Value.CollectionName);
            }

            public async Task<List<Usuario>> GetAsync() =>
                await _usuarioCollection.Find(x => true).ToListAsync();

            public async Task<Usuario> GetAsync(string id) =>
                await _usuarioCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            public async Task CreateAsync(Usuario usuario) =>
                await _usuarioCollection.InsertOneAsync(usuario);

            public async Task UpdateAsync(string id, Usuario usuario) =>
                await _usuarioCollection.ReplaceOneAsync(x => x.Id == id, usuario);

            public async Task RemoveAsync(string id) =>
                await _usuarioCollection.DeleteOneAsync(x => x.Id == id);
        }
    }

}
