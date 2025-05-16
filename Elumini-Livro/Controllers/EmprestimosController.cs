using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimosController : ControllerBase
    {
        private readonly EmprestimosServices _emprestimosServices;

        public EmprestimosController(EmprestimosServices emprestimosServices)
        {
            _emprestimosServices = emprestimosServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<Emprestimos>>> GetEmprestimos()
        {
            var emprestimos = await _emprestimosServices.GetAsync();
            return Ok(emprestimos);
        }

        [HttpGet("/{usuarioId}")]
        public async Task<ActionResult<List<Emprestimos>>> GetPorUsuario(string usuarioId)
        {
            var emprestimos = await _emprestimosServices.GetPorUsuarioAsync(usuarioId);
            return Ok(emprestimos);
        }

        [HttpPost]
        public async Task<IActionResult> PostEmprestimos(Emprestimos emprestimos)
        {
            await _emprestimosServices.CriarAsync(emprestimos);
            return CreatedAtAction(nameof(GetEmprestimos), new { id = emprestimos.id }, emprestimos);
        }

        [HttpPost]
        public async Task<IActionResult> RegristrarEmprestimo(string usuarioId, string livroId)
        {
            var emprestimos = await _emprestimosServices.RegistrarEmprestimoAsync(usuarioId, livroId);

            if (emprestimos == null)
                return BadRequest("Usuário ou livro inválido, ou o livro já está emprestado.");

            return Ok(emprestimos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmprestimos(string id, Emprestimos emprestimos)
        {
            var existingEmprestimo = await _emprestimosServices.GetAsync();
            if (existingEmprestimo == null)
            {
                return NotFound();
            }
            if (emprestimos.dataDevolucaoReal.HasValue)
            {
                await _emprestimosServices.RegristarDevolucaoAsync(id, emprestimos.dataDevolucaoReal.Value.DateTime);
            }
            else
            {
                return BadRequest("Data de devolução real é obrigatória.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmprestimos(string id)
        {
            var existingEmprestimo = await _emprestimosServices.GetAsync();
            if (existingEmprestimo == null)
            {
                return NotFound();
            }
            await _emprestimosServices.RemoveAsync(id);
            return NoContent();
        }
    }
}
