using System;
using System.Collections.Generic;
using static System.Console;

namespace LalenasFirstProject
{
    public class OefeningenMaaltafelsGeenDubbels
    {
        private readonly OefeningenPrinter _printer;
        private readonly List<(string opgave, int uitkomst)> _alleOefeningen = new List<(string opgave, int uitkomst)>();

        public OefeningenMaaltafelsGeenDubbels(IReadOnlyCollection<int> tafels, IEnumerable<Bewerking> bewerkingen, OefeningenPrinter printer)
        {
            _printer = printer;
            MaakOefeningenLijst(tafels, bewerkingen);
        }

        private void MaakOefeningenLijst(IReadOnlyCollection<int> tafels, IEnumerable<Bewerking> bewerkingen)
        {
            foreach (var bewerking in bewerkingen)
            {
                switch (bewerking)
                {
                    case Bewerking.Maal:
                        {
                            foreach (var tafel in tafels)
                            {
                                for (var i = 1; i <= 10; i++)
                                {
                                    var oefening = $"{tafel}x{i}=";
                                    _alleOefeningen.Add((oefening, tafel * i));
                                }
                            }

                            break;
                        }
                    case Bewerking.GedeeldDoor:
                        {
                            foreach (var tafel in tafels)
                            {
                                for (var i = 1; i <= 10; i++)
                                {
                                    var oefening = $"{tafel * i}:{tafel}=";
                                    _alleOefeningen.Add((oefening, i));
                                }
                            }

                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public (int punten, int totaalPunten) Start()
        {
            var punten = 0;

            var aantalOefeningen = _alleOefeningen.Count;
            for (var i = 0; i < aantalOefeningen; i++)
            {
                var oefening = _alleOefeningen.NeemWillekeurig();

                //WriteLine(oefening.opgave);
                _printer.Print(oefening.opgave);
                var uitkomst = ConsoleHelper.VraagUitkomst();

                if (uitkomst == oefening.uitkomst)
                {
                    punten++;
                    WriteLine("Juist !");
                }
                else
                {
                    WriteLine("Fout !");
                    WriteLine($"het juiste antwoord is: {oefening.uitkomst}");
                }
            }

            return (punten, aantalOefeningen);
        }
    }
}