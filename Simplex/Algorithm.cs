using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace Simplex
{

    public class Algorithm : IAlgorithm
    {
        public double a { get; set; } = 1; //- współczynnik odbicia  a>0
        public double b { get; set; } = 0.5; //- współczynnik kontrakcji 0<b<1
        public double c { get; set; } = 0.2; //- współczynnik ekspancji c>
        public double epsilon { get; set; } = 1;

        private Function function;
        private int vars_number;
        private int tips_number;
        private List<Tuple<double, double>> limits;
        private List<string> calculations;
        private List<double[]> simplex;
        private List<double> simplex_val;
        private double[] Pp;
        private int h, L, ZW;

        public List<List<double[]>> points;
        public Action CalculatedSucc;

        public Algorithm(Function fn, List<Tuple<double, double>> lm)
        {
            function = fn;
            limits = lm;
            vars_number = function.getArgumentsNumber();
            tips_number = vars_number + 1;
            points = new List<List<double[]>>();

            RandPoints();
            points.Add(simplex);
            //new LayerViewModel(fn);
            BegginingProcedure();
            RunSimplexRun();
        }

        public void BegginingProcedure()
        {
            simplex_val = CalculateFunction();
            ZW = 0;
        }

        public void RunSimplexRun()
        {            
            h = simplex_val.IndexOf(simplex_val.Max());
            L = simplex_val.IndexOf(simplex_val.Min());

            Pp = CalculateCenter();

            var Ps = Reflection(simplex[h], Pp, a);
            var Fs = function.calculate(Pp);
            var Fo = function.calculate(Ps);

            if (Fo < simplex_val[L])
            {
                var Pss = Expansion(Ps, Pp, c);
                var Fe = function.calculate(Pss);

                if (Fe < simplex_val[h])
                {
                    simplex[h] = Pss;
                }
                else
                {
                    simplex[h] = Ps;
                }

                if (isMinCondReached())
                {
                    CalculatedSucc();
                    return;
                }
                else
                {
                    if (ZW == 1)
                    {
                        BegginingProcedure();
                    }
                    RunSimplexRun();
                }
            }
            else
            {
                bool break_f = false;
                for (int i = 0; i < tips_number; i++)
                {
                    if (Fo > simplex_val[i] && i != h) // za wyjątkiem *f(Ph)
                    {
                        break_f = true;
                        break;
                    }
                }

                if (break_f == true)
                {
                    if (Fo < simplex_val[h])
                    {
                        simplex[h] = Ps;
                    }

                    var Psss = Contraction(simplex[h], Pp, b);
                    var Fk = function.calculate(Psss);

                    if (Fk < simplex_val[h])
                    {
                        simplex[h] = Psss;

                        if (isMinCondReached())
                        {
                            CalculatedSucc();
                        }
                        else
                        {
                            if (ZW == 1)
                            {
                                BegginingProcedure();
                            }
                            RunSimplexRun();
                        }
                    }
                    else
                    {
                        // wykonanie redukcji simpleksu
                        foreach (var t in simplex)
                        {
                            for (int i = 0; i < vars_number; i++)
                            {
                                t[i] = 0.5 * (t[i] + simplex[L][i]);
                            }
                        }

                        ZW = 1;

                        if (isMinCondReached())
                        {
                            CalculatedSucc();
                        }
                        else
                        {
                            if (ZW == 1)
                            {
                                BegginingProcedure();
                            }
                            RunSimplexRun();
                        }
                    }
                    
                }

                else
                {
                    simplex[h] = Ps;

                    if (isMinCondReached())
                    {
                        CalculatedSucc();
                    }
                    else
                    {
                        if (ZW == 1)
                        {
                            BegginingProcedure();
                        }
                        RunSimplexRun();
                    }
                }
            }
        }

        public void RandPoints()
        {
            simplex = new List<double[]>();

            Random random = new Random();
            for (int t = 0; t < tips_number; t++)
            {
                var d_tmp = new double[vars_number];
                var v = 0;
                foreach (var e in limits)
                {
                    d_tmp[v] = e.Item1 + (e.Item2 - e.Item1) * random.NextDouble();
                    v++;
                }
                simplex.Add(d_tmp);
            }
        }

        private List<double> CalculateFunction()
        {
            try
            {
                var tmp = new List<double>();
                foreach (var p in simplex)
                {
                    tmp.Add(function.calculate(p));
                }
                return tmp;
            }
            catch
            {
                throw new Exception("Error while calculating function");
            }
        }

        private double[] CalculateCenter()
        {
            var sum = new double[vars_number];
            foreach (var p in simplex)
            {
                if (simplex.IndexOf(p) != h)
                {
                    for (int i = 0; i < vars_number; i++)
                    {
                        sum[i] += p[i];
                    }
                }
            }
            var tmp = new double[vars_number];
            for (int j = 0; j < vars_number; j++)
            {
                tmp[j] = sum[j] / vars_number;
            }

            return tmp;
        }

        private double[] Reflection(double[] p1, double[] p2, double s)
        {
            var tmp = new double[vars_number];
            for (int i = 0; i < vars_number; i++)
            {
                tmp[i] = (1 + s) * p2[i] - s * p1[i];
            }

            return tmp;
        }

        private double[] Expansion(double[] p1, double[] p2, double s)
        {
            var tmp = new double[vars_number];
            for (int i = 0; i < vars_number; i++)
            {
                tmp[i] = (1 - s) * p1[i] - s * p2[i]; //(1 + s)?
            }

            return tmp;
        }

        private double[] Contraction(double[] p1, double[] p2, double s)
        {
            var tmp = new double[vars_number];
            for (int i = 0; i < vars_number; i++)
            {
                tmp[i] = s * p1[i] + (1 - s) * p2[i];
            }

            return tmp;
        }

        private bool isMinCondReached()
        {
            double max = 0;
            double[] p1, p2;
            var j = 0;

            while (j < tips_number)
            {
                p1 = simplex[j];

                if (j + 1 >= tips_number)
                {
                    p2 = simplex[0];
                }
                else
                {
                    p2 = simplex[j + 1];
                }

                var sum = 0.0;
                for (int i = 0; i < vars_number; i++)
                {
                    sum = sum + Math.Pow(p1[i] - p2[i], 2);
                }
                var len = Math.Sqrt(sum);
                if (len > max)
                {
                    max = len;
                }
                j++;
            }
            Debug.WriteLine(max);
            // now check if max length is smaller then the defined error
            if (max < epsilon)
            {
                return true;
            }

            return false;
        }
    }
}
