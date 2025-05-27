using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly AutoresServices _autoresServices;

        public AutoresController(AutoresServices autoresServices)
        {
            _autoresServices = autoresServices;
        }

        [HttpGet]
        public async Task<List<Autores>> GetAutores()
        {
        return await _autoresServices.GetAsync();
        }
        //CONLCUIDO

        [HttpGet("pesquisar")]
        public async Task<IActionResult> PesquisarAutores(string nome)
        {
            var autores = await _autoresServices.PesquisarAutoresAsync(nome);
            if (autores != null)
            {

                return Ok(autores);

            }
            return BadRequest("Nenhum autor encontrado");

        }
        //CONLCUIDO

        [HttpPost]
        public async Task<IActionResult> CriarAutores(string nome, string biografia, string nacionalidade)
        {
            var novoAutores = await _autoresServices.CriarAutoresAsync(nome, biografia, nacionalidade);
            return Ok(novoAutores);
        }
        //CONLCUIDO

        [HttpPut]
        public async Task<IActionResult> AtualizarAutores(string id ,string nome, string biografia, string nacionalidade)
        {

            var autoresAtualizado = await _autoresServices.AtualizarAutoresAsync(id, nome, biografia, nacionalidade);

            if (autoresAtualizado == null)
            {
                return NotFound("Autor não encontrado.");
            }

            return Ok(autoresAtualizado);
        }
        //CONLCUIDO

        [HttpDelete]
        public async Task<IActionResult> ExcluirAutores(string id)
        {
            var autoresDeletado = await _autoresServices.ExcluirAutoresAsync(id);
            if (!autoresDeletado)
            {
                return NotFound("Autor não encontrado.");
            }
            return Ok(autoresDeletado);
        }
        //CONLCUIDO
    }
}
