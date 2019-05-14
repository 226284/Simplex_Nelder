using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Simplex
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        PlotViewModel PlotViewModel;
        public Window1()
        {
            PlotViewModel = new PlotViewModel();
            PlotViewModel.Plot = new OxyPlot.PlotModel() { Title = "Testowy" };
            PlotViewModel.Plot.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PlotViewModel.Plot.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 0.1, "sin(x)"));
            plot1.InvalidatePlot(true);
        }
    }
}
