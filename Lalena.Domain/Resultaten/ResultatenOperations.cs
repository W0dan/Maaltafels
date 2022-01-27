using System;
using System.Collections.Generic;
using System.Linq;

namespace Lalena.Domain.Resultaten
{
    public static class ResultatenOperations
    {
        public record CreateResultaatParameters(List<int> Tafels, List<string> Bewerkingen, string DoorWie, int Punten,
            int MaxTeBehalen, IEnumerable<CreateResultaatFout> Fouten);

        public record CreateResultaatFout(string Opgave, int CorrectAntwoord, int IngevuldAntwoord);

        public static Resultaat Create(CreateResultaatParameters parameters)
        {
            var tafels = parameters
                .Tafels.Aggregate("", (ts, t) => string.IsNullOrWhiteSpace(ts) ? t.ToString("0") : $"{ts}, {t}");
            var bewerkingen = parameters
                .Bewerkingen.Aggregate("", (bs, b) => string.IsNullOrWhiteSpace(bs) ? b : $"{bs}, {b}");
            var fouten = parameters.Fouten
                .Select(f => new Fout(f.Opgave, f.CorrectAntwoord, f.IngevuldAntwoord))
                .ToList();
            
            var resultaat = new Resultaat(Guid.NewGuid(), tafels, bewerkingen, parameters.DoorWie, DateTime.Now,
                parameters.Punten, parameters.MaxTeBehalen, fouten);

            return resultaat;
        }
    }
}