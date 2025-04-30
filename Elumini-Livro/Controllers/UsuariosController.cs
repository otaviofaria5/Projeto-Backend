using Microsoft.AspNetCore.Mvc;
using Projeto_Backend.Services;
using Projeto_Backend.Model;
using static Projeto_Backend.Services.UsuariosService;


namespace Projeto_Backend.Controllers
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
            => await _usuarioServices.GetAsync();

        [HttpPost]
        public async Task<Usuario> PostUsuario(Usuario usuario)
        {
            await _usuarioServices.CreateAsync(usuario);

            return usuario;
        }

        [HttpPut]
        public async Task<Usuario> PutUsuario(string id, Usuario usuario)
        {
            await _usuarioServices.UpdateAsync(id, usuario);
            return usuario;
        }

        [HttpDelete]
        public async Task<Usuario> DeleteUsuario(string id, Usuario usuario)
        {
            await _usuarioServices.RemoveAsync(id);
            return usuario;

        }

    }
}