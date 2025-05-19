using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers
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
        {
          return await _livrosServices.GetAsync();
        }
    }
}
