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
        private List<Tuple<float, float>> limits;

        public Algorithm(Function fn, List<Tuple<float,float>> lm)
        {
            function = fn;
            limits = lm;
        }
    }
}
