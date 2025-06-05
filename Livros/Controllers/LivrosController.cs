using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController(LivrosServices livrosServices) : ControllerBase
    {
        private readonly LivrosServices _livrosServices = livrosServices;

        [HttpGet]
        public async Task<List<Livros>> GetStatus()
        {
            return await _livrosServices.GetAsync();
        }

        // CONCLUIDO
        [HttpGet("pesquisar")]
        public async Task<IActionResult> PesquisarLivros(string pesquisar)
        {
            var livros = await _livrosServices.PesquisarLivrosAsync(pesquisar);

            if (livros == null || livros.Count == 0)
                return NotFound("Nenhum livro encontrado com esse título, autor, gênero ou ISBN.");

            return Ok(livros);
        }

        // CONCLUIDO
        [HttpGet("disponiveis")]
        public async Task<IActionResult> GetLivrosDisponiveis()
        {
            var livros = await _livrosServices.ObterLivrosDisponiviesAsync();
            return Ok(livros);
        }

        [HttpGet("reservados")]
        public async Task<IActionResult> GetLivrosReservados()
        {
            var livros = await _livrosServices.ObterLivrosReservadosAsync();
            return Ok(livros);
        }


        [HttpPost]
        public async Task<IActionResult> CriarLivro(string Titulo, string Autor, string Genero, string Isbn, string Descricao)
        {
            var livro = await _livrosServices.CriarLivroAsync(Titulo, Autor, Genero, Isbn, Descricao);

            return Ok(livro);
        }


        [HttpPut]
        public async Task<IActionResult> AtualizarLivro(string Id, string Titulo, string Autor, string Genero, string Isbn, string Descricao)
        {
            var livroAtualizado = await _livrosServices.AtualizarLivroAsync(Id, Titulo, Autor, Genero, Isbn, Descricao);
            if (livroAtualizado == null)
            {
                return NotFound("Livro não encontrado.");
            }

            return Ok(livroAtualizado);
        }


        [HttpDelete]
        public async Task<IActionResult> ExcluirLivro(string Id)
        {
            var livroDeletado = await _livrosServices.ExcluirLivroAsync(Id);

            if (!livroDeletado)
            {
                return NotFound("Livro não encontrado.");
            }

            return Ok("Livro Excluido Com Sucesso");
        }
     
    }
}
