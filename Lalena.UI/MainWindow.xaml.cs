using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LalenasFirstProject;

namespace Lalena.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OneByOne();

            var oefeningenLijst = new OefeningenLijst(GetBewerkingen(), GetMaaltafels().ToList());
            dialog.ShowOneByOne(oefeningenLijst.GetAllOefeningen());
        }

        private IEnumerable<Bewerking> GetBewerkingen()
        {
            if (OefeningenMaal.IsChecked == true) yield return Bewerking.Maal;
            if (OefeningenGedeelddoor.IsChecked == true) yield return Bewerking.GedeeldDoor;
        }

        private IEnumerable<int> GetMaaltafels()
        {
            if (MaaltafelVan1.IsChecked == true) yield return 1;
            if (MaaltafelVan2.IsChecked == true) yield return 2;
            if (MaaltafelVan3.IsChecked == true) yield return 3;
            if (MaaltafelVan4.IsChecked == true) yield return 4;
            if (MaaltafelVan5.IsChecked == true) yield return 5;
            if (MaaltafelVan6.IsChecked == true) yield return 6;
            if (MaaltafelVan7.IsChecked == true) yield return 7;
            if (MaaltafelVan8.IsChecked == true) yield return 8;
            if (MaaltafelVan9.IsChecked == true) yield return 9;
        }
    }
}
