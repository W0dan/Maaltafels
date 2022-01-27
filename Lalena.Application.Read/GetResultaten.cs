using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lalena.Application.Read
{
    public static class GetResultaten
    {
        public class Response
        {
            public List<Resultaat> Resultaten { get; set; } = null!;

            public class Resultaat
            {
                public Guid Id { get; set; }

                public string DoorWie { get; set; } = null!;
                public DateTime Wanneer { get; set; }

                public string Tafels { get; set; } = null!;
                public string Bewerkingen { get; set; } = null!;

                public int Punten { get; set; }
                public int MaxTeBehalen { get; set; }
            }
        }

        public class Handler
        {
            private readonly DbContext _dbContext;

            public Handler(DbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Response> Handle(CancellationToken cancellationToken) =>
                new()
                {
                    Resultaten = await _dbContext.Set<Database.Entities.Resultaat>()
                        .Select(r => new Response.Resultaat
                        {
                            Id = r.Id,
                            Bewerkingen = r.Bewerkingen,
                            Tafels = r.Tafels,
                            Wanneer = r.Wanneer,
                            DoorWie = r.DoorWie,
                            Punten = r.Punten,
                            MaxTeBehalen = r.MaxTeBehalen,
                        })
                        .ToListAsync(cancellationToken)
                };
        }
    }
}