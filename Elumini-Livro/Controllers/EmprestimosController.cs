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
        // Concluido
        [HttpGet("historico")]
        public async Task<IActionResult> ObterHistoricoPorNome(string nome)
        {
            var historico = await _emprestimosServices.ObterHistoricoPorNomeAsync(nome);
            var resultado = historico.Select(e => new
            {
                Livro = e.livroId, 
                e.dataEmprestimo,
                DataPrevistaDevolucao = e.dataDevolucaoPrevista,
                e.dataDevolucaoReal
            });
            return Ok(resultado);
        }
        // Concluido

        [HttpGet("emprestados")]
        public async Task<IActionResult> ObterLivrosEmprestados()
        {
            var emprestados = await _emprestimosServices.ObterLivrosEmprestadosAsync();
            return Ok(emprestados);
        }

        // Concluido

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarEmprestimo(string nome, string titulo)
        {
            var resultado = await _emprestimosServices.RegistrarEmprestimoAsync(nome, titulo);
            if (resultado == null)
            {
                return NotFound("Usuário ou livro não encontrado.");
            }
            return Ok(new { mensagem = resultado });
        }
        [HttpPost("devolverPorTitulo")]
        public async Task<IActionResult> RegistrarDevolucao(string titulo)
        {
            var resultado = await _emprestimosServices.RegistrarDevolucaoPorTituloAsync(titulo);
            return Ok(new { mensagem = resultado });
        }

        // duvida pendente com Danielle



        [HttpPost("devolver/id")]
        public async Task<IActionResult> RegistrarDevolucaoPorId(string emprestimoId)
        {
            var resultado = await _emprestimosServices.RegistrarDevolucaoAsync(emprestimoId);

            if (resultado == "Empréstimo inválido ou já devolvido.")
            {
                return NotFound(new { mensagem = resultado });
            }

            return Ok(new { mensagem = resultado });
        }

        // duvida pendente com Danielle

        [HttpDelete]
        public async Task<IActionResult> DeleteEmprestimos(string id)
        {
            var existingEmprestimo = await _emprestimosServices.GetAsync();
            if (existingEmprestimo == null)
            {
                return NotFound();
            }
            await _emprestimosServices.ExlcuirEmprestimosAsync(id);
            return NoContent();
        }

        // Concluido
    }
}
