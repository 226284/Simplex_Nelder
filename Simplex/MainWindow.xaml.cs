using org.mariuszgromada.math.mxparser;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace Simplex
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Function gui_fun;
        private List<Tuple<double, double>> gui_limits;
        private Algorithm alg;
        private int tmp_vars;
        private bool calc_active_flag = false;
        private int debug_index = -1;

        public PlotViewModel PlotViewModel
        {
            get { return (PlotViewModel)DataContext; }
        }
        public PlotModel ScatterModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            InitializeGUI();

            Algorithm.CalculatedSucc += OnCalculatedSucc;
        }

        private void OnCalculatedSucc(double[] obj)
        {
            string string_obj = "";
            var variables = new string[] { "x1", "x2", "x3", "x4", "x5" };

            foreach (var d in obj)
            {
                string_obj += variables[Array.IndexOf(obj, d)] + ": [" + d.ToString() + "]" + "\n";
            }

            MessageBoxResult result = MessageBox.Show(this, "Obliczono pomyślnie:\n" + string_obj, "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            if (alg != null)
            {
                foreach (var s in alg.calculations)
                {
                    wnd_debug.Text = wnd_debug.Text + s + "\n";
                }
            }
            debug_index = -1;
        }

        private void InitializeGUI()
        {
            ScatterModel = new PlotModel();

            HideAllConditions();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //var lyr = new Layer();
            //lyr.Show();

            //var s2 = new LineSeries
            //{
            //    StrokeThickness = 1,
            //    MarkerSize = 1,
            //    MarkerStroke = OxyColors.ForestGreen,
            //    MarkerType = MarkerType.Plus
            //};

            //for (int i = 0; i < 100; i++)
            //{
            //    s2.Points.Add(new DataPoint(i, i));
            //}
            //ScatterModel.Series.Add(s2);
            //ScatterModel.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 0.1, gui_fun.getDescription()));
            //PlotViewModel.ScatterModel = ScatterModel;
            ScatterModel = new PlotModel();

            ScatterModel.Axes.Add(new LinearColorAxis
            {
                Palette = OxyPalettes.Rainbow(100),
                IsAxisVisible = false
            });

            double x0 = gui_limits[0].Item1;
            double x1 = gui_limits[0].Item2;
            double y0 = gui_limits[1].Item1;
            double y1 = gui_limits[1].Item2;


            Func<double, double, double> peaks = (x, y) => gui_fun.calculate(x, y);
            var xx = ArrayBuilder.CreateVector(x0, x1, 100);
            var yy = ArrayBuilder.CreateVector(y0, y1, 100);
            var peaksData = ArrayBuilder.Evaluate(peaks, xx, yy);

            var heatMapSeries = new HeatMapSeries
            {
                Data = peaksData,
                X0 = x0,
                X1 = x1,
                Y0 = y0,
                Y1 = y1
            };

            var cs = new ContourSeries
            {
                Color = OxyColors.Black,
                //LabelBackground = OxyColors.White,
                ColumnCoordinates = yy,
                RowCoordinates = xx,
                Data = peaksData
            };
            ScatterModel.Series.Add(heatMapSeries);
            ScatterModel.Series.Add(cs);
            PlotViewModel.ScatterModel = null;
            PlotViewModel.ScatterModel = ScatterModel;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateFunction();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return;
            }

            gui_limits = new List<Tuple<double, double>>();

            try
            {
                if (tmp_vars == 2)
                {
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond1.Text), Double.Parse(wnd_maxcond1.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond2.Text), Double.Parse(wnd_maxcond2.Text)));

                    this.wnd_btn_showlayer.IsEnabled = true;
                }

                if (tmp_vars == 3)
                {
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond1.Text), Double.Parse(wnd_maxcond1.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond2.Text), Double.Parse(wnd_maxcond2.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond3.Text), Double.Parse(wnd_maxcond3.Text)));
                }

                if (tmp_vars == 4)
                {
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond1.Text), Double.Parse(wnd_maxcond1.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond2.Text), Double.Parse(wnd_maxcond2.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond3.Text), Double.Parse(wnd_maxcond3.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond4.Text), Double.Parse(wnd_maxcond4.Text)));
                }

                if (tmp_vars == 5)
                {
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond1.Text), Double.Parse(wnd_maxcond1.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond2.Text), Double.Parse(wnd_maxcond2.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond3.Text), Double.Parse(wnd_maxcond3.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond4.Text), Double.Parse(wnd_maxcond4.Text)));
                    gui_limits.Add(new Tuple<double, double>(Double.Parse(wnd_mincond5.Text), Double.Parse(wnd_maxcond5.Text)));
                }

            }
            catch
            {
                MessageBoxResult result = MessageBox.Show(this, "Nieprawidłowe warunki brzegowe!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.wnd_btn_showlayer.IsEnabled = false;
                return;
            }

            DisableAllConditions();

            // check parameters
            try
            {
                Algorithm.a = Double.Parse(this.wnd_a.Text);
                Algorithm.b = Double.Parse(this.wnd_b.Text);
                Algorithm.c = Double.Parse(this.wnd_c.Text);
                Algorithm.epsilon = Double.Parse(this.wnd_epsilon.Text);
                Algorithm.c = Double.Parse(this.wnd_c.Text);
                Algorithm.max_licznik = Int32.Parse(this.wnd_iter.Text);

                this.wnd_a.IsEnabled = false;
                this.wnd_b.IsEnabled = false;
                this.wnd_c.IsEnabled = false;
                this.wnd_epsilon.IsEnabled = false;
                this.wnd_iter.IsEnabled = false;
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show(this, "Nieprawidłowe parametry!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                EnableAllConditions();
                this.wnd_btn_showlayer.IsEnabled = false;
                return;
            }

            if (gui_fun != null && gui_limits != null)
            {
                alg = new Algorithm(gui_fun, gui_limits);
                alg.Initialize();
            }

            wnd_oblicz.Visibility = Visibility.Hidden;
            wnd_restart.Visibility = Visibility.Visible;
            calc_active_flag = true;
        }

        private void Wnd_fun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValidateFunction();
            }
        }

        private void ValidateFunction()
        {
            HideAllConditions();

            var str_fun = this.wnd_fun.Text;
            gui_fun = new Function(str_fun);

            if (gui_fun.checkSyntax() == false)
            {
                MessageBoxResult result = MessageBox.Show(this, "Nieprawidłowa składnia funkcji:\n" + gui_fun.getErrorMessage(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                throw new Exception("Incorrect function syntax");
            }

            tmp_vars = gui_fun.getArgumentsNumber();

            if (tmp_vars == 2)
            {
                wnd_maxcond1.Visibility = Visibility.Visible;
                wnd_mincond1.Visibility = Visibility.Visible;
                wnd_varcond1.Visibility = Visibility.Visible;
                wnd_maxcond2.Visibility = Visibility.Visible;
                wnd_mincond2.Visibility = Visibility.Visible;
                wnd_varcond2.Visibility = Visibility.Visible;
            }
            else if (tmp_vars == 3)
            {
                wnd_maxcond1.Visibility = Visibility.Visible;
                wnd_mincond1.Visibility = Visibility.Visible;
                wnd_varcond1.Visibility = Visibility.Visible;
                wnd_maxcond2.Visibility = Visibility.Visible;
                wnd_mincond2.Visibility = Visibility.Visible;
                wnd_varcond2.Visibility = Visibility.Visible;
                wnd_maxcond3.Visibility = Visibility.Visible;
                wnd_mincond3.Visibility = Visibility.Visible;
                wnd_varcond3.Visibility = Visibility.Visible;
            }
            else if (tmp_vars == 4)
            {
                wnd_maxcond1.Visibility = Visibility.Visible;
                wnd_mincond1.Visibility = Visibility.Visible;
                wnd_varcond1.Visibility = Visibility.Visible;
                wnd_maxcond2.Visibility = Visibility.Visible;
                wnd_mincond2.Visibility = Visibility.Visible;
                wnd_varcond2.Visibility = Visibility.Visible;
                wnd_maxcond3.Visibility = Visibility.Visible;
                wnd_mincond3.Visibility = Visibility.Visible;
                wnd_varcond3.Visibility = Visibility.Visible;
                wnd_maxcond4.Visibility = Visibility.Visible;
                wnd_mincond4.Visibility = Visibility.Visible;
                wnd_varcond4.Visibility = Visibility.Visible;
            }
            else if (tmp_vars == 5)
            {
                wnd_maxcond1.Visibility = Visibility.Visible;
                wnd_mincond1.Visibility = Visibility.Visible;
                wnd_varcond1.Visibility = Visibility.Visible;
                wnd_maxcond2.Visibility = Visibility.Visible;
                wnd_mincond2.Visibility = Visibility.Visible;
                wnd_varcond2.Visibility = Visibility.Visible;
                wnd_maxcond3.Visibility = Visibility.Visible;
                wnd_mincond3.Visibility = Visibility.Visible;
                wnd_varcond3.Visibility = Visibility.Visible;
                wnd_maxcond4.Visibility = Visibility.Visible;
                wnd_mincond4.Visibility = Visibility.Visible;
                wnd_varcond4.Visibility = Visibility.Visible;
                wnd_maxcond5.Visibility = Visibility.Visible;
                wnd_mincond5.Visibility = Visibility.Visible;
                wnd_varcond5.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(this, "Nieprawidłowa ilość zmiennych", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                throw new Exception("Incorrect variables number");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateFunction();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return;
            }

            if (calc_active_flag == true)
            {
                Wnd_restart_Click(new object(), new RoutedEventArgs());
            }
        }

        private void Wnd_restart_Click(object sender, RoutedEventArgs e)
        {
            wnd_oblicz.Visibility = Visibility.Visible;
            wnd_restart.Visibility = Visibility.Hidden;

            //HideAllConditions();
            EnableAllConditions();

            this.wnd_a.IsEnabled = true;
            this.wnd_b.IsEnabled = true;
            this.wnd_c.IsEnabled = true;
            this.wnd_epsilon.IsEnabled = true;
            this.wnd_iter.IsEnabled = true;
            this.wnd_btn_showlayer.IsEnabled = false;
            this.wnd_debug.Text = "";
            debug_index = -1;

            PlotViewModel.ScatterModel = null;
        }

        private void HideAllConditions()
        {
            wnd_maxcond1.Visibility = Visibility.Hidden;
            wnd_mincond1.Visibility = Visibility.Hidden;
            wnd_varcond1.Visibility = Visibility.Hidden;
            wnd_maxcond2.Visibility = Visibility.Hidden;
            wnd_mincond2.Visibility = Visibility.Hidden;
            wnd_varcond2.Visibility = Visibility.Hidden;
            wnd_maxcond3.Visibility = Visibility.Hidden;
            wnd_mincond3.Visibility = Visibility.Hidden;
            wnd_varcond3.Visibility = Visibility.Hidden;
            wnd_maxcond4.Visibility = Visibility.Hidden;
            wnd_mincond4.Visibility = Visibility.Hidden;
            wnd_varcond4.Visibility = Visibility.Hidden;
            wnd_maxcond5.Visibility = Visibility.Hidden;
            wnd_mincond5.Visibility = Visibility.Hidden;
            wnd_varcond5.Visibility = Visibility.Hidden;
        }

        private void DisableAllConditions()
        {
            wnd_maxcond1.IsEnabled = false;
            wnd_mincond1.IsEnabled = false;
            wnd_varcond1.IsEnabled = false;
            wnd_maxcond2.IsEnabled = false;
            wnd_mincond2.IsEnabled = false;
            wnd_varcond2.IsEnabled = false;
            wnd_maxcond3.IsEnabled = false;
            wnd_mincond3.IsEnabled = false;
            wnd_varcond3.IsEnabled = false;
            wnd_maxcond4.IsEnabled = false;
            wnd_mincond4.IsEnabled = false;
            wnd_varcond4.IsEnabled = false;
            wnd_maxcond5.IsEnabled = false;
            wnd_mincond5.IsEnabled = false;
            wnd_varcond5.IsEnabled = false;
        }

        private void EnableAllConditions()
        {
            wnd_maxcond1.IsEnabled = true;
            wnd_mincond1.IsEnabled = true;
            wnd_varcond1.IsEnabled = true;
            wnd_maxcond2.IsEnabled = true;
            wnd_mincond2.IsEnabled = true;
            wnd_varcond2.IsEnabled = true;
            wnd_maxcond3.IsEnabled = true;
            wnd_mincond3.IsEnabled = true;
            wnd_varcond3.IsEnabled = true;
            wnd_maxcond4.IsEnabled = true;
            wnd_mincond4.IsEnabled = true;
            wnd_varcond4.IsEnabled = true;
            wnd_maxcond5.IsEnabled = true;
            wnd_mincond5.IsEnabled = true;
            wnd_varcond5.IsEnabled = true;
        }

        private void NextButtonClicked(object sender, RoutedEventArgs e)
        {
            if (alg != null)
            {
                if (debug_index == -1)
                {
                    wnd_debug.Text = alg.calculations.First();
                    debug_index = 0;
                }
                else if (debug_index < alg.calculations.Count - 1)
                {
                    wnd_debug.Text = alg.calculations[++debug_index];
                }
            }
        }

        private void RevertButtonClicked(object sender, RoutedEventArgs e)
        {
            if (alg != null)
            {
                wnd_debug.Text = "";
                foreach (var s in alg.calculations)
                {
                    wnd_debug.Text = wnd_debug.Text + s + "\n";
                }
                debug_index = -1;
            }
        }

        private void PreviousButtonClicked(object sender, RoutedEventArgs e)
        {
            if (alg != null)
            {
                if (debug_index == -1)
                {
                    wnd_debug.Text = alg.calculations.Last();
                    debug_index = alg.calculations.Count - 1;
                }
                else if (debug_index > 0)
                {
                    wnd_debug.Text = alg.calculations[--debug_index];
                }
            }
        }
    }
}
