using org.mariuszgromada.math.mxparser;
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
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace Simplex
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Function gui_fun;
        private List<Tuple<float, float>> gui_limits;
        private IAlgorithm alg;

        public MainWindow()
        {
            InitializeComponent();

            InitializeGUI();
        }

        private void InitializeGUI()
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
            if (ValidateFunction() == false) return;

            if (gui_fun != null && gui_limits != null)
            {
                alg = new Algorithm(gui_fun, gui_limits);
            }
        }

        private void Wnd_fun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValidateFunction();
            }
        }

        private bool ValidateFunction()
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

            var str_fun = this.wnd_fun.Text;
            gui_fun = new Function(str_fun);
            this.wnd_debug.Text = this.wnd_debug.Text + gui_fun.getArgumentsNumber() + "\n";

            if(gui_fun.checkSyntax() == false)
            {
                MessageBoxResult result = MessageBox.Show(this, "Nieprawidłowa składnia funkcji:\n" + gui_fun.getErrorMessage(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var vars = gui_fun.getArgumentsNumber();

            if (vars == 2)
            {
                wnd_maxcond1.Visibility = Visibility.Visible;
                wnd_mincond1.Visibility = Visibility.Visible;
                wnd_varcond1.Visibility = Visibility.Visible;
                wnd_maxcond2.Visibility = Visibility.Visible;
                wnd_mincond2.Visibility = Visibility.Visible;
                wnd_varcond2.Visibility = Visibility.Visible;
            }
            else if (vars == 3)
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
            else if (vars == 4)
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
            else if (vars == 5)
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
                return false;
            }
            return true;
        }
    }
}
