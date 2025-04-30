using Microsoft.AspNetCore.Mvc;
using Projeto_Backend.Services;
using Projeto_Backend.Model;

namespace Projeto_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly LivrosServices _livrosServices;
        public LivrosController(LivrosServices livrosServices)
        {
            _livrosServices = livrosServices;
        }
   
        [HttpGet]
        public async Task<List<Livros>> GetStatus()
            => await _livrosServices.GetAsync();
        [HttpPost]
        public async Task<Livros>PostStatus(Livros livros)
        {
            await _livrosServices.CreateAsync(livros);
            return livros;
        }

        [HttpPut]
        public async Task<Livros> PutStatus(string id, Livros livros)
        {
            await _livrosServices.UpdateAsync(id, livros);
            return livros;
        }

        [HttpDelete]
        public async Task<Livros> DeleteLivros(string id, Livros livros)
        {
            await _livrosServices.RemoveAsync(id);
            return livros;

        }

    }
}
