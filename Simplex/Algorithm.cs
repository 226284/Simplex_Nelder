using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace Simplex
{
    public class Algorithm: IAlgorithm
    {
        private Function function;
        private int vars_number;
        private int tips_number;
        private List<Tuple<double, double>> limits;
        private List<string> calculations;
        private List<List<double>> Simplex;

        public Algorithm(Function fn, List<Tuple<double,double>> lm)
        {
            function = fn;
            limits = lm;
            vars_number = function.getArgumentsNumber();
            tips_number = vars_number + 1;

            Simplex = new List<List<double>>();
            RandPoints();
        }

        public void RandPoints()
        {
            Random random = new Random();
            for (int t = 0; t < tips_number; t++)
            {
                var tmp = new List<double>();
                foreach (var e in limits)
                {
                    tmp.Add(e.Item1 + (e.Item2 - e.Item1) * random.NextDouble());
                }
                Simplex.Add(tmp);
            }
        }
    }
}
