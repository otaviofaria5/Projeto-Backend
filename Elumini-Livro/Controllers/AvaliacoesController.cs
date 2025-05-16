using Microsoft.AspNetCore.Mvc;
using Model;        
using Services;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacoesController(AvaliacoesServices avaliacoesServices) : ControllerBase
    {
        private readonly AvaliacoesServices _avaliacoesServices = avaliacoesServices;

        [HttpGet]
        public async Task<List<Avaliacoes>> GetAvaliacoes()
            => await _avaliacoesServices.GetAsync();
    }
}
