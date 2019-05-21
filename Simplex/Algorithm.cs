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
        static public double a { get; set; } // - współczynnik odbicia  a>0
        static public double b { get; set; } // - współczynnik kontrakcji 0<b<1
        static public double c { get; set; } // - współczynnik ekspansji c>
        static public double epsilon { get; set; } // - maksymalny błąd 
        static public int max_licznik { get; set; } // - ilość iteracji
        static public string precision { get; set; } = "F4";
        public bool isTwoDimProb { get; set; }

        public List<string> calculations;
        public List<List<double[]>> simplex_points;
        private Function function;
        private int vars_number;
        private int tips_number;
        private List<Tuple<double, double>> limits;
        private List<double[]> simplex;
        private List<double> simplex_val;
        private bool f_break;
        //private double[] Pp;
        private int h, L, licznik;

        static public event Action<double[],double> CalculatedSucc;

        public Algorithm(Function fn, List<Tuple<double, double>> lm)
        {
            function = fn;
            limits = lm;
            vars_number = function.getArgumentsNumber();
            tips_number = vars_number + 1;
            simplex = new List<double[]>();
            calculations = new List<string>();
            simplex_points = new List<List<double[]>>();
        }

        public void Initialize()
        {
            calculations.Clear();
            RandPoints();
            RunSimplexRun();
        }

        public void RunSimplexRun()
        {
            while (true)
            {
                licznik++;

                try
                {
                    simplex_val = CalculateFunction();
                }
                catch
                {
                    Debug.WriteLine("Error while calculating function");
                }

                h = simplex_val.IndexOf(simplex_val.Max());
                L = simplex_val.IndexOf(simplex_val.Min());

                var Pp = CalculateCenter();

                //calculations.Add(UpdateString(licznik, simplex, simplex[h], function.calculate(Pp)));
                calculations.Add(UpdateString(licznik, simplex, simplex_val, simplex[L], simplex_val[L]));

                if (isTwoDimProb)
                {
                    List<double[]> copy = new List<double[]>(simplex);
                    simplex_points.Add(copy);
                }

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

                    if (isMinCondReached() || licznik >= max_licznik)
                    {
                        CalculatedSucc(simplex[L], simplex_val[L]);
                        return;
                    }
                    //else
                    //{
                    //    goto endofloop;
                    //    RunSimplexRun();
                    //}
                }
                else
                {
                    bool break_f = false;
                    for (int i = 0; i < tips_number; i++)
                    {
                        if (Fo > simplex_val[i] && i != h)
                        {
                            break_f = true;
                            break;
                        }
                    }

                    if (break_f == true)
                    {
                        if (Fo < simplex_val[h])
                        {
                            this.simplex[h] = Ps;
                        }

                        var Psss = Contraction(simplex[h], Pp, b);
                        var Fk = function.calculate(Psss);

                        if (Fk < simplex_val[h])
                        {
                            simplex[h] = Psss;

                            if (isMinCondReached())
                            {
                                CalculatedSucc(simplex[L], simplex_val[L]);
                                return;
                            }
                            //else
                            //{
                            //  goto endofloop;
                            // RunSimplexRun();
                            //}
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

                            if (isMinCondReached() || licznik >= max_licznik)
                            {
                                CalculatedSucc(simplex[L], simplex_val[L]);
                                return;
                            }
                            //else
                            //{
                            //break;
                            //RunSimplexRun();
                            //}
                        }

                    }

                    else
                    {
                        simplex[h] = Ps;

                        if (isMinCondReached() || licznik >= max_licznik)
                        {
                            CalculatedSucc(simplex[L], simplex_val[L]);
                            return;
                        }
                        //else
                        //{
                        //break;
                        //RunSimplexRun();
                        //}
                    }
                }
            }
        }

        public void RandPoints()
        {
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
            //ebug.WriteLine(max);
            // now check if max length is smaller then the defined error
            if (max < epsilon)
            {
                return true;
            }

            return false;
        }

        private string UpdateString(int iter, List<double[]> points, List<double> p_val, double[] center, double val)
        {
            string string_obj = "----------ITERACJA: " + iter + "----------\n";
            var variables = new string[] { "x1", "x2", "x3", "x4", "x5" };

            string_obj += "Wierzchołki simpleksu:\n";
            foreach (var p in points)
            {
                string_obj += points.IndexOf(p).ToString() + ") \n";
                foreach (var d in p)
                {
                    string_obj += "  " + variables[Array.IndexOf(p, d)] + ": [" + d.ToString(precision) + "]" + "\n";
                }
                string_obj += "  f(P" + points.IndexOf(p) + ") = " + p_val[points.IndexOf(p)] + "\n";
            }

            //string_obj += "\nŚrodek symetrii w punkcie:\n";
            string_obj += "\nMin wartość dla wierzchołka:\n";

            foreach (var d in center)
            {
                string_obj += "  " + variables[Array.IndexOf(center, d)] + ": [" + d.ToString(precision) + "]" + "\n";
            }

            //string_obj += "\nWartość funkcji w środku:\n  " + val.ToString() + "\n";
            string_obj += "\nWartość funkcji w min:\n  " + val.ToString(precision) + "\n";

            return string_obj;
        }
    }
}
