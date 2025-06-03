using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Model;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class AvaliacoesServices : ControllerBase
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

        public async Task<IActionResult> ExibirLivroAsync(string Livro)
        {
            var livro = await _livrosCollection
                .Find(l => l.Titulo == Livro)
                .FirstOrDefaultAsync();

            if (livro == null)
                return new NotFoundObjectResult("Livro não encontrado.");

            var avaliacoes = await _avaliacoesCollection
                .Find(x => x.LivroId == livro.Id)
                .ToListAsync();
         
            if (avaliacoes.Count == 0)
            {
                return new NotFoundObjectResult("Nenhuma avaliação encontrada para este livro.");
            }

            return new OkObjectResult(avaliacoes.FirstOrDefault());
        }

        public async Task<IActionResult> CadastrarAvaliacaoAsync(string Livro, string Nome, int Avaliacao, string Comentario)
        {
            if (Avaliacao < 1 || Avaliacao > 5)
                return new BadRequestObjectResult("A avaliação deve ser entre 1 e 5.");

            var livro = await _livrosCollection
                .Find(l => l.Titulo == Livro)
                .FirstOrDefaultAsync();

            if (livro == null)
                return new NotFoundObjectResult("Livro não encontrado.");

            var usuario = await _usuariosCollection
                .Find(u => u.Nome == Nome)
                .FirstOrDefaultAsync();

            if (usuario == null)
                return new NotFoundObjectResult("Usuário não encontrado.");

            var avaliacaoObj = new Avaliacoes
            {
                LivroId = livro.Id, 
                UsuarioId = usuario.Id, 
                Avaliacao = Avaliacao,
                Comentario = Comentario
            };

            await _avaliacoesCollection.InsertOneAsync(avaliacaoObj);

            return new OkObjectResult(avaliacaoObj);
        }


    }
}
