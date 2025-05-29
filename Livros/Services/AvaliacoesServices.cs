using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Model;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class AvaliacoesServices
    {
        private readonly IMongoCollection<Avaliacoes> _avaliacoesCollection;
        private readonly IMongoCollection<Livros> _livrosCollection;
        private readonly IMongoCollection<Usuario> _usuariosCollection;

        public AvaliacoesServices(IOptions<AvaliacoesDatabaseSettings> avaliacoesService)
        {
            var mongoClient = new MongoClient(avaliacoesService.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(avaliacoesService.Value.DatabaseName);
            _avaliacoesCollection = mongoDatabase.GetCollection<Avaliacoes>(avaliacoesService.Value.CollectionName);
            _livrosCollection = mongoDatabase.GetCollection<Livros>("Livros");
            _usuariosCollection = mongoDatabase.GetCollection<Usuario>("Usuarios");
        }

        public async Task<List<Avaliacoes>> GetAsync()
        {
            return await _avaliacoesCollection.Find(x => true).ToListAsync();
        }

        public async Task<IActionResult> ExibirLivroAsync(string nomeLivro)
        {
            var livro = await _livrosCollection
                .Find(l => l.titulo == nomeLivro)
                .FirstOrDefaultAsync();

            if (livro == null)
                return new NotFoundObjectResult("Livro não encontrado.");

            var avaliacoes = await _avaliacoesCollection
                .Find(x => x.livroId == livro.id)
                .ToListAsync();
         
            if (avaliacoes.Count == 0)
            {
                return new NotFoundObjectResult("Nenhuma avaliação encontrada para este livro.");
            }

            return new OkObjectResult(avaliacoes.FirstOrDefault());
        }

        public async Task<IActionResult> CadastrarAvaliacaoAsync(string nomeLivro, string nomeUsuario, int avaliacao, string comentario)
        {
            var livro = await _livrosCollection
                .Find(l => l.titulo == nomeLivro)
                .FirstOrDefaultAsync();

            if (livro == null)
                return new NotFoundObjectResult("Livro não encontrado.");

            var usuario = await _usuariosCollection
                .Find(u => u.nome == nomeUsuario)
                .FirstOrDefaultAsync();

            if (usuario == null)
                return new NotFoundObjectResult("Usuário não encontrado.");

            var avaliacaoObj = new Avaliacoes
            {
                livroId = livro.id, 
                usuarioId = usuario.id, 
                avaliacao = avaliacao,
                comentario = comentario
            };

            await _avaliacoesCollection.InsertOneAsync(avaliacaoObj);

            return new OkObjectResult(avaliacaoObj);
        }

        public async Task<bool> ExcluirAvaliacaoAsync(string id)
        {
            var avaliacao = await _avaliacoesCollection
                .Find(x => x.id == id)
                .FirstOrDefaultAsync();
           
            if (avaliacao == null) return false;

            await _avaliacoesCollection.DeleteOneAsync(x => x.id == id);
           
            return true;
        }

    }
}
