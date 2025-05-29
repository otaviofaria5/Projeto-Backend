using Microsoft.AspNetCore.Mvc;
using Model;        
using Services;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly AvaliacoesServices _avaliacoesServices;

        public AvaliacoesController(AvaliacoesServices avaliacoesServices)
        {
            _avaliacoesServices = avaliacoesServices;
        }

        [HttpGet]
        public async Task<List<Avaliacoes>> GetAvaliacoes()
        {
            return await _avaliacoesServices.GetAsync();
        }

        //CONCLUIDO

        [HttpGet("exibir")]
        public async Task<IActionResult> ExibirLivro(string nomeLivro)
        {
            try
            {
                var avaliacao = await _avaliacoesServices.ExibirLivroAsync(nomeLivro);
                return Ok(avaliacao);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //CONLCUIDO 

        [HttpPost]
        public async Task<IActionResult> CadatrarAvaliacao(string nomeLivro, string nomeUsuario, int avaliacao, string comentario)
        {
            try
            {
                var resultado = await _avaliacoesServices.CadastrarAvaliacaoAsync(nomeLivro, nomeUsuario, avaliacao, comentario);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // CONCLUIDO 


        [HttpDelete]
        public async Task<IActionResult> ExcluirAvaliacao(string id)
        {
            var resultado = await _avaliacoesServices.ExcluirAvaliacaoAsync(id);
            if (!resultado)
            {
                return NotFound("Avaliação não encontrada.");
            }
            return Ok("Avaliação excluída com sucesso.");
        }

        //DUVIDA PRETENDE COM DANI
    }
}
