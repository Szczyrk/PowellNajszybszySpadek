using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using nzy3d_wpfDemo;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Argument> Arguments = new List<Argument>();
        List<Function> Restrictions_g = new List<Function>();
        public DataGridViewButtonColumn button;
        bool Debug = true;
        static TextBox textBox14S, textBox28S, textBox27S, textBox26S,
            textBox25S, textBox22S, textBox21S, textBox19S, textBox17S;
        string[] komunikat = new[]{
        "c<c_min Metoda „minimum” ",
        "|F*_k-F*_(k-1)|<E",
         "||x*_k-x*_(k-1)||<E",
        "ilość korków k>max_k",
       "<gradient F(x),gradient F(x)><E"
        };
        Powell powell;
        List<double> values;
        string[] arguments;
        Function f;
        Thread InstanceCaller;
        private volatile bool isRunning;
        public static EventWaitHandle waitHandle = new ManualResetEvent(initialState: true);

        public bool IsRunning { get => isRunning; set => isRunning = value; }

        class Argument
        {
            public string name;
            public string value;
            public Argument(string nameArgument, string valueArgument)
            {
                name = nameArgument;
                value = valueArgument;

            }
        }

        public Form1()
        {
            InitializeComponent();
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            button = new DataGridViewButtonColumn();
            button.Text = "Usuń";
            button.HeaderText = @"Action";
            button.Name = "button";
            button.UseColumnTextForButtonValue = true;
            button.Width = 40;
            dataGridView1.Columns.Add(button);
            textBox14S = textBox14;
            textBox28S = textBox28;
            textBox27S = textBox27;
            textBox26S = textBox26;
            textBox25S = textBox25;
            textBox22S = textBox22;
            textBox19S = textBox19;
            textBox17S = textBox17;
            if (Debug)
                textBox14.Visible = true;
            else
                textBox14.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            IsRunning = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (CheckInpuValue())
                return;
            button1.Visible = false;
            IsRunning = true;

            button5.Visible = true;
            button6.Visible = false;
            if (InstanceCaller != null)
                InstanceCaller.Abort();
            waitHandle.Set();

            f = new Function("f", comboBox1.Text, dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[0].Value.ToString()).ToArray());

            mXparser.consolePrintln(dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[0].Value.ToString()).ToArray());
            mXparser.consolePrintln();
            values = new List<double>();
            foreach (string value in dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[1].Value.ToString()).ToArray())
                values.Add(double.Parse(value));

            mXparser.consolePrintln(values.ToArray().ToString());

            string[] restrictions_g = checkedListBox1.CheckedItems.OfType<string>().ToArray();
            arguments = dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[0].Value.ToString()).ToArray();
            Restrictions_g.Clear();
            for (int i = 0; i < restrictions_g.Length; i++)
            {
                Restrictions_g.Add(new Function("Restriction" + i, restrictions_g[i], arguments));
            }
            double c_min;
            if (!double.TryParse(textBox5.Text, out c_min))
            {
                c_min = 0;
            }
            int max_k = 10;
            int.TryParse(textBox1.Text, out max_k);

            double c;
            if (!double.TryParse(textBox6.Text, out c))
            {
                c = 0;
            }

            double E;
            if (!double.TryParse(textBox16.Text, out E))
            {
                E = 0.001;
            }

            double m2;
            if (!double.TryParse(textBox33.Text, out m2))
            {
                m2 = 10;
            }

            double m1;
            if (!double.TryParse(textBox34.Text, out m1))
            {
                m1 = 0.25;
            }



            powell = new Powell(comboBox1.Text, restrictions_g, arguments, c_min, max_k, c);
            powell.E = E;
            powell.m2 = m2;
            powell.m1 = m1;

            InstanceCaller = new Thread(
                     new ThreadStart(Thread_powell));
            InstanceCaller.SetApartmentState(ApartmentState.STA);
            InstanceCaller.IsBackground = true;
            // Start the thread.
            InstanceCaller.Start();

        }
        void Thread_powell()
        {
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            powell.Calculate(values.ToArray());
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            double[] x = powell._xPath[powell.k];
            for (int i = 0; i < Restrictions_g.Count; i++)
                SendMessage($"g{i}(x*) = {Restrictions_g[i].calculate(x)}");

            for (int i = 0; i < arguments.Length; i++)
            {
                mXparser.consolePrintln(x[i]);
                SendMessage($"{arguments[i]}: {x[i]}");
            }
            mXparser.consolePrintln($"pkt_opt: {powell.funOptimumStep[powell.k]} \r\n");
            SendMessage($"pkt_opt: { powell.funOptimumStep[powell.k]}");
            mXparser.consolePrintln($"k: { powell.k} \r\n");
            SendMessage($"ilość kroków: { powell.k}");
            mXparser.consolePrintln($"Przerawnie: {komunikat[(int)powell.breakF]} \r\n");



            if (powell.breakF == 0)
                SendMessage($"c: {powell.c}");
            SendMessage($"Przerwanie: { komunikat[(int)powell.breakF]}");
            SendWindow();
            IsRunning = false;
            SendMessage($"Czas wykonywania: {elapsedTime}");
            SendMessage("");
        }

        private bool CheckInpuValue()
        {
            if (comboBox1.Text == "")
            {
                textBox12.Text = $" Proszę podać funkcję \r\n{textBox12.Text}";
                return true;
            }
            if (textBox5.Text == "")
            {
                textBox12.Text = $" Proszę podać c_min \r\n{textBox12.Text}";
                return true;
            }
            if (dataGridView1.Rows.Count == 0)
            {
                textBox12.Text = $"Proszę podać argumenty i wartości początkowe \r\n{textBox12.Text}";
                return true;
            }
            if (textBox1.Text == "")
            {
                textBox12.Text = $"Proszę podać k \r\n{textBox12.Text}";
                return true;
            }
            if (textBox6.Text == "")
            {
                textBox12.Text = $"Proszę podać c \r\n{textBox12.Text}";
                return true;
            }
            if (textBox16.Text == "")
            {
                textBox12.Text = $"Proszę podać E \r\n{textBox12.Text}";
                return true;
            }
            return false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox12.Text = "";
            textBox14.Text = "";
            textBox28.Text = "";
            textBox27.Text = "";
            textBox26.Text = "";
            textBox25.Text = "";
            textBox22.Text = "";
            textBox19.Text = "";
            textBox17.Text = "";
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add(textBox2.Text);
            checkedListBox1.SetSelected(checkedListBox1.Items.Count - 1, true);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Argument argument = new Argument(textBox10.Text, textBox11.Text);
            Arguments.Add(argument);
            dataGridView1.Rows.Add(argument.name, argument.value);
        }

        private void Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < Arguments.Count)
            {
                Arguments.RemoveAt(e.RowIndex);
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }
        public static void DebugSendMessage(string value)
        {
            textBox14S.Invoke((MethodInvoker)delegate
            {
                textBox14S.AppendText(value); // runs on UI thread
                textBox14S.AppendText(Environment.NewLine);
            });
        }

        public static void SendMessageC(string value)
        {
            textBox28S.Invoke((MethodInvoker)delegate
            {
                textBox28S.AppendText(value); // runs on UI thread
                textBox28S.AppendText(Environment.NewLine);
            });
        }

        public static void SendMessageX1(string value)
        {
            textBox25S.Invoke((MethodInvoker)delegate
            {
                textBox25S.AppendText(value); // runs on UI thread
                textBox25S.AppendText(Environment.NewLine);
            });
        }

        public static void SendMessageX2(string value)
        {
            textBox26S.Invoke((MethodInvoker)delegate
            {
                textBox26S.AppendText(value); // runs on UI thread
                textBox26S.AppendText(Environment.NewLine);
            });
        }


        public static void SendMessageF(string value)
        {
            textBox27S.Invoke((MethodInvoker)delegate
            {
                textBox27S.AppendText(value); // runs on UI thread
                textBox27S.AppendText(Environment.NewLine);
            });
        }
        public static void SendMessageE4(string value)
        {
            textBox22S.Invoke((MethodInvoker)delegate
            {
                textBox22S.AppendText(value); // runs on UI thread
                textBox22S.AppendText(Environment.NewLine);
            });
        }

        public static void SendMessageE3(string value)
        {
            textBox21S.Invoke((MethodInvoker)delegate
            {
                textBox21S.AppendText(value); // runs on UI thread
                textBox21S.AppendText(Environment.NewLine);
            });
        }
        public static void SendMessageE2(string value)
        {
            textBox19S.Invoke((MethodInvoker)delegate
            {
                textBox19S.AppendText(value); // runs on UI thread
                textBox19S.AppendText(Environment.NewLine);
            });
        }
        public static void SendMessageE1(string value)
        {
            textBox17S.Invoke((MethodInvoker)delegate
            {
                textBox17S.AppendText(value); // runs on UI thread
                textBox17S.AppendText(Environment.NewLine);
            });
        }
        public void SendMessage(string value)
        {
            textBox12.Invoke((MethodInvoker)delegate
            {
                textBox12.AppendText(value); // runs on UI thread
                textBox12.AppendText(Environment.NewLine);
            });
        }

        void SendWindow()
        {
            button5.Invoke((MethodInvoker)delegate
            {
                button5.Visible = false; // runs on UI thread
            });
            button6.Invoke((MethodInvoker)delegate
            {
                button6.Visible = false; // runs on UI thread
            });
            button1.Invoke((MethodInvoker)delegate
            {
                button1.Visible = true; // runs on UI thread
            });
            if (InvokeRequired)
            {
              //  Invoke(new Action(SendWindow));
                if (arguments.Length == 2 && powell._xPath.All<double[]>(o => o.All<double>(l => !double.IsNaN(l) && !double.IsInfinity(l) && !float.IsInfinity((float)l))))
                {
                    if (powell.funOptimumStep.All<double>(o => !double.IsNaN(o) && !double.IsInfinity(o) && !float.IsInfinity((float)o)))
                    {
                        Thread thread = new Thread(
                                  new ThreadStart(Window));
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.IsBackground = true;
                        // Start the thread.
                        thread.Start();

                    }
                }
                return;
            }
        }
        void Window()
        {
            List<Function> func = new List<Function>();
            func.Add(f);
            foreach (var resF in Restrictions_g)
                func.Add(resF);
            MainWindow form = new MainWindow(func, powell._xPath, powell.funOptimumStep);
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(form);
            form.Show();
            System.Windows.Threading.Dispatcher.Run();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            IsRunning = false;
            waitHandle.Reset();
            button5.Visible = false;
            button6.Visible = true;
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            IsRunning = true;
            button1.Visible = false;
            waitHandle.Set();
            button5.Visible = true;
            button6.Visible = false;
        }

    }
}
