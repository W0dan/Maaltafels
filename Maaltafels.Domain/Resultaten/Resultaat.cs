using System;
using System.Collections.Generic;

namespace Maaltafels.Domain.Resultaten
{
    public record Resultaat(Guid Id, string Tafels, string Bewerkingen, string DoorWie, DateTime Wanneer, int Punten,
        int MaxTeBehalen, List<Fout> Fouten);
}