using Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Services
{
    public class LivrosServices : ControllerBase 
    {
        private readonly IMongoCollection<Livros> _livrosCollection;
        private readonly IMongoCollection<Autores> _autoresCollection;

        public LivrosServices(IOptions<LivrosDatabaseSettings> livroService)
        {
            var mongoClient = new MongoClient(livroService.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(livroService.Value.DatabaseName);
            _livrosCollection = mongoDatabase.GetCollection<Livros>(livroService.Value.CollectionName);
            _autoresCollection = mongoDatabase.GetCollection<Autores>("Autores");
        }

        public async Task<List<Livros>> GetAsync()
        {
            return await _livrosCollection.Find(x => true).ToListAsync();
        }
        public async Task<List<Livros>> PesquisarLivrosAsync(string pesquisar)
        {
            if (string.IsNullOrWhiteSpace(pesquisar))
            {
                return await _livrosCollection.Find(Builders<Livros>.Filter.Empty).ToListAsync();
            }

            var regex = new BsonRegularExpression(pesquisar, "i");

          
            var filtroAutor = Builders<Autores>.Filter.Regex(a => a.Nome, new BsonRegularExpression(pesquisar, "i"));
            var autores = await _autoresCollection.Find(filtroAutor).FirstOrDefaultAsync();

            var filtros = new List<FilterDefinition<Livros>>
            {
               Builders<Livros>.Filter.Regex(l => l.Titulo, regex),
               Builders<Livros>.Filter.Regex(l => l.Genero, regex),
               Builders<Livros>.Filter.Regex(l => l.Isbn, regex)
            };

            if (autores != null && !string.IsNullOrEmpty(autores.Id))
            {
                filtros.Add(Builders<Livros>.Filter.Eq(l => l.AutorId, autores.Id));
            }

            var filtroFinal = Builders<Livros>.Filter.Or(filtros);

            return await _livrosCollection.Find(filtroFinal).ToListAsync();
        }

        public async Task<List<Livros>> ObterLivrosDisponiviesAsync()
        {
            var filter = Builders<Livros>.Filter.Eq(x => x.Disponivel, true);
            return await _livrosCollection.Find(filter).ToListAsync();
        }

        public async Task<List<Livros>> ObterLivrosReservadosAsync()
        {
            var filter = Builders<Livros>.Filter.Eq(x => x.Disponivel, false);
            return await _livrosCollection.Find(filter).ToListAsync();
        }

        public async Task<IActionResult> CriarLivroAsync(string Titulo, string Autor, string Genero, string Isbn, string Descricao)
        {
            var autores = await _autoresCollection.Find(l => l.Nome == Autor).FirstOrDefaultAsync();
            if (autores == null)
                return new NotFoundObjectResult("Autor não encontrado.");

            var livro = new Livros
            {
                Titulo = Titulo,
                AutorId = autores.Id,
                Genero = Genero,
                Isbn = Isbn,
                Descricao = Descricao
            };

            await _livrosCollection.InsertOneAsync(livro);

            return Ok(livro);
        }

        public async Task<IActionResult> AtualizarLivroAsync(string Id, string Titulo, string Autor, string Genero, string Isbn, string Descricao)
        {
            var autores = await _autoresCollection
                .Find(l => l.Nome == Autor)
                .FirstOrDefaultAsync();

            var livro = await _livrosCollection
                .Find(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (livro == null)
                return NotFound("Livro não encontrado.");

            if (autores == null)
                return NotFound("Autor não encontrado.");

            livro.Titulo = Titulo;
            livro.AutorId = autores.Id;
            livro.Genero = Genero;
            livro.Isbn = Isbn;
            livro.Descricao = Descricao;

            await _livrosCollection.ReplaceOneAsync(x => x.Id == Id, livro);

            return Ok(livro);
        }

        public async Task<bool> ExcluirLivroAsync(string Id)
        {
            var livro = await _livrosCollection
                .Find(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (livro == null) return false;

            await _livrosCollection.DeleteOneAsync(x => x.Id == Id);

            return true;
        }
    }
}
