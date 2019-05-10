﻿using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
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


        public MainWindow()
        {
            InitializeComponent();

            InitializeGUI();
        }

        private void OnCalculatedSucc()
        {
            MessageBoxResult result = MessageBox.Show(this, "Obliczono pomyślnie", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void InitializeGUI()
        {
            HideAllConditions();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Layer wnd = new Layer();
            wnd.Show();
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
                    double min, max;
                    Double.TryParse(wnd_mincond1.Text, out min);
                    Double.TryParse(wnd_maxcond1.Text, out max);
                    gui_limits.Add(new Tuple<double, double>(min, max));

                    Double.TryParse(wnd_mincond2.Text, out min);
                    Double.TryParse(wnd_maxcond2.Text, out max);
                    gui_limits.Add(new Tuple<double, double>(min, max));
                }

                if (tmp_vars == 3)
                {
                    double min, max;
                    Double.TryParse(wnd_mincond1.Text, out min);
                    Double.TryParse(wnd_maxcond1.Text, out max);
                    gui_limits.Add(new Tuple<double, double>(min, max));

                    Double.TryParse(wnd_mincond2.Text, out min);
                    Double.TryParse(wnd_maxcond2.Text, out max);
                    gui_limits.Add(new Tuple<double, double>(min, max));

                    Double.TryParse(wnd_mincond3.Text, out min);
                    Double.TryParse(wnd_maxcond3.Text, out max);
                    gui_limits.Add(new Tuple<double, double>(min, max));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Incorrect limits");
                return;
            }

            DisableAllConditions();

            // check parameters
            try
            {
                /*double tmp;
                Double.TryParse(this.wnd_a.Text, out tmp);
                Algorithm.a = tmp;
                Double.TryParse(this.wnd_b.Text, out tmp);
                Algorithm.b = tmp;
                Double.TryParse(this.wnd_c.Text, out tmp);
                Algorithm.c = tmp;
                Double.TryParse(this.wnd_epsilon.Text, out tmp);
                Algorithm.epsilon = tmp;
                */
                this.wnd_a.IsEnabled = false;
                this.wnd_b.IsEnabled = false;
                this.wnd_c.IsEnabled = false;
                this.wnd_epsilon.IsEnabled = false;
            }
            catch
            {
                throw new Exception("Incorrect Parameters");
            }

            if (gui_fun != null && gui_limits != null)
            {
                alg = new Algorithm(gui_fun, gui_limits);

                alg.CalculatedSucc += OnCalculatedSucc;
            }

            wnd_oblicz.Visibility = Visibility.Hidden;
            wnd_restart.Visibility = Visibility.Visible;
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
        }

        private void Wnd_restart_Click(object sender, RoutedEventArgs e)
        {
            wnd_oblicz.Visibility = Visibility.Visible;
            wnd_restart.Visibility = Visibility.Hidden;

            HideAllConditions();
            EnableAllConditions();

            this.wnd_a.IsEnabled = true;
            this.wnd_b.IsEnabled = true;
            this.wnd_c.IsEnabled = true;
            this.wnd_epsilon.IsEnabled = true;
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
    }
}
