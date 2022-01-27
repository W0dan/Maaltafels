using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lalena.Domain.Resultaten;

namespace Lalena.Application.Write.Resultaten
{
    public static class SaveResultaat
    {
        public class SaveResultaatRequest
        {
            public List<int> Tafels { get; set; } = null!;
            public List<string> Bewerkingen { get; set; } = null!;

            public string DoorWie { get; set; } = null!;

            public int Punten { get; set; }
            public int MaxTeBehalen { get; set; }

            public List<SaveResultaatFout> Fouten { get; set; } = null!;
        }

        public class SaveResultaatFout
        {
            public string Opgave { get; set; } = null!;
            public int CorrectAntwoord { get; set; }
            public int IngevuldAntwoord { get; set; }
        }

        public class Handler
        {
            private readonly IResultatenRepository _repository;

            public Handler(IResultatenRepository repository)
            {
                _repository = repository;
            }
            
            public async Task Handle(SaveResultaatRequest request, CancellationToken cancellationToken)
            {
                var fouten = request.Fouten
                    .Select(f => new ResultatenOperations.CreateResultaatFout(f.Opgave, f.CorrectAntwoord, f.IngevuldAntwoord));
                var parameters = new ResultatenOperations.CreateResultaatParameters(request.Tafels, request.Bewerkingen,
                    request.DoorWie, request.Punten, request.MaxTeBehalen, fouten);

                var resultaat = ResultatenOperations.Create(parameters);
                
                await _repository.Add(resultaat, cancellationToken);
            }
        }
    }
}