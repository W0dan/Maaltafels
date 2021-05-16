using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lalena.UI
{
    /// <summary>
    /// Interaction logic for Resultaat.xaml
    /// </summary>
    public partial class Resultaat
    {
        private readonly int _behaaldePunten;
        private readonly int _totaal;
        private readonly List<((string opgave, int resultaat) oefening, string ingevuld)> _fouten;
        private readonly (TimeSpan totalTime, TimeSpan averageExerciseTime, TimeSpan minimumExerciseTime, TimeSpan maximumExerciseTime) _timings;

        public Resultaat(
            int behaaldePunten,
            int totaal,
            List<((string opgave, int resultaat) oefening, string ingevuld)> fouten,
            (TimeSpan totalTime, TimeSpan averageExerciseTime, TimeSpan minimumExerciseTime, TimeSpan maximumExerciseTime) timings)
        {
            _behaaldePunten = behaaldePunten;
            _totaal = totaal;
            _fouten = fouten;
            _timings = timings;
            InitializeComponent();
        }

        private void Resultaat_OnLoaded(object sender, RoutedEventArgs e)
        {
            ToonPunten();
        }

        private void ToonPunten()
        {
            var percent = _totaal == 0 ? 0 : (double)_behaaldePunten / _totaal;
            if (percent > 0.9)
            {
                Punten.Foreground = new SolidColorBrush(Colors.Green);
            }
            else if (percent > 0.5)
            {
                Punten.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                Punten.Foreground = new SolidColorBrush(Colors.Red);
            }
            Punten.Text = $"Behaalde punten: {_behaaldePunten} op {_totaal}";

            TotaleTijd.Text = $"Totale tijd: {GetTijdText(_timings.totalTime)}";
            GemiddeldeTijd.Text = $"Gemiddelde tijd: {GetTijdText(_timings.averageExerciseTime)}";
            Snelste.Text = $"Snelste tijd: {GetTijdText(_timings.minimumExerciseTime)}";
            MinstSnelle.Text = $"Minst snelle tijd: {GetTijdText(_timings.maximumExerciseTime)}";

            foreach (var ((opgave, resultaat), ingevuld) in _fouten)
            {
                Fouten.Children.Add(new TextBlock { FontSize = 30, Text = $"opgave: {opgave}{resultaat}, ingevuld: {ingevuld}" });
            }
        }

        private static string GetTijdText(TimeSpan tijd)
        {
            if (tijd.TotalSeconds < 60)
                return $"{tijd.TotalSeconds.Round(1)} sec.";

            var minuten = tijd.Minutes;
            var seconden = tijd.TotalSeconds - (60 * minuten);

            return $"{minuten} min. {seconden.Round(1)} sec.";
        }
    }
}
