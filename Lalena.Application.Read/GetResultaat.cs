using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lalena.Application.Read
{
    public static class GetResultaat
    {
        public class GetResultaatResponse
        {
            public Guid Id { get; init; }

            public string DoorWie { get; init; } = null!;
            public DateTime Wanneer { get; init; }

            public string Tafels { get; init; } = null!;
            public string Bewerkingen { get; init; } = null!;

            public int Punten { get; init; }
            public int MaxTeBehalen { get; init; }

            public IEnumerable<GetResultaatFout> Fouten { get; init; } = null!;
        }

        public class GetResultaatFout
        {
            public string Opgave { get; init; } = null!;
            public int CorrectAntwoord { get; init; }
            public int IngevuldAntwoord { get; init; }
        }

        public class Handler
        {
            private readonly DbContext _dbContext;

            public Handler(DbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<GetResultaatResponse> Handle(Guid id, CancellationToken cancellationToken) =>
                await _dbContext.Set<Database.Entities.Resultaat>()
                    .Select(r => new GetResultaatResponse
                    {
                        Id = r.Id,
                        Bewerkingen = r.Bewerkingen,
                        Tafels = r.Tafels,
                        Wanneer = r.Wanneer,
                        DoorWie = r.DoorWie,
                        Punten = r.Punten,
                        MaxTeBehalen = r.MaxTeBehalen,
                        Fouten = r.Fouten.Select(f => new GetResultaatFout
                        {
                            Opgave = f.Opgave,
                            CorrectAntwoord = f.CorrectAntwoord,
                            IngevuldAntwoord = f.IngevuldAntwoord
                        })
                    })
                    .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}