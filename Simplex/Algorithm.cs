using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace Simplex
{
    public class Algorithm : IAlgorithm
    {
        static public double a { get; set; } //- współczynnik odbicia  a>0
        static public double b { get; set; } //- współczynnik kontrakcji 0<b<1
        static public double c { get; set; } //- współczynnik ekspancji c>
        static public double epsilon { get; set; }

        private Function function;
        private int vars_number;
        private int tips_number;
        private List<Tuple<double, double>> limits;
        private List<string> calculations;
        private List<double[]> simplex;
        private List<double> simplex_val;
        private double[] Pprim;
        private int h, L, ZW;

        public Action CalculatedSucc;

        public Algorithm(Function fn, List<Tuple<double, double>> lm)
        {
            function = fn;
            limits = lm;
            vars_number = function.getArgumentsNumber();
            tips_number = vars_number + 1;

            RandPoints();

            RunSimplexRun();
        }

        public void RunSimplexRun()
        {
            simplex_val = CalculateFunction();

            ZW = 0;

            h = simplex_val.IndexOf(simplex_val.Max());
            L = simplex_val.IndexOf(simplex_val.Min());

            Pprim = CalculateCenter();

            var Pstar = Reflection(simplex[h], Pprim, a);
            var Fs = function.calculate(Pprim);
            var Fo = function.calculate(Pstar);

            if (Fo < simplex_val[L])
            {
                var Pprimprim = Expansion(Pstar, Pprim, c);
                var Fe = function.calculate(Pprimprim);

                if (Fe < simplex_val[h])
                {
                    simplex[h] = Pprimprim;
                }
                else
                {
                    simplex[h] = Pprim;
                }

                if (isMinCondReached())
                {
                    CalculatedSucc();
                }
                else
                {
                    RunSimplexRun();
                }
            }
            else
            {
                bool break_f = false;
                for (int i = 0; i < tips_number; i++)
                {
                    if (Fo > simplex_val[i]) // za wyjątkiem *f(Ph)
                    {
                        break_f = true;
                    }
                }

                if (break_f == true)
                {
                    if (Fo < simplex_val[h])
                    {
                        simplex[h] = Pprim;
                    }
                    else
                    {
                        var Pppp = Contraction(simplex[h], Pprim, b);
                        var Fk = function.calculate(Pppp);

                        if (Fk < simplex_val[h])
                        {
                            simplex[h] = Pppp;

                            // sprawdź warunek na minimum
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

                            // sprawdź kryterium na minimum
                            // jeśli spełniony to koniec, jeśli nie to:
                            // RunSimplexRun();
                        }
                    }
                }
                else
                {
                    simplex[h] = Pprim;

                    // sprawdź kryterium na minimum
                    // jeśli spełniony to koniec, jeśli nie to:
                    // RunSimplexRun();
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
                for (int i = 0; i < vars_number; i++)
                {
                    sum[i] += p[i];
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
                tmp[i] = (1 + s) * p1[i] - s * p2[i];
            }

            return tmp;
        }

        private double[] Expansion(double[] p1, double[] p2, double s)
        {
            var tmp = new double[vars_number];
            for (int i = 0; i < vars_number; i++)
            {
                tmp[i] = (1 - s) * p1[i] - s * p2[i];
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
            foreach (var p in simplex)
            {
                var sum = 0.0;
                for (int i = 0; i < vars_number; i++)
                {
                    sum = sum + Math.Pow(Pprim[i] - p[i], 2);
                }
                var len = Math.Sqrt(sum);
                if (len > max)
                {
                    max = len;
                }
            }

            // now check if max length is smaller then the defined error
            if (max < epsilon)
            {
                return true;
            }

            return false;
        }
    }
}
