using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Lalena.UI
{
    /// <summary>
    /// Interaction logic for OneByOne.xaml
    /// </summary>
    public partial class OneByOne : Window
    {
        private (string opgave, int resultaat) _oefening;
        private List<(string opgave, int resultaat)> _alleOefeningen;
        private bool _isFout = false;
        private int _totaal;
        private int _aantalGedaan = 0;
        private int _behaaldePunten = 0;
        private bool _voltooid = false;
        private readonly List<((string opgave, int resultaat) oefening, string ingevuld)> _fouten = new List<((string opgave, int resultaat) oefening, string ingevuld)>();

        public OneByOne()
        {
            InitializeComponent();
        }

        public void ShowOneByOne(List<(string opgave, int resultaat)> alleOefeningen)
        {
            _alleOefeningen = alleOefeningen;
            _totaal = _alleOefeningen.Count;
            Progress.Maximum = _totaal;
            NextOefening();

            ShowDialog();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (_voltooid)
            {
                Close();
            }

            if (_isFout)
            {
                NextOefening();
                return;
            }

            if (!int.TryParse(Resultaat.Text.Trim(), out var resultaat))
            {
                SetFout(Resultaat.Text);
                return;
            }

            if (_oefening.resultaat != resultaat)
            {
                SetFout(resultaat.ToString());
                return;
            }

            _behaaldePunten++;
            NextOefening();
        }

        private void NextOefening()
        {
            UpdateProgress();

            if (_aantalGedaan >= _totaal)
            {
                ToonPunten();
                return;
            }

            _oefening = _alleOefeningen.Skip(_aantalGedaan).Take(1).Single();

            Resultaat.Text = "";
            Verbetering.Text = "";
            Opgave.Text = _oefening.opgave;
            _aantalGedaan++;
            _isFout = false;
        }

        private void UpdateProgress()
        {
            Progress.Value = _aantalGedaan;
        }

        private void ToonPunten()
        {
            _voltooid = true;

            Opgave.Text = "";
            Resultaat.Text = "";
            Resultaat.IsEnabled = false;

            var resultaatDialog = new Resultaat(_behaaldePunten, _totaal, _fouten);

            resultaatDialog.ShowDialog();

            Close();
        }

        private void SetFout(string ingevuld)
        {
            _isFout = true;
            _fouten.Add((oefening: _oefening, ingevuld));
            Verbetering.Foreground = new SolidColorBrush(Colors.Red);
            Verbetering.Text = $"FOUT !  het juiste antwoord is {_oefening.resultaat:0}";
        }
    }
}
