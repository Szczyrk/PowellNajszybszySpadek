﻿using org.mariuszgromada.math.mxparser;
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
        private Function function;
        private Function H = new Function("H", "if(g>0, 1, 0)", "g");
        public int max_k = 10;
        public int k = 0;
        private List<List<double>> _thetas = new List<List<double>>();
        private List<List<double>> _deltas = new List<List<double>>();
        public List<double[]> _xPath = new List<double[]>();
        private List<double> restrictions_xo = new List<double>();
        public List<double> funOptimumStep = new List<double>();
        private string[] _arguments;
        private double[] x;
        private double c_min;
        public double c;
        public double E = 0.001;
        double c_0;
        private bool[] chceckDoneSixStep;
        private double m1 = 0.25;
        private double m2 = 2;
        public double alfa;
        Function gradient;

        public enum Break { c_cmin, fk1_fk2, xk1_xk2, max_absG, maxk }

        public Break breakF;

        public Powell(string function_f, string[] restrictions_g, string[] arguments, double cm, int maxk, double _c)
        {
            Restrictions_g.Clear();
            restrictions_xo.Clear();
            k = 0;

            _function_x = function_f;
            _restrictions_g = restrictions_g;
            _arguments = arguments;
            _thetas.Add(new List<double>());
            _deltas.Add(new List<double>());
            for (int i = 0; i < _restrictions_g.Length; i++)
            {
                Restrictions_g.Add(new Function("Restriction" + i, _restrictions_g[i], _arguments));
                _thetas[k].Add(0.1);
                _deltas[k].Add(1);
                mXparser.consolePrintln($"Restriction error : {Restrictions_g[i].getErrorMessage()}");
                Form1.DebugSendMessage($"Restriction error : {Restrictions_g[i].getErrorMessage()}");
            }
            max_k = maxk;
            chceckDoneSixStep = new bool[max_k + 1];
            for (int i = 0; i < max_k; i++)
                chceckDoneSixStep[i] = false;

            c_min = cm;
            c = _c;

            function = new Function("f", function_f, arguments);
        }

        public double[] Calculate(double[] _x)
        {
            x = _x;

            Changealfa();
/*            int maxk = max_k;
            max_k = maxk * 1 / 10;
            Form1.DebugSendMessage($"");
            Form1.DebugSendMessage($"Pierwsza alfa {alfa}");
            Form1.DebugSendMessage($"");
             Calculate_2(x);*/
            Form1.DebugSendMessage($"");
            Form1.DebugSendMessage($"alfa {alfa}");
            Form1.DebugSendMessage($"");
/*            Changealfa();
            max_k = maxk;
            k = 0;*/
            x = Calculate_2(x);
            if (k >= max_k)
                breakF = Break.maxk;
            return x;
        }
        void Changealfa()
        {
            List<double> alfaFunction = new List<double>();
            double[] gradient = Gradient(k);
            double alfa2 = 0.1, tmp2 = 10000000;
            for (double i = 0.01; i < 0.5; i += 0.01)
            {
                alfa = i;
                alfaFunction.Add(MinimalizationFunction(Subtraction(x, MultiAlfa(gradient)), k));
                double tmp = alfaFunction.Min();
                if (tmp < tmp2)
                {
                    tmp2 = tmp;
                    alfa2 = i;
                }
            }
            alfa = alfa2;
        }
        public double[] Calculate_2(double[] _x)
        {
            x = _x;
            k = 0;
            for (int i = 0; i < _thetas.Count; i++)
            {
                _thetas[i].Clear();
                _deltas[i].Clear();
            }
            _xPath.Clear();
            _thetas.Clear();
            _deltas.Clear();
            _thetas.Add(new List<double>());
            _deltas.Add(new List<double>());
            for (int i = 0; i < _restrictions_g.Length; i++)
            {
                _thetas[k].Add(0.1);
                _deltas[k].Add(1);
            }
            _xPath.Add(x);
            funOptimumStep.Add(function.calculate(x));
            Form1.DebugSendMessage($"f : {funOptimumStep[k]}");
            for (int i = 0; i < x.Length; i++)
                Form1.DebugSendMessage($"{_arguments[i]} : {x[i]}");
            Form1.DebugSendMessage($"");
            Form1.DebugSendMessage($"Next f() = {_function_x} {String.Join(" , ", _arguments)} {String.Join(" , ", x)} g(x)= {String.Join(" , ", _restrictions_g)} k= {max_k} c={c} c_min= {c_min}");
            Form1.DebugSendMessage($"");

                k++;
                _thetas.Add(new List<double>());
                _deltas.Add(new List<double>());
                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    _thetas[k].Add(0.1);
                    _deltas[k].Add(1);
                }
            x = Subtraction(x, MultiAlfa(Gradient(k)));
            _xPath.Add(x);
            funOptimumStep.Add(function.calculate(x));
            Form1.DebugSendMessage($"f : {funOptimumStep[k]}");
            for (int i = 0; i < x.Length; i++)
                Form1.DebugSendMessage($"{_arguments[i]} : {x[i]}");
            while (k < max_k)
            {
                Form1.DebugSendMessage($"");
                if (CheckKryteriumStopu())
                    return x;
                k++;
                _thetas.Add(new List<double>());
                _deltas.Add(new List<double>());
                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    _thetas[k].Add(0.1);
                    _deltas[k].Add(1);
                }
                if (Calculate_Powell())
                    return x;

            }
            return x;
        }

        private bool CheckKryteriumStopu()
        {
            double step = MinimalizationFunction(_xPath[k], k);
            double stepPrevious = MinimalizationFunction(_xPath[k - 1], k - 1);
            double score = Math.Abs(step - stepPrevious);

            mXparser.consolePrintln($"F(x)step : {step}");
            Form1.DebugSendMessage($"F(x)step : {step}");
            mXparser.consolePrintln($"F(x)stepPrevious : {stepPrevious}");
            Form1.DebugSendMessage($"F(x)stepPrevious : {stepPrevious}");
            mXparser.consolePrintln($"F(x)score : {score}");
            Form1.DebugSendMessage($"F(x)score : {score}");
            if (score <= E)
            {
                breakF = Break.fk1_fk2;
                return true;
            }

            double[] sub = Subtraction(_xPath[k], _xPath[k - 1]);
            for (int i = 0; i < sub.Length; i++)
            {
                sub[i] = Math.Abs(sub[i]);
            }
            score = sub.Max();

            mXparser.consolePrintln($"x_kstep : {String.Join(" ' ", _xPath[k])}");
            Form1.DebugSendMessage($"x_kstep : {String.Join(" ' ", _xPath[k])}");
            mXparser.consolePrintln($"vstepPrevious : {String.Join(" ' ", _xPath[k - 1])}");
            Form1.DebugSendMessage($"x_kstepPrevious : {String.Join(" ' ", _xPath[k - 1])}");
            mXparser.consolePrintln($"sub : {String.Join(" ' ", sub)}");
            Form1.DebugSendMessage($"sub : {String.Join(" ' ", sub)}");
            mXparser.consolePrintln($"x_kscore : {score}");
            Form1.DebugSendMessage($"x_kscore : {score}");
            if (score <= E)
            {
                breakF = Break.xk1_xk2;
                return true;
            }

            sub = Gradient(k);
            for (int i = 0; i < sub.Length; i++)
            {
                sub[i] = Math.Abs(sub[i]);
            }
            score = sub.Max();

            mXparser.consolePrintln($"G tepPrevious : {String.Join(" ' ", sub)}");
            Form1.DebugSendMessage($"G stepPrevious : {String.Join(" ' ", sub)}");
            mXparser.consolePrintln($"G score : {score}");
            Form1.DebugSendMessage($"G score : {score}");
            if (score <= E)
            {
                breakF = Break.max_absG;
                return true;
            }


            return false;
        }

        public bool Calculate_Powell()
        {
            if (k > max_k)
                return false;


            //Krok 2
            x = Subtraction(x, MultiAlfa(Gradient(k)));
            _xPath.Add(x);
            funOptimumStep.Add(function.calculate(x));
            Form1.DebugSendMessage($"f : {funOptimumStep[k]}");
            for (int i = 0; i < x.Length; i++)
                Form1.DebugSendMessage($"{_arguments[i]} : {x[i]}");


            c_0 = c;


            mXparser.consolePrintln($"H error : {H.getErrorMessage()}");
            Form1.DebugSendMessage($"H error : {H.getErrorMessage()}");
            //mXparser.consolePrintln($"powell error : {powell.getErrorMessage()}");

            restrictions_xo.Clear();
            //Krok 3
            for (int i = 0; i < _restrictions_g.Length; i++)
            {
                if (Restrictions_g[i].calculate(x) + _thetas[k][i] >= 0)
                {
                    restrictions_xo.Add(Math.Abs(Restrictions_g[i].calculate(x)));
                    c = restrictions_xo.Max();
                }
            }

            //Krok 4
            mXparser.consolePrintln($"C {c}");
            Form1.DebugSendMessage($"C {c}");
            if (c < c_min)
            {
                breakF = Break.c_cmin;
                return true;
            }
            //Krok 5
            if (c < c_0)
            {
                //Krok 8
                StepEight();
            }
            else
            {
                c = c_0;
                //Krok 6
                StepSix();
            }
            return false;
        }

        private double[] MultiAlfa(double[] v)
        {
            double[] vs = new double[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                vs[i] = v[i] * alfa;
            }
            return vs;
        }

        private double[] Subtraction(double[] v1, double[] v2)
        {
            double[] vs = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                vs[i] = v1[i] + v2[i];
            }
            return vs;
        }

        double[] Gradient(int k)
        {
            double[] new_gradient = new double[x.Length];
            int j = 0;
            foreach (string arg in _arguments)
            {
                List<Function> Restrictions_g_gradient = new List<Function>();
                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    Restrictions_g_gradient.Add(new Function("Restriction" + i, $"der({_restrictions_g[i]},{arg})", _arguments));
                    mXparser.consolePrintln($"Restriction error : {Restrictions_g_gradient[i].getErrorMessage()}");
                    Form1.DebugSendMessage($"Restriction error : {Restrictions_g_gradient[i].getErrorMessage()}");
                }

                double sum = 0;
                for (int i = 0; i < Restrictions_g_gradient.Count; i++)
                {
                    sum += _deltas[k][i] * Math.Pow((-Restrictions_g_gradient[i].calculate(x) + _thetas[k][i]), 2)
                        * H.calculate(-Restrictions_g_gradient[i].calculate(x) + _thetas[k][i]);
                }
                mXparser.consolePrintln($"Sum ograniczen: {sum}");
                Form1.DebugSendMessage($"Sum ograniczen): {sum}");

                gradient = new Function("Gradient", $"-der({_function_x},{arg})+{sum}", _arguments);

                new_gradient[j++] = gradient.calculate(x);
                mXparser.consolePrintln($"new_gradient {new_gradient[j - 1]}");
            }
            mXparser.consolePrintln($"gradient error : {gradient.getErrorMessage()}");
            Form1.DebugSendMessage($"gradient error : {gradient.getErrorMessage()}");
            return new_gradient;
        }


        void StepEight()
        {

            if (k == 0 || chceckDoneSixStep[k - 1])
            {
                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    _thetas[k][i] = Math.Max(Restrictions_g[i].calculate(x) + _thetas[k][i], 0.1);
                }
                Form1.DebugSendMessage($"{k} deltas step8I =  {String.Join(" , ", _deltas[k])} thetas = {String.Join(" , ", _thetas[k])} ");
                //krok 7

                Form1.DebugSendMessage($"");
                k++;
                _thetas.Add(new List<double>());
                _deltas.Add(new List<double>());
                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    _thetas[k].Add(0.1);
                    _deltas[k].Add(1);
                }

                x = Subtraction(x, MultiAlfa(Gradient(k - 1)));
                _xPath.Add(x);
                funOptimumStep.Add(function.calculate(x));
                Form1.DebugSendMessage($"f : {funOptimumStep[k]}");
                for (int i = 0; i < x.Length; i++)
                    Form1.DebugSendMessage($"{_arguments[i]} : {x[i]}");
                c_0 = c;
            }
            else
            {
                if (c <= m1 * c_0)
                {
                    for (int i = 0; i < _restrictions_g.Length; i++)
                    {
                        _thetas[k][i] = Math.Max(Restrictions_g[i].calculate(x) + _thetas[k][i], 0.1);
                    }
                    Form1.DebugSendMessage($"{k} deltas step8IIA  =  {String.Join(" , ", _deltas[k])} thetas = {String.Join(" , ", _thetas[k])} ");
                    //krok 7
                    Form1.DebugSendMessage($"");
                    k++;
                    _thetas.Add(new List<double>());
                    _deltas.Add(new List<double>());
                    for (int i = 0; i < _restrictions_g.Length; i++)
                    {
                        _thetas[k].Add(0.1);
                        _deltas[k].Add(1);
                    }

                    x = Subtraction(x, MultiAlfa(Gradient(k - 1)));
                    _xPath.Add(x);
                    funOptimumStep.Add(function.calculate(x));
                    Form1.DebugSendMessage($"f : {funOptimumStep[k]}");
                    for (int i = 0; i < x.Length; i++)
                        Form1.DebugSendMessage($"{_arguments[i]} : {x[i]}");
                    c_0 = c;
                }
                else
                {
                    chceckDoneSixStep[k] = true;
                    for (int i = 0; i < Restrictions_g.Count; i++)
                    {
                        if ((Math.Abs(Restrictions_g[i].calculate(x)) > m1 * c_0) && (Restrictions_g[i].calculate(x) + _thetas[k][i] >= 0))
                        {
                            _deltas[k][i] = m2 * _deltas[k][i];
                            _thetas[k][i] = _thetas[k][i] / m2;
                        }
                    }
                    Form1.DebugSendMessage($"{k} deltas step8IIB  =  {String.Join(" , ", _deltas[k])} thetas = {String.Join(" , ", _thetas[k])} ");
                }
            }
        }

        void StepSix()
        {
            chceckDoneSixStep[k] = true;
            for (int i = 0; i < Restrictions_g.Count; i++)
            {
                if ((Math.Abs(Restrictions_g[i].calculate(x)) > m1 * c_0) && (Restrictions_g[i].calculate(x) + _thetas[k][i] >= 0))
                {
                    _deltas[k][i] = m2 * _deltas[k][i];
                    _thetas[k][i] = _thetas[k][i] / m2;
                }
            }
            Form1.DebugSendMessage($"{k} deltas step6  =  {String.Join(" , ", _deltas[k])} thetas = {String.Join(" , ", _thetas[k])} ");
            //Krok 7
           StepSeven();
        }

        void StepSeven()
        {
            Form1.DebugSendMessage($"");
            k++;
            _thetas.Add(new List<double>());
            _deltas.Add(new List<double>());
            for (int i = 0; i < _restrictions_g.Length; i++)
            {
                _thetas[k].Add(0.1);
                _deltas[k].Add(1);
            }

            x = Subtraction(x, MultiAlfa(Gradient(k - 1)));
            _xPath.Add(x);
            funOptimumStep.Add(function.calculate(x));
            Form1.DebugSendMessage($"f : {funOptimumStep[k]}");
            for (int i = 0; i < x.Length; i++)
                Form1.DebugSendMessage($"{_arguments[i]} : {x[i]}");
            c_0 = c;
            StepEight();
        }


        double MinimalizationFunction(double[] x, int k)
        {

            double new_x;

            double sum = 0;
            for (int i = 0; i < Restrictions_g.Count; i++)
            {
                sum += _deltas[k][i] * Math.Pow((Restrictions_g[i].calculate(x) + _thetas[k][i]), 2)
                    * H.calculate(Restrictions_g[i].calculate(x) + _thetas[k][i]);
            }
            powell = new Function("Powell", $"{_function_x}+{sum}", _arguments);
            new_x = powell.calculate(x);


            return new_x;
        }
    }
}
