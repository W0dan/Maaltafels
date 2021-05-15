using System;
using System.Diagnostics;

namespace LalenasFirstProject
{
    public class OefeningenPrinter
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Stopwatch _stopwatchPerOefening = new Stopwatch();

        public OefeningenPrinter()
        {
            _stopwatch.Start();
            _stopwatchPerOefening.Start();
        }

        public void Print(string oefening)
        {
            var perOefening = _stopwatchPerOefening.Elapsed > new TimeSpan(0, 0, 1, 0)
                ? $"{_stopwatchPerOefening.Elapsed.Minutes}:{_stopwatchPerOefening.Elapsed.Seconds}"
                : $"{_stopwatchPerOefening.Elapsed.Seconds}";

            var total = $"{_stopwatch.Elapsed.Minutes}:{_stopwatch.Elapsed.Seconds}";

            Console.Write($"{total} {perOefening} {oefening}");
            _stopwatchPerOefening.Restart();
        }
    }
}