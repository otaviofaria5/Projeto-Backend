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
        [HttpGet("historico")]
        public async Task<IActionResult> ObterHistoricoEmprestimos()
        {
            var historico = await _emprestimosServices.ObterHistoricoAsync();

            return Ok(historico);
        }


        [HttpGet("lista")]
        public async Task<IActionResult> ObterLivrosEmprestados()
        {
            var emprestados = await _emprestimosServices.ObterLivrosEmprestadosAsync();

            return Ok(emprestados);
        }


        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarEmprestimo(string Nome, string Titulo, DateTimeOffset DataEmprestimo, DateTimeOffset DataDevolucaoPrevista)
        {
            var resultado = await _emprestimosServices.RegistrarEmprestimoAsync(Nome, Titulo, DataEmprestimo, DataDevolucaoPrevista );

            if (resultado == null)
            {
                return NotFound("Usuário ou livro não encontrado.");
            }

            return Ok(new { mensagem = (resultado as ObjectResult)?.Value });
        }

     

        [HttpPost("devolver")]
        public async Task<IActionResult> RegistrarDevolucao(string Nome, string Livro)
        {
            if (string.IsNullOrWhiteSpace(Livro) || string.IsNullOrWhiteSpace(Nome))
            {
                return BadRequest("Nome do livro e nome do usuário são obrigatórios.");
            }

            var resultado = await _emprestimosServices.RegistrarDevolucaoAsync(Nome, Livro);

            if (resultado is ObjectResult objectResult && objectResult.Value is string mensagem &&
                (mensagem.StartsWith("Livro não encontrado") ||
                 mensagem.StartsWith("Usuário não encontrado") ||
                 mensagem.StartsWith("Empréstimo inválido")))
            {
                return NotFound(mensagem);
            }

            return Ok(new { mensagem = (resultado as ObjectResult)?.Value });
        }


    }
}
