using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Powell
    {
        private string _function_x;
        private string[] _restrictions_g;
        public List<Function> Restrictions_g = new List<Function>();
        private Function powell;
        private Function H = new Function("H", "if(g<0, 1, 0)", "g");
        private int max_k = 10;
        public int k = 0;
        private List<List<double>> _thetas = new List<List<double>>();
        private List<List<double>> _deltas = new List<List<double>>();
        private List<double> restrictions_xo = new List<double>();
        private string[] _arguments;
        private double c_min;
        private double c;
        double c_0;
        private bool[] chceckDoneSixStep;
        private double m1 = 1 / 4;
        private double m2 = 10;
        public Powell(string function_f, string[] restrictions_g, string[] arguments, double cm)
        {
            Restrictions_g.Clear();
            restrictions_xo.Clear();
            k = 0;
            chceckDoneSixStep = new bool[max_k];
            for (int i = 0; i < max_k; i++)
                chceckDoneSixStep[i] = false;

                            _function_x = function_f;
            _restrictions_g = restrictions_g;
            for (int i = 0; i < _restrictions_g.Length; i++)
            {
                Restrictions_g.Add(new Function("Restriction" + i, _restrictions_g[i], _arguments));
                _thetas[k].Add(0);
                _deltas[k].Add(1);
                mXparser.consolePrintln($"Restriction error : {Restrictions_g[i].getErrorMessage()}");
            }
            _arguments = arguments;
            c_min = cm;


        }
        public double[] Calculate(double[] x, double _c)
        {
            c = _c;
            //Krok 2
            x = MinimalizationFunction(x);
            c_0 = c;

            mXparser.consolePrintln($"H error : {H.getErrorMessage()}");
            mXparser.consolePrintln($"powell error : {powell.getErrorMessage()}");

            //Krok 3
            for (int i = 0; i < _restrictions_g.Length; i++)
            {
                if (Restrictions_g[i].calculate(x) + _thetas[k][i] < 0)
                {
                    restrictions_xo.Add(Math.Abs(Restrictions_g[i].calculate(x)));
                    if (restrictions_xo.Max() > c)
                    {
                        c = restrictions_xo.Max();
                    }
                }
            }
            //Krok 4
            mXparser.consolePrintln($"C {c}");
            if (c < c_min)
                return x;
            //Krok 5
            List<int> I = new List<int>();
            if (c > c_0)
            {
                c = c_0;
                //Krok 6
                x = StepSix(x);
            }
            //Krok 8
            x = StepEight(x, false);
            return x;
        }

        double[] MinimalizationFunction(double[] x)
        {
            double[] new_x = new double[x.Length];
            for (int j = 0; j < x.Length; j++)
            {
                x[j] += 1;
                double sum = 0;
                for (int i = 0; i < Restrictions_g.Count; i++)
                    sum += _deltas[k][i] * Math.Pow((Restrictions_g[i].calculate(x) + _thetas[k][i]), 2)
                        * H.calculate(Restrictions_g[i].calculate(x) + _thetas[k][i]);

                powell = new Function("Powell", $"{_function_x}+{sum}", _arguments);
                new_x[j] = powell.calculate(x);
                x[j] -= 1;
            }
            return new_x;
        }
        double[] StepEight(double[] x, bool executeI)
        {
            if (k < max_k)
                if (k == 0 || chceckDoneSixStep[k-1] || executeI)
                {
                    for (int i = 0; i < _restrictions_g.Length; i++)
                    {
                        _thetas[k][i] = Math.Min(Restrictions_g[i].calculate(x) + _thetas[k][i], 0);
                    }
                    //krok 7
                    x = StepSeven(x);
                    StepEight(x, false);
                }
                else
                {
                    if (c <= m1 * c_0)
                        x = StepEight(x, true);
                    else
                        x = StepSix(x);
                }

            return x;
        }

        double[] StepSix(double[] x)
        {
            chceckDoneSixStep[k] = true;
            for (int i = 0; i < _restrictions_g.Length; i++)
            {
                if ((Math.Abs(Restrictions_g[i].calculate(x)) > m1 * c_0) && (Restrictions_g[i].calculate(x) + _thetas[k][i] < 0))
                {
                    _deltas[k][i] = m2 * _deltas[k][i];
                    _thetas[k][i] = _thetas[k][i] / m2;
                }
            }
            //Krok 7
            return StepSeven(x);
        }

        double[] StepSeven(double[] x)
        {
            k++;
            c_0 = c;
            x = MinimalizationFunction(x);
            x = StepEight(x, false);
            return x;
        }
    }
}
