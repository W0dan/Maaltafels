using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lalena.Application.Write.Resultaten;
using Lalena.Domain.Resultaten;
using Microsoft.EntityFrameworkCore;

namespace Lalena.Database.Repositories
{
    public class ResultatenRepository : IResultatenRepository
    {
        private readonly DbContext _dbContext;

        public ResultatenRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Resultaat resultaat, CancellationToken cancellationToken)
        {
            var dbFouten = resultaat.Fouten
                .Select(f => new Entities.Fout());
            var dbResultaat = new Entities.Resultaat
            {
                Id = resultaat.Id,
                DoorWie = resultaat.DoorWie,
                Wanneer = resultaat.Wanneer,
                Punten = resultaat.Punten,
                Tafels = resultaat.Tafels,
                Bewerkingen = resultaat.Bewerkingen,
                MaxTeBehalen = resultaat.MaxTeBehalen,
                Fouten = dbFouten
            };

            _dbContext.Set<Entities.Resultaat>()
                .Add(dbResultaat);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}