using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlayGround
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

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            OnEnterNumber(sender, null);
        }

        private void OnEnterNumber(object sender, KeyEventArgs e)
        {
            var progress = new DrawingGroup();

            try
            {
                var left = int.Parse(LeftPanel.Text);
                var right = int.Parse(RightPanel.Text);
                var total = int.Parse(Total.Text);

                if (left + right > total)
                    throw new Exception();
                if (left < 0 || right < 0 || total < 0)
                    throw new Exception();

                var leftGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(left, 1)));
                var leftSide = new GeometryDrawing(Brushes.DarkGreen, new Pen(Brushes.DarkGreen, 0), leftGeometry);
                progress.Children.Add(leftSide);

                var midGeometry = new RectangleGeometry(new Rect(new Point(left, 0), new Point(total - right, 1)));
                var midSide = new GeometryDrawing(Brushes.LightGray, new Pen(Brushes.LightGray, 0), midGeometry);
                progress.Children.Add(midSide);

                var rightGeometry = new RectangleGeometry(new Rect(new Point(total - right, 0), new Point(total, 1)));
                var rightSide = new GeometryDrawing(Brushes.DarkRed, new Pen(Brushes.DarkRed, 0), rightGeometry);
                progress.Children.Add(rightSide);
            }
            catch (Exception)
            {
                //ignore exception
            }
            finally
            {
                Progress.Source = new DrawingImage(progress);
            }
        }
    }
}
