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

        // concluido 

        [HttpGet("historico/{usuarioId}")]
        public async Task<IActionResult> ObterHistoricoEmprestimos(string usuarioId)
        {
            var historico = await _emprestimosServices.ObterHistoricoEmprestimosAsync(usuarioId);
            var resultado = historico.Select(e => new
            {
                id = e.id,
                livro = e.livroId,
                dataEmprestimo = e.dataEmprestimo,
                dataPrevistaDevolucao = e.dataDevolucaoPrevista,
                dataDevolucao = e.dataDevolucaoReal
            });
            return Ok(resultado);
        }

        // falta testar buscar historico de emprestimos por usuarioId
        [HttpGet("emprestados")]
        public async Task<IActionResult> ObterLivrosEmprestados()
        {
            var emprestados = await _emprestimosServices.ObterLivrosEmprestadosAsync();
            return Ok(emprestados);
        }
        // falta testar o retorno do livro emprestado, se ele está emprestado ou não.

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarEmprestimo(string nome, string titulo)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest("Nome do usuário e título do livro são obrigatórios.");
            }

            var sucesso = await _emprestimosServices.RegistrarEmprestimoAsync(nome, titulo);

            if (sucesso == null || sucesso.ToLower() != "true")
            {
                return NotFound("Não foi possível registrar o empréstimo. Verifique se o livro está disponível ou se os dados estão corretos.");
            }

            return Ok("Empréstimo registrado com sucesso.");
        }
        // Faltar testar o regristro do livro 
        [HttpPost("devolucao/{id}")]
        public async Task<IActionResult> RegistrarDevolucao(int id)
        {
            if (id <= 0)
            {
                return BadRequest("O ID do empréstimo é obrigatório e deve ser válido.");
            }

            var sucesso = await _emprestimosServices.RegistrarDevolucaoAsync(id);
            if (sucesso == null || sucesso.ToLower() != "true")
            {
                return NotFound("Não foi possível registrar a devolução. Verifique se o empréstimo existe ou se os dados estão corretos.");
            }

            return Ok("Devolução registrada com sucesso.");
        }
        // falta testar a devolução do livro 

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
        // falta testar remoção do emprestimos.
    }
}
