using Microsoft.AspNetCore.Mvc;
using Model;        
using Services;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacoesController(AvaliacoesServices avaliacoesServices) : ControllerBase
    {
        private readonly AvaliacoesServices _avaliacoesServices = avaliacoesServices;

        [HttpGet]
        public async Task<List<Avaliacoes>> GetAvaliacoes()
        {
            return await _avaliacoesServices.GetAsync();
        }

        [HttpGet("exibir")]
        public async Task<IActionResult> ExibirLivro(string Livro)
        {
            try
            {
                var avaliacao = await _avaliacoesServices.ExibirLivroAsync(Livro);
                return Ok(avaliacao);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CadatrarAvaliacao(string Livro, string Nome, int Avaliacao, string Comentario)
        {
            try
            {
                var resultado = await _avaliacoesServices.CadastrarAvaliacaoAsync(Livro, Nome, Avaliacao, Comentario);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
