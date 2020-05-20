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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Argument> Arguments = new List<Argument>();
        List<Function> Restrictions_g = new List<Function>();
        public DataGridViewButtonColumn button;
        bool Debug = true;
        static TextBox textBox14S;
        string[] komunikat = new[]{
        "c<c_min Metoda „minimum” ",
        "f*_k-f*_(k-1)<E",
         "x*_k-x*_(k-1)<E ",
        "Max(gradient)<E test stacjonarności",
        "ilość korków k>max_k"
        };
        Powell powell;
        List<double> values;
        string[] arguments;
        Function f;
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

        List<double[]> points = new List<double[]>();
        List<double[]> points2 = new List<double[]>();
        public Form1()
        {
            InitializeComponent();
            button = new DataGridViewButtonColumn();
            button.Text = "Usuń";
            button.HeaderText = @"Action";
            button.Name = "button";
            button.UseColumnTextForButtonValue = true;
            button.Width = 40;
            dataGridView1.Columns.Add(button);
            textBox14S = textBox14;
            if (Debug)
                textBox14.Visible = true;
            else
                textBox14.Visible = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (CheckInpuValue())
                return;


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


            powell = new Powell(comboBox1.Text, restrictions_g, arguments, c_min, max_k, c);
            powell.E = E;
            Thread InstanceCaller = new Thread(
                     new ThreadStart(Thread_powell));
            InstanceCaller.SetApartmentState(ApartmentState.STA);
            // Start the thread.
            InstanceCaller.Start();

        }
        void Thread_powell()
        {
            powell.Calculate(values.ToArray());
            double[] x = powell._xPath[powell.k];
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
            points.Clear();
            points2.Clear();
            textBox12.Text = "";
            textBox14.Text = "";
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
                textBox14S.Text = $"{value}\r\n {textBox14S.Text}"; // runs on UI thread
            });
        }

        public void SendMessage(string value)
        {
            textBox12.Invoke((MethodInvoker)delegate
            {
                textBox12.Text = $"{value}\r\n {textBox12.Text}"; // runs on UI thread
            });
        }

        void SendWindow()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(SendWindow));
                List<Function> func = new List<Function>();
                func.Add(f);
                foreach (var resF in Restrictions_g)
                    func.Add(resF);
                if (arguments.Length == 2 && powell._xPath.All<double[]>(o => o.All<double>(l => !double.IsNaN(l) && !double.IsInfinity(l) && !float.IsInfinity((float)l))))
                {
                    if (powell.funOptimumStep.All<double>(o => !double.IsNaN(o) && !double.IsInfinity(o) && !float.IsInfinity((float)o)))
                    {
                        MainWindow form = new MainWindow(func, powell._xPath, powell.funOptimumStep);
                        System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(form);
                        form.Show();
                        System.Windows.Threading.Dispatcher.Run();
                    }
                }
                return;
            }
        }
    }
}
