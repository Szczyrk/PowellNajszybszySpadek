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
        private int k = 0;
        private List<List<double>> _thetas = new List<List<double>>();
        private List<List<double>> _deltas = new List<List<double>>();
        private List<double> restrictions_xo = new List<double>();
        private string[] _arguments;
        private double c_min;
        private double c_0;
        private double m1 = 1 / 4;
        private double m2 = 10;
        public Powell(string function_f, string[] restrictions_g, string[] arguments, double cm, double c0)
        {
            Restrictions_g.Clear();
            restrictions_xo.Clear();
            k = 0;


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
            c_0 = c0;

        }
        public double Calculate(double[] x)
        {

            double sum = 0;
            for (int i = 0; i < Restrictions_g.Count; i++)
                sum += _deltas[k][i] * Math.Pow((Restrictions_g[i].calculate(x) + _thetas[k][i]), 2)
                    * H.calculate(Restrictions_g[i].calculate(x) + _thetas[k][i]);

            powell = new Function("Powell", $"{_function_x}+{sum}", _arguments);
            //powell.addFunctions(Restrictions_g.ToArray());
            //powell.addFunctions(theta, delta, H);

            mXparser.consolePrintln($"H error : {H.getErrorMessage()}");
            mXparser.consolePrintln($"powell error : {powell.getErrorMessage()}");

            double _x0 = powell.calculate(x);
            double c = 0, c_max = 0;

            for (int i = 0; i < _restrictions_g.Length; i++)
            {
                if (Restrictions_g[i].calculate(_x0) + _thetas[k][i] < 0)
                {
                    restrictions_xo.Add(Math.Abs(Restrictions_g[i].calculate(_x0)));
                    if (restrictions_xo.Max() > c_max)
                    {
                        c_max = restrictions_xo.Max();
                        c = i;
                    }
                }
            }

            mXparser.consolePrintln($"C {c} : {c_max}");
            if (c_max < c_min)
                return _x0;
            List<int> I = new List<int>();
            if (c_max > c_0)
            {
                c_max = c_0;

                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    if ((Math.Abs(Restrictions_g[i].calculate(_x0)) > m1 * c_0) && (Restrictions_g[i].calculate(_x0) + _thetas[k][i] < 0))
                    {
                        _deltas[k][i] = m2 * _deltas[k][i];
                        _thetas[k][i] = _thetas[k][i] / m2;
                    }
                }
                k++;
                return Calculate(x);
            }
            return _x0;
        }
    }
}
