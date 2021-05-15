using static System.Console;

namespace LalenasFirstProject
{
    public static class MaaltafelsTonen
    {
        private static void ToonAlleMaaltafels()
        {
            for (var maaltafel = 1; maaltafel <= 10; maaltafel++)
            {
                SchrijfMaaltafelVan(maaltafel);
            }

            ReadLine();
        }

        private static void SchrijfMaaltafelVan(int maaltafel)
        {
            for (var getalleke = 1; getalleke <= 10; getalleke++)
            {
                WriteLine($"{getalleke}x{maaltafel}={getalleke * maaltafel}");
            }

            WriteLine();
        }
    }
}