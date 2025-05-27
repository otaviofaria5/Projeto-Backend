using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Model;

namespace Services
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

        public async Task<List<Usuario>> ListarUsuarioAsync()
        {
            return await _usuarioCollection.Find(x => true).ToListAsync();
        }

        public async Task<Usuario?> CadastrarUsuarioAsync(string nome, string email, string endereco)
        {
            var usuario = new Usuario
            {
                nome = nome,
                email = email,
                endereco = endereco
            };

            await _usuarioCollection.InsertOneAsync(usuario);
            return usuario;
        }

        public async Task<Usuario?> AtualizarUsuarioAsync(string id, string nome, string email, string endereco)
        {
            var usuario = await _usuarioCollection.Find(x => x.id == id).FirstOrDefaultAsync();
            if (usuario == null) return null;
            usuario.nome = nome;
            usuario.email = email;
            usuario.endereco = endereco;
            await _usuarioCollection.ReplaceOneAsync(x => x.id == id, usuario);
            return usuario;
        }

        public async Task<bool> ExcluirUsuarioAsync(string id)
        {
            var usuario = await _usuarioCollection.Find(x => x.id == id).FirstOrDefaultAsync();
            if (usuario == null) return false;
            await _usuarioCollection.DeleteOneAsync(x => x.id == id);
            return true;
        }
    }
}
