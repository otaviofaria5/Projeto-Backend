using Microsoft.AspNetCore.Mvc;
using Model;
using Services;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioServices _usuarioServices;

        public UsuarioController(UsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }

        [HttpGet]
        public async Task<List<Usuario>> GetUsuario()
        { 
            return await _usuarioServices.ListarUsuarioAsync();
        }
        // Concluido 
        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario(string nome, string email, string endereco)
        {
            var usuario = await _usuarioServices.CadastrarUsuarioAsync(nome, email, endereco);
            return Ok(usuario);
        }
        // concluido 
        [HttpPut]
        public async Task<IActionResult> AtualizarUsuario(string id, string nome, string email, string endereco)
        {
            var usuario = await _usuarioServices.AtualizarUsuarioAsync(id, nome, email, endereco);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }
            return Ok(usuario);
        }

        // concluido 

        [HttpDelete]
        public async Task<IActionResult> ExcluirUsuario(string id)
        {
            var sucesso = await _usuarioServices.ExcluirUsuarioAsync(id);
            if (!sucesso) return NotFound("Usuário não encontrado.");
            return NoContent();
        }

        // concluido

    }
}