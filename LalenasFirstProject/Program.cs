using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace LalenasFirstProject
{
    class Program
    {
        static void Main()
        {
            Clear();
            var bewerkingen = VraagBewerkingen().Distinct().ToList();
            var tafels = VraagTafels().ToList();

            var puntenLijst = new List<(int punten, int totaalPunten)>();

            var oefeningenProvider = new OefeningenMaaltafelsGeenDubbels(tafels, bewerkingen, new OefeningenPrinter());

            while (true)
            {
                Clear();

                SchrijfOpgave(tafels, bewerkingen);

                var punten = oefeningenProvider.Start();

                puntenLijst.Add(punten);

                WriteLine();
                for (var index = 0; index < puntenLijst.Count; index++)
                {
                    var punt = puntenLijst[index];
                    WriteLine($"{index + 1}) Punten: {punt}/{punt.totaalPunten}");
                }

                WriteLine("\n\nOpnieuw ? (n = nee)");
                var opnieuw = ReadLine();
                if (opnieuw?.Trim().ToLower() == "n")
                    break;
            }
        }

        private static IEnumerable<Bewerking> VraagBewerkingen()
        {
            WriteLine("Wil je maal of gedeeld door doen, of beide ?");
            WriteLine("maal= x");
            WriteLine("gedeeld door= :");
            WriteLine("beide= x, :");
            WriteLine();
            var invoer = ReadLine() ?? string.Empty;
            var invoerLijst = invoer
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());
            foreach (var invoerItem in invoerLijst)
            {
                if (invoerItem.ToLower() == "x") yield return Bewerking.Maal;
                if (invoerItem.ToLower() == ":") yield return Bewerking.GedeeldDoor;
            }
            WriteLine();
        }

        private static IEnumerable<int> VraagTafels()
        {
            WriteLine("Welke maaltafels wil je doen ?");
            WriteLine("Bijvoorbeeld: 1, 2, 5");
            WriteLine();
            var invoer = ReadLine() ?? string.Empty;

            var invoerLijst = invoer
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());
            foreach (var invoerItem in invoerLijst)
            {
                if (int.TryParse(invoerItem, out var maaltafel))
                    yield return maaltafel;
            }
            WriteLine();
        }

        private static void SchrijfOpgave(List<int> tafels, IEnumerable<Bewerking> bewerkingen)
        {
            static string LocalSchrijfBewerking(Bewerking bewerking)
                => bewerking == Bewerking.Maal ? "maaltafels" : "deeltafels";

            var opgave = bewerkingen.Aggregate(string.Empty, (current, bewerking) => current == string.Empty
                ? $"Alle {LocalSchrijfBewerking(bewerking)}"
                : $"{current} en alle {LocalSchrijfBewerking(bewerking)}");
            WriteLine($"{opgave} van {LijstAlsTekst(tafels)}");
            WriteLine();
        }

        private static string LijstAlsTekst<T>(List<T> lijst)
        {
            if (lijst.Count == 0)
                return string.Empty;

            var resultaat = lijst[0].ToString();
            if (lijst.Count == 1)
                return resultaat;

            for (var i = 1; i < lijst.Count - 1; i++)
            {
                resultaat = $"{resultaat}, {lijst[i]}";
            }

            return $"{resultaat} en {lijst[^1]}";
        }
    }
}
