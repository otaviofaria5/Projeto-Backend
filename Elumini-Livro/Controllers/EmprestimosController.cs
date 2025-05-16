using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimosController(EmprestimosServices emprestimosServices) : ControllerBase
    {
        private readonly EmprestimosServices _emprestimosServices = emprestimosServices;

        [HttpGet]
        public async Task<ActionResult<List<Emprestimos>>> GetEmprestimos()
        {
            var emprestimos = await _emprestimosServices.GetAsync();
            return Ok(emprestimos);
        }

        [HttpGet("/{usuarioId}")]
        public async Task<ActionResult<List<Emprestimos>>> GetPorUsuario(string usuario)
        {
            var emprestimos = await _emprestimosServices.GetPorUsuarioAsync(usuario);
            return Ok(emprestimos);
        }

        [HttpPost]
        public async Task<IActionResult> PostEmprestimos(Emprestimos emprestimos)
        {
            await _emprestimosServices.CriarAsync(emprestimos);
            return CreatedAtAction(nameof(GetEmprestimos), new { id = emprestimos.id }, emprestimos);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarEmprestimo(string nome, string titulo)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest("Nome do usuário e título do livro são obrigatórios.");
            }

            var sucesso = await _emprestimosServices.RegistrarEmprestimoAsync(nome, titulo);

            if (!sucesso)
            {
                return NotFound("Não foi possível registrar o empréstimo. Verifique se o livro está disponível ou se os dados estão corretos.");
            }

            return Ok("Empréstimo registrado com sucesso.");
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
