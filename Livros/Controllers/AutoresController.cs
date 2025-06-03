using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController(AutoresServices autoresServices) : ControllerBase
    {
        private readonly AutoresServices _autoresServices = autoresServices;


        [HttpGet]
        public async Task<List<Autores>> GetAutores()
        {
            return await _autoresServices.GetAsync();
        }

        
        [HttpGet("pesquisar")]
        public async Task<IActionResult> BuscarAutoresPorLetra(string pesquisar)
        {
            var autores = await _autoresServices.PesquisarAutorAsync(pesquisar);

            if (autores == null || autores.Count == 0)
                return NotFound("Nenhum autor encontrado.");

            return Ok(autores);
        }
       

        [HttpPost]
        public async Task<IActionResult> CadastrarAutor(string Nome, string Biografia, string Nacionalidade)
        {
            var novoAutores = await _autoresServices.CadastrarAutorAsync(Nome, Biografia, Nacionalidade);
            return Ok(novoAutores);
        } 

        [HttpPut]
        public async Task<IActionResult> AtualizarAutor(string Id, string Nome, string Biografia, string Nacionalidade)
        {
            var autoresAtualizado = await _autoresServices.AtualizarAutorAsync(Id, Nome, Biografia, Nacionalidade);

            if (autoresAtualizado == null)
            {
                return NotFound("Autor não encontrado.");
            }

            return Ok(autoresAtualizado);
        }
   

        [HttpDelete]
        public async Task<IActionResult> ExcluirAutor(string Id)
        {
            var autoresDeletado = await _autoresServices.ExcluirAutorAsync(Id);
            if (!autoresDeletado)
            {
                return NotFound("Autor não encontrado.");
            }

            return Ok("Autor excluído com sucesso.");
        }
    }
}
