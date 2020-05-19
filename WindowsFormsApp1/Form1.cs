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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Argument> Arguments = new List<Argument>();
        List<Function> Restrictions_g = new List<Function>();
        public DataGridViewButtonColumn button;
        bool Debug = true;
        static TextBox textBox14S;

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


            Function f = new Function("f", comboBox1.Text, dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[0].Value.ToString()).ToArray());

            mXparser.consolePrintln(dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[0].Value.ToString()).ToArray());
            mXparser.consolePrintln();
            List<double> values = new List<double>();
            foreach (string value in dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[1].Value.ToString()).ToArray())
                values.Add(double.Parse(value));

            mXparser.consolePrintln(values.ToArray().ToString());

            string[] restrictions_g = checkedListBox1.CheckedItems.OfType<string>().ToArray();
            string[] arguments = dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[0].Value.ToString()).ToArray();
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


            Powell powell = new Powell(comboBox1.Text, restrictions_g, arguments, c_min, max_k, c);


            double[] x = powell.Calculate(values.ToArray());
            for (int i = 0; i < arguments.Length; i++)
            {
                mXparser.consolePrintln(x[i]);
                textBox12.Text = $"{arguments[i]}: {x[i]}\r\n {textBox12.Text}";
            }
            mXparser.consolePrintln($"pkt_opt: {powell.funOptimumStep[powell.k]} \r\n");
            textBox12.Text = $"pkt_opt: { powell.funOptimumStep[powell.k]} \r\n {textBox12.Text}";
            mXparser.consolePrintln($"k: { powell.k} \r\n");
            textBox12.Text = $"ilość kroków: { powell.k} \r\n {textBox12.Text}";

            List<Function> func = new List<Function>();
            func.Add(f);
            foreach (var resF in Restrictions_g)
                func.Add(resF);
            if (arguments.Length == 2 && powell._xPath.All<double[]>(o => o.All<double>(l => !double.IsNaN(l))))
            {
                MainWindow form = new MainWindow(func, powell._xPath, powell.funOptimumStep);
                System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(form);
                form.Show();
            }
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
                textBox12.Text = $"Proszę podać argumenty i wartości poczkowe \r\n{textBox12.Text}";
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
            textBox14S.Text = $"{value} \r\n{textBox14S.Text}";
        }

    }
}
