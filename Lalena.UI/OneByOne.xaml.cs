using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using static System.Int32;

namespace Lalena.UI
{
    /// <summary>
    /// Interaction logic for OneByOne.xaml
    /// </summary>
    public partial class OneByOne
    {
        private (string opgave, int resultaat) _oefening;
        private List<(string opgave, int resultaat)> _alleOefeningen;
        private bool _isFout = false;
        private int _totaal;
        private int _aantalGedaan = 0;
        private int _behaaldePunten = 0;
        private readonly List<((string opgave, int resultaat) oefening, string ingevuld)> _fouten = new List<((string opgave, int resultaat) oefening, string ingevuld)>();

        private readonly Stopwatch _overallStopwatch = new Stopwatch();
        private readonly Stopwatch _excerciseStopwatch = new Stopwatch();
        private TimeSpan _minumumExerciseTime = new TimeSpan(MaxValue);
        private TimeSpan _maximumExerciseTime;

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

            _overallStopwatch.Start();

            ShowDialog();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isFout)
            {
                NextOefening();
                return;
            }

            if (!TryParse(Resultaat.Text.Trim(), out var resultaat))
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
            if (_excerciseStopwatch.IsRunning)
            {
                var duurtijd = _excerciseStopwatch.Elapsed;
                _excerciseStopwatch.Reset();

                if (duurtijd.Ticks > _maximumExerciseTime.Ticks)
                {
                    _maximumExerciseTime = duurtijd;
                }

                if (duurtijd.Ticks < _minumumExerciseTime.Ticks)
                {
                    _minumumExerciseTime = duurtijd;
                }
            }

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

            _excerciseStopwatch.Start();
        }

        private void UpdateProgress()
        {
            Progress.Value = _aantalGedaan;
        }

        private void ToonPunten()
        {
            var timings = GetTimings();

            Opgave.Text = "";
            Resultaat.Text = "";
            Resultaat.IsEnabled = false;

            var resultaatDialog = new Resultaat(_behaaldePunten, _totaal, _fouten, timings);

            resultaatDialog.ShowDialog();

            Close();
        }

        private (TimeSpan totalTime, TimeSpan averageExerciseTime, TimeSpan minimumExerciseTime, TimeSpan maximumExerciseTime) GetTimings()
        {
            _overallStopwatch.Stop();

            var totalTime = _overallStopwatch.Elapsed;
            var averageTime = new TimeSpan(totalTime.Ticks / _alleOefeningen.Count);

            return (totalTime, averageTime, _minumumExerciseTime, _maximumExerciseTime);
        }

        private void SetFout(string ingevuld)
        {
            _isFout = true;
            _fouten.Add((oefening: (_oefening.opgave, _oefening.resultaat), ingevuld));
            Verbetering.Foreground = new SolidColorBrush(Colors.Red);
            Verbetering.Text = $"FOUT !  het juiste antwoord is {_oefening.resultaat:0}";
        }
    }
}
