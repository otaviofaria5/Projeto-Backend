using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Model;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class UsuarioServices : ControllerBase
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

        public async Task<IActionResult> CadastrarUsuarioAsync(string Nome, string Email, string Endereco)
        {
            var usuario = new Usuario
            {
                Nome = Nome,
                Email = Email,
                Endereco = Endereco
            };

            await _usuarioCollection.InsertOneAsync(usuario);

            return new OkObjectResult(usuario);
        }

        public async Task<IActionResult> AtualizarUsuarioAsync(string Id, string Nome, string Email, string Endereco)
        {
            var usuario = await _usuarioCollection
                .Find(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (usuario == null) return new NotFoundResult();

            usuario.Nome = Nome;
            usuario.Email = Email;
            usuario.Endereco = Endereco;

            await _usuarioCollection.ReplaceOneAsync(x => x.Id == Id, usuario);

            return new OkObjectResult(usuario);
        }

        public async Task<bool> ExcluirUsuarioAsync(string Id)
        {
            var usuario = await _usuarioCollection
                .Find(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (usuario == null) return false;

            await _usuarioCollection.DeleteOneAsync(x => x.Id == Id);

            return true;
        }
    }
}
