using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lalena.UI
{
    /// <summary>
    /// Interaction logic for Resultaat.xaml
    /// </summary>
    public partial class Resultaat : Window
    {
        private readonly int _behaaldePunten;
        private readonly int _totaal;
        private readonly List<((string opgave, int resultaat) oefening, string ingevuld)> _fouten;

        public Resultaat(int behaaldePunten, int totaal, List<((string opgave, int resultaat) oefening, string ingevuld)> fouten)
        {
            _behaaldePunten = behaaldePunten;
            _totaal = totaal;
            _fouten = fouten;
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

            foreach (var ((opgave, resultaat), ingevuld) in _fouten)
            {
                Fouten.Children.Add(new TextBlock { FontSize = 30, Text = $"opgave: {opgave}{resultaat}, ingevuld: {ingevuld}" });
            }
        }
    }
}
