using System;
using System.Collections.Generic;

namespace Lalena.Database.Entities
{
    public class Resultaat
    {
        public Guid Id { get; set; }

        public string DoorWie { get; set; } = null!;
        public DateTime Wanneer { get; set; }
        
        public string Tafels { get; set; } = null!;
        public string Bewerkingen { get; set; } = null!;

        public int Punten { get; set; }
        public int MaxTeBehalen { get; set; }
        
        public IEnumerable<Fout> Fouten { get; set; } = null!;
    }
}