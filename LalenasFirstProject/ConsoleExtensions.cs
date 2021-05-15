using System;

namespace LalenasFirstProject
{
    public static class ConsoleHelper
    {
        public static int? VraagUitkomst()
            => int.TryParse(Console.ReadLine(), out var resultaat)
                ? (int?)resultaat
                : null;

    }
}