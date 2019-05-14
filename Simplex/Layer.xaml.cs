using OxyPlot;
using OxyPlot.Series;
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
using System.Windows.Shapes;

namespace Simplex
{
    /// <summary>
    /// Logika interakcji dla klasy Layer.xaml
    /// </summary>
    public partial class Layer : Window
    {
        private PlotViewModel _PlotViewModel;
       
        public Layer()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //PlotViewModel.Plot = new OxyPlot.PlotModel() { Title = "Testowy" };
            //PlotViewModel.Plot.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

            //this.layer_wnd_plot.InvalidatePlot(true);
        }
    }
}
