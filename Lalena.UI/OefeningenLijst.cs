﻿using System;
using System.Collections.Generic;
using LalenasFirstProject;

namespace Lalena.UI
{
    public class OefeningenLijst
    {
        private readonly List<(string, int)> _alleOefeningen = new List<(string, int)>();

        public OefeningenLijst(IEnumerable<Bewerking> bewerkingen, IList<int> tafels)
        {
            foreach (var bewerking in bewerkingen)
                switch (bewerking)
                {
                    case Bewerking.Maal:
                        {
                            foreach (var tafel in tafels)
                                for (var i = 0; i <= 10; i++)
                                {
                                    var oefening = $"{tafel}x{i}=";
                                    _alleOefeningen.Add((oefening, tafel * i));
                                }

                            break;
                        }
                    case Bewerking.GedeeldDoor:
                        {
                            foreach (var tafel in tafels)
                                for (var i = 0; i <= 10; i++)
                                {
                                    var oefening = $"{tafel * i}:{tafel}=";
                                    _alleOefeningen.Add((oefening, i));
                                }

                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        public IEnumerable<(string, int)> GetOefeningen(int aantal)
        {
            if (aantal > _alleOefeningen.Count)
                aantal = _alleOefeningen.Count;

            for (var i = 0; i < aantal; i++)
            {
                yield return _alleOefeningen.NeemWillekeurig();
            }
        }

        public List<(string, int)> GetAllOefeningen()
        {
            var result = new List<(string, int)>();
            var oefening = _alleOefeningen.TryNeemWillekeurig();
            while (oefening.HasValue)
            {
                result.Add(oefening.Value);
                oefening = _alleOefeningen.TryNeemWillekeurig();
            }

            return result;
        }
    }
}