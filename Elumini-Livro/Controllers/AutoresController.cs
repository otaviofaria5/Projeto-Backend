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
            => await _autoresServices.GetAsync();

        [HttpPost]
        public async Task<IActionResult> CriarAutores(string nome, string biografia, string nacionalidade)
        {

            var novoAutor = await _autoresServices.CriarAutoresAsync(nome , biografia, nacionalidade);
            return Ok(novoAutor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAutores(string id, Autores autores)
        {
            if (autores.nome == null || autores.biografia == null || autores.nacionalidade == null)
            {
                return BadRequest("Todos os campos (nome, biografia, nacionalidade) são obrigatórios.");
            }

            var autorAtualizado = await _autoresServices.AtualizarAutoresAsync(id, autores.nome, autores.biografia, autores.nacionalidade);
            if (autorAtualizado == null)
            {
                return NotFound("Autor não encontrado.");
            }
            return Ok(autorAtualizado);
        }
    }


}
