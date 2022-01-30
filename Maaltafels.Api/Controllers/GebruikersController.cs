using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Maaltafels.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GebruikersController : ControllerBase
    {
        [HttpGet]
        public async Task GetGebruikers()
        {
            await Task.CompletedTask;
        }

        [HttpPost]
        public async Task SaveGebruiker(string gebruikersnaam)
        {
            await Task.CompletedTask;
        }
    }
}