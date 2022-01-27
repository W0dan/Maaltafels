using System;
using System.Collections.Generic;
using System.Linq;
using Lalena.Domain;
using Lalena.Domain.Oefeningen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lalena.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OefeningenController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public GetOefeningenResponse Get([FromQuery] GetOefeningenRequest request) => 
            new(new OefeningenLijst(
                    GetBewerkingen(request), GetTafels(request)).GetAllOefeningen()
                );

        private static IList<int> GetTafels(GetOefeningenRequest request) =>
            request.Tafels.Split(',').Where(IsInt).Select(int.Parse).ToList();

        private static bool IsInt(string str) => 
            int.TryParse(str, out _);

        private static IList<Bewerking> GetBewerkingen(GetOefeningenRequest request) => 
            request.Bewerkingen.Split(',').Select(GetBewerking).ToList();

        private static Bewerking GetBewerking(string bewerking) =>
            bewerking switch
            {
                "x" => Bewerking.Maal,
                ":" => Bewerking.GedeeldDoor,
                _ => throw new Exception("unsupported")
            };
    }

    public class GetOefeningenResponse
    {
        public IEnumerable<Oefening> Oefeningen { get; init; }

        public GetOefeningenResponse()
        {
        }

        public GetOefeningenResponse(IEnumerable<(string opgave, int resultaat)> oefeningen) =>
            Oefeningen = oefeningen.Select(x => new Oefening(x.opgave, x.resultaat));

        public record Oefening(string Opgave, int Resultaat);
    }

    public class GetOefeningenRequest
    {
        public string Bewerkingen { get; set; }
        public string Tafels { get; set; }
    }
}