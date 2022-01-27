using System.Threading;
using System.Threading.Tasks;

namespace Lalena.Application.Write.Resultaten
{
    public interface IResultatenRepository
    {
        Task Add(Domain.Resultaten.Resultaat resultaat, CancellationToken cancellationToken);
    }
}