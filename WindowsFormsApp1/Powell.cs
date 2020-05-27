using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Function powell;
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
        public double m1 = 0.25;
        public double m2 = 10;
        public double thetaStart = 0;
        public double alfa;
        int Lp = 1;
        int round = 8;
        Function gradient;

        public enum Break { c_cmin, fk1_fk2, xk1_xk2, maxk, skalarG }

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
                _thetas[k].Add(thetaStart);
                _deltas[k].Add(1);
                mXparser.consolePrintln($"Restriction ErrorMessage : {Restrictions_g[i].getErrorMessage()}");
                Form1.DebugSendMessage($"Restriction ErrorMessage : {Restrictions_g[i].getErrorMessage()}");
            }
            max_k = maxk;
            chceckDoneSixStep = new bool[max_k + 1];
            for (int i = 0; i < max_k; i++)
                chceckDoneSixStep[i] = false;

            c_min = cm;
            c = _c;

            function = new Function("f", function_f, arguments);
        }

        void DebugSendMessage(double e1, double e2, double e4)
        {
            Form1.SendMessageC($"{c}");
            if (_arguments[0] == "x1" && _arguments.Length == 2)
            {
                Form1.SendMessageX1($"{x[0]}");
                Form1.SendMessageX2($"{x[1]}");
            }
            else
            {
                Form1.SendMessageX1($"{x[1]}");
                Form1.SendMessageX2($"{x[0]}");
            }
            Form1.SendMessageF($"{funOptimumStep[k]}");
            Form1.SendMessageE1($"{e1}");
            Form1.SendMessageE2($"{e2}");
            Form1.SendMessageE4($"{e4}");

        }

        public double[] Calculate(double[] _x)
        {
            x = _x;
            x = Calculate_2(x);
            if (k >= max_k)
                breakF = Break.maxk;
            return x;
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
                _thetas[k].Add(thetaStart);
                _deltas[k].Add(1);
            }
            _xPath.Add(x);
            funOptimumStep.Add(function.calculate(x));
            //Krok 3
            for (int i = 0; i < _restrictions_g.Length; i++)
            {
                if (Restrictions_g[i].calculate(x) + _thetas[k][i] > 0)
                {
                    restrictions_xo.Add(Math.Abs(Restrictions_g[i].calculate(x)));
                    c = restrictions_xo.Max();
                }
            }

            //Krok 4
            mXparser.consolePrintln($"C {c}");
            Form1.DebugSendMessage($"C {c}");
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
                _thetas[k].Add(_thetas[k - 1][i]);
                _deltas[k].Add(_deltas[k - 1][i]);
            }
            x = Subtraction(x, MultiAlfa(Gradient(k)));
            _xPath.Add(x);
            funOptimumStep.Add(function.calculate(x));
            restrictions_xo.Clear();

            Form1.DebugSendMessage($"f : {funOptimumStep[k]}");
            for (int i = 0; i < x.Length; i++)
                Form1.DebugSendMessage($"{_arguments[i]} : {x[i]}");
            while (k < max_k)
            {
                Form1.DebugSendMessage($"");
                if (CheckKryteriumStopu())
                    return x;
                Calculate_Powell();
            }
            return x;
        }

        void SaveToFileLatex()
        {
            if (Form1.saveToFile)
            {
                string docPath =
                  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "data_output.txt"), true))
                {
                    outputFile.WriteLine($"{Lp++} & {x[0]} & {x[1]} & {funOptimumStep[k]} & {c} \\hline");
                }
            }
        }

        private bool CheckKryteriumStopu()
        {
            double step = funOptimumStep[k];
            double stepPrevious = funOptimumStep[k - 1];
            double e1 = Math.Abs(step - stepPrevious);
            bool e1B = false;

            mXparser.consolePrintln($"F(x)step : {step}");
            Form1.DebugSendMessage($"F(x)step : {step}");
            mXparser.consolePrintln($"F(x)stepPrevious : {stepPrevious}");
            Form1.DebugSendMessage($"F(x)stepPrevious : {stepPrevious}");
            mXparser.consolePrintln($"F(x)score : {e1}");
            Form1.DebugSendMessage($"F(x)score e1 : {e1}");
            if (e1 <= E)
            {
                breakF = Break.fk1_fk2;
                e1B = true;
            }

            double[] sub = Subtraction(_xPath[k], _xPath[k - 1]);
            for (int i = 0; i < sub.Length; i++)
            {
                sub[i] = Math.Abs(sub[i]);
            }
            double e2 = sub.Max();
            bool e2B = false;

            mXparser.consolePrintln($"x_kstep : {String.Join(" ' ", _xPath[k])}");
            Form1.DebugSendMessage($"x_kstep : {String.Join(" ' ", _xPath[k])}");
            mXparser.consolePrintln($"vstepPrevious : {String.Join(" ' ", _xPath[k - 1])}");
            Form1.DebugSendMessage($"x_kstepPrevious : {String.Join(" ' ", _xPath[k - 1])}");
            mXparser.consolePrintln($"sub : {String.Join(" ' ", sub)}");
            Form1.DebugSendMessage($"sub : {String.Join(" ' ", sub)}");
            mXparser.consolePrintln($"x_kscore : {e2}");
            Form1.DebugSendMessage($"x_kscore e2: {e2}");
            if (e2 <= E)
            {
                breakF = Break.xk1_xk2;
                e2B = true;
            }

            sub = Gradient(k);
            double e4 = 0;
            bool e4B = false;
            for (int i = 0; i < sub.Length; i++)
            {
                e4 += Math.Pow(sub[i], 2);
            }

            mXparser.consolePrintln($"G tepPrevious : {String.Join(" ' ", sub)}");
            Form1.DebugSendMessage($"G stepPrevious : {String.Join(" ' ", sub)}");
            mXparser.consolePrintln($"G score : {e4}");
            Form1.DebugSendMessage($"G score e4: {e4}");
            if (e4 <= E)
            {
                breakF = Break.skalarG;
                e4B = true;
            }

            DebugSendMessage(e1, e2, e4);
            if (e1B || e2B || e4B)
                return true;
            return false;
        }

        public void Calculate_Powell()
        {
            while (k < max_k)
            {
                Form1.waitHandle.WaitOne();

                Form1.DebugSendMessage($"");
                Form1.DebugSendMessage($"alfa {alfa}");
                Form1.DebugSendMessage($"");
                //Krok 2
                k++;
                _thetas.Add(new List<double>());
                _deltas.Add(new List<double>());
                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    _thetas[k].Add(_thetas[k - 1][i]);
                    _deltas[k].Add(_deltas[k - 1][i]);
                }
                x = Subtraction(x, MultiAlfa(Gradient(k)));
                _xPath.Add(x);
                funOptimumStep.Add(function.calculate(x));
                Form1.DebugSendMessage($"f : {funOptimumStep[k]}");
                for (int i = 0; i < x.Length; i++)
                    Form1.DebugSendMessage($"{_arguments[i]} : {x[i]}");


                c_0 = c;

                if (c < c_min)
                {
                    breakF = Break.c_cmin;
                    return;
                }

                if (Restrictions_g.All<Function>(r => r.calculate(x) <= 0))
                    return;
                //    mXparser.consolePrintln($"H ErrorMessage : {H.getErrorMessage()}");
                //    Form1.DebugSendMessage($"H ErrorMessage : {H.getErrorMessage()}");
                //mXparser.consolePrintln($"powell ErrorMessage : {powell.getErrorMessage()}");

                restrictions_xo.Clear();
                //Krok 3
                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    if (Restrictions_g[i].calculate(x) + _thetas[k][i] > 0)
                    {
                        restrictions_xo.Add(Math.Abs(Restrictions_g[i].calculate(x)));
                        c = restrictions_xo.Max();
                    }
                }
                SaveToFileLatex();

                //Krok 4
                mXparser.consolePrintln($"C {c}");
                Form1.DebugSendMessage($"C {c}");
                if (c < c_min)
                {

                    breakF = Break.c_cmin;
                    return;

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
                if (Restrictions_g.Count == 0)
                    return;
            }
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
                vs[i] = Math.Round(v1[i] + v2[i], round);
            }
            return vs;
        }

        double[] Gradient(int k)
        {
            double[] new_gradient = new double[x.Length];
            int j = 0;
            foreach (string arg in _arguments)
            {
                if (Restrictions_g.Count > 0)
                {
                    string sumR = "";
                    for (int i = 0; i < Restrictions_g.Count; i++)
                    {
                        if (H.calculate(Restrictions_g[i].calculate(x) + _thetas[k][i]) == 1)
                        {
                            sumR += $"({_deltas[k][i]})*";
                            sumR += $"(({_restrictions_g[i]})+({_thetas[k][i]}))^2";
                            sumR += "+";
                        }
                    }
                    if (sumR.Length > 0)
                        sumR = sumR.Remove(sumR.Length - 1, 1);
                    else
                        sumR = "0";
                    gradient = new Function("Powell", $"-der({_function_x},{arg})-der({sumR},{arg})", _arguments);
                }
                else
                    gradient = new Function("Powell", $"-der({_function_x},{arg})", _arguments);

                new_gradient[j++] = Math.Round(gradient.calculate(x), round);
            }
            //  mXparser.consolePrintln($"gradient ErrorMessage : {gradient.getErrorMessage()}");
            //  Form1.DebugSendMessage($"gradient ErrorMessage : {gradient.getErrorMessage()}");
            Form1.DebugSendMessage($"gradient: {String.Join("  ,  ", new_gradient)}");

            alfa = tau(k, new_gradient);
            if (alfa == -100000 || alfa == 0)
            {
                Form1.DebugSendMessage($"");
                Form1.DebugSendMessage($"Nie moża wyznaczyć tau");
                Form1.DebugSendMessage($"");
                if (round > 0)
                {
                    round--;
                    new_gradient = Gradient(k);
                }
                else
                    alfa = 0.000000000000001;
            }
            return new_gradient;
        }
        double tau(int k, double[] gradient)
        {
            for (int i = 0; i < gradient.Length; i++)
                if (gradient[i] == 0)
                    gradient[i] = 0.0000001;
            if (Restrictions_g.Count > 0)
            {
                string sumR = "";
                for (int i = 0; i < Restrictions_g.Count; i++)
                {
                    if (H.calculate(Restrictions_g[i].calculate(x) + _thetas[k][i]) == 1)
                    {
                        sumR += $"({_deltas[k][i]})*";
                        sumR += $"(({_restrictions_g[i]})+({_thetas[k][i]}))^2";
                        sumR += "+";
                    }
                }
                if (sumR.Length > 0)
                    sumR = sumR.Remove(sumR.Length - 1, 1);
                else
                    sumR = "0";
                powell = new Function("Powell", $"{_function_x}+({sumR})", _arguments);
            }
            else
                powell = new Function("Powell", $"{_function_x}", _arguments);
            string function_tau = $"Powell(";
            for (int i = 0; i < _arguments.Length; i++)
            {
                function_tau += $"({_arguments[i]})+tau*({gradient[i]})";
                if (i != _arguments.Length - 1)
                    function_tau += ", ";
            }
            function_tau += ")";
            Function tauF = new Function("tauF", function_tau, _arguments);
            tauF.addFunctions(powell);
            Argument tau = new Argument("tau");
            tauF.addArguments(tau);
            //  if (x.Length == 2)
            //       Form1.DebugSendMessage($"tau calculate : {tauF.calculate(x[0], x[1], 1)}");
            //   mXparser.consolePrintln($"tau ErrorMessage : {tauF.getErrorMessage()}");
            //  Form1.DebugSendMessage($"tau ErrorMessage : {tauF.getErrorMessage()}");

            function_tau = $"solve(der(tauF(";
            for (int i = 0; i < _arguments.Length; i++)
            {
                function_tau += $"({_arguments[i]}), ";
            }
            function_tau += "tau), tau), tau, -100000, 100000 )";


            Expression e = new Expression(function_tau);
            e.addFunctions(tauF);
            e.addFunctions(function);
            for (int i = 0; i < _arguments.Length; i++)
            {
                Argument x0 = new Argument(_arguments[i], x[i]);
                e.addArguments(x0);
            }
            e.addArguments(tau);
            // mXparser.consolePrintln($"tau' ErrorMessage : {e.getErrorMessage()}");
            // Form1.DebugSendMessage($"tau' ErrorMessage : {e.getErrorMessage()}");
            double alfa2 = e.calculate();
            return alfa2;
        }

        void StepEight()
        {

            if (k == 0 || chceckDoneSixStep[k - 1])
            {
                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    _thetas[k][i] = Math.Max(Restrictions_g[i].calculate(x) + _thetas[k][i], 0);
                }
                Form1.DebugSendMessage($"{k} deltas step8I =  {String.Join(" , ", _deltas[k])} thetas = {String.Join(" , ", _thetas[k])} ");
                //krok 7
                k++;
                _thetas.Add(new List<double>());
                _deltas.Add(new List<double>());
                for (int i = 0; i < _restrictions_g.Length; i++)
                {
                    _thetas[k].Add(_thetas[k - 1][i]);
                    _deltas[k].Add(_deltas[k - 1][i]);
                }
                x = Subtraction(x, MultiAlfa(Gradient(k)));
                _xPath.Add(x);
                funOptimumStep.Add(function.calculate(x));
                Form1.DebugSendMessage($"");
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
                        _thetas[k][i] = Math.Max(Restrictions_g[i].calculate(x) + _thetas[k][i], 0);
                    }
                    Form1.DebugSendMessage($"{k} deltas step8IIA  =  {String.Join(" , ", _deltas[k])} thetas = {String.Join(" , ", _thetas[k])} ");
                    //krok 7
                    Form1.DebugSendMessage($"");
                    k++;
                    _thetas.Add(new List<double>());
                    _deltas.Add(new List<double>());
                    for (int i = 0; i < _restrictions_g.Length; i++)
                    {
                        _thetas[k].Add(_thetas[k - 1][i]);
                        _deltas[k].Add(_deltas[k - 1][i]);
                    }
                    x = Subtraction(x, MultiAlfa(Gradient(k)));
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
                        if ((Math.Abs(Restrictions_g[i].calculate(x)) > m1 * c_0) && (Restrictions_g[i].calculate(x) + _thetas[k][i] > 0))
                        {
                            if (_deltas[k][i] < Math.Pow(10, round / 2))
                                _deltas[k][i] = m2 * _deltas[k][i];
                            _thetas[k][i] = Math.Round(_thetas[k][i] / m2, round);
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
                if ((Math.Abs(Restrictions_g[i].calculate(x)) > m1 * c_0) && (Restrictions_g[i].calculate(x) + _thetas[k][i] > 0))
                {
                    if (_deltas[k][i] < Math.Pow(10, round / 2))
                        _deltas[k][i] = m2 * _deltas[k][i];
                    _thetas[k][i] = Math.Round(_thetas[k][i] / m2, round);
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
                _thetas[k].Add(_thetas[k - 1][i]);
                _deltas[k].Add(_deltas[k - 1][i]);
            }
            x = Subtraction(x, MultiAlfa(Gradient(k)));
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
            powell = new Function("Powell", $"{_function_x}+({sum})", _arguments);
            new_x = powell.calculate(x);


            return new_x;
        }
    }
}
