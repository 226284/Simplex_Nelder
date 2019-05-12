using org.mariuszgromada.math.mxparser;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    public class LayerViewModel: INotifyPropertyChanged
    {
        public LayerViewModel()
        {
            this.MyModel = new Plot { Title = "Plot1" };

            // Color axis (the X and Y axes are generated automatically)
            this.MyModel.Axes.Add(new OxyPlot.Wpf.LinearColorAxis());
            /*
            // generate 1d normal distribution
            var singleData = new double[100];
            for (int x = 0; x < 100; ++x)
            {
                singleData[x] = Math.Exp((-1.0 / 2.0) * Math.Pow(((double)x - 50.0) / 20.0, 2.0));
            }

            // generate 2d normal distribution
            var data = new double[10, 10];
            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    data[y, x] = fn.calculate(x, y);
                }
            }*/

            /*var heatMapSeries = new HeatMapSeries
            {
                X0 = -10,
                X1 = 10,
                Y0 = -10,
                Y1 = 10,
                Interpolate = true,
                RenderMethod = HeatMapRenderMethod.Bitmap,
                Data = data
            };*/

            //this.MyModel.Series.Add(heatMapSeries);
            //this.MyModel
        }

        public Plot MyModel { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
