using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController(AutoresServices autoresServices) : ControllerBase
    {
        private readonly AutoresServices _autoresServices = autoresServices;

        [HttpGet]
        public async Task<List<Autores>> GetAutores()
            => await _autoresServices.GetAsync();
    }


}
