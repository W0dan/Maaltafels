using System.Collections.Generic;
using static System.Console;

namespace LalenasFirstProject
{
    public class OefeningenMaaltafelsWillekeurig
    {
        private readonly List<int> _tafels;
        private readonly List<Bewerking> _bewerkingen;

        public OefeningenMaaltafelsWillekeurig(List<int> tafels, List<Bewerking> bewerkingen)
        {
            _tafels = tafels;
            _bewerkingen = bewerkingen;
        }

        public int Start()
        {
            var punten = 0;

            for (var i = 0; i < 10; i++)
            {
                var getal1 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.KiesWillekeurig();
                var getal2 = _tafels.KiesWillekeurig();
                var bewerking = _bewerkingen.KiesWillekeurig();

                SchrijfBewerking(getal1, bewerking, getal2);
                var uitkomst = ConsoleHelper.VraagUitkomst();

                if (IsUitkomstJuist(getal1, getal2, bewerking, uitkomst))
                {
                    punten++;
                    WriteLine("Juist !");
                }
                else
                {
                    WriteLine("Fout !");
                    if (bewerking == Bewerking.Maal)
                        WriteLine($"het juiste antwoord is: {getal1 * getal2}");
                    if (bewerking == Bewerking.GedeeldDoor)
                        WriteLine($"het juiste antwoord is: {getal1}");
                }
            }

            return punten;
        }

        private static void SchrijfBewerking(int getal1, Bewerking bewerking, int getal2)
        {
            if (bewerking == Bewerking.Maal)
                Write($"{getal1}x{getal2}=");
            if (bewerking == Bewerking.GedeeldDoor)
                Write($"{getal1 * getal2}:{getal2}=");
        }

        private static bool IsUitkomstJuist(int getal1, int getal2, Bewerking bewerking, int? uitkomst)
            => bewerking == Bewerking.Maal
                ? getal1 * getal2 == uitkomst
                : getal1 == uitkomst;
    }
}