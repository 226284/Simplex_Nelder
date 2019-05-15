using org.mariuszgromada.math.mxparser;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using LineSeries = OxyPlot.Series.LineSeries;

namespace Simplex
{
    public class PlotViewModel : ViewModelBase
    {
        private PlotModel _scatterModel;
        public PlotModel ScatterModel
        {
            get { return _scatterModel; }
            set
            {
                if (value != _scatterModel)
                {
                    _scatterModel = value;
                    OnPropertyChanged();
                }
            }
        }
    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] String propName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

