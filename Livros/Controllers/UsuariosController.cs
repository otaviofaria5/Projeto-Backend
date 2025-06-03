using Microsoft.AspNetCore.Mvc;
using Model;
using Services;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(UsuarioServices usuarioServices) : ControllerBase
    {
        private readonly UsuarioServices _usuarioServices = usuarioServices;

        [HttpGet]
        public async Task<List<Usuario>> GetUsuario()
        {
            return await _usuarioServices.ListarUsuarioAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario(string Nome, string Email, string Endereco)
        {
            var usuario = await _usuarioServices.CadastrarUsuarioAsync(Nome, Email, Endereco);

            return Ok(usuario);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarUsuario(string Id, string Nome, string Email, string Endereco)
        {
            var usuario = await _usuarioServices.AtualizarUsuarioAsync(Id, Nome, Email, Endereco);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(usuario);
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirUsuario(string Id)
        {
            var sucesso = await _usuarioServices.ExcluirUsuarioAsync(Id);

            if (!sucesso) return NotFound("Usuário não encontrado.");

     
            return Ok("Usuário Excluído Com Sucesso.");
        }
    }
}