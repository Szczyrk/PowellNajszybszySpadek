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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int sx = 486;
        int sy = 277;

        List<Argument> Arguments = new List<Argument>();
        public DataGridViewButtonColumn button;

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

        List<Point> points = new List<Point>();
        List<Point> points2 = new List<Point>();
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
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            sx = pictureBox3.Width / 2;
            sy = pictureBox3.Height / 2;

            Function f = new Function("f", textBox1.Text, dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[0].Value.ToString()).ToArray());
            mXparser.consolePrintln(dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[0].Value.ToString()).ToArray());
            mXparser.consolePrintln();
            List<double> values = new List<double>();
            foreach (string value in dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[1].Value.ToString()).ToArray())
                values.Add(double.Parse(value));

            mXparser.consolePrintln(values.ToArray().ToString());

            string[] restrictions_g = checkedListBox1.CheckedItems.OfType<string>().ToArray();
            string[] arguments = dataGridView1.Rows.Cast<DataGridViewRow>().Select(r => r.Cells[0].Value.ToString()).ToArray();
            Powell powell = new Powell(textBox1.Text, restrictions_g, arguments, double.Parse(textBox6.Text), 1);

            mXparser.consolePrintln(powell.Calculate(values.ToArray()));
            for (int i = -200; i < 201; i++)
            {
                Point p = new Point(sx + i, sy - (int)(f.calculate(i)));
                points.Add(p);
            }
            for (int i = 0; i < points.Count - 1; i++)
            {
                pictureBox3.CreateGraphics().DrawLine(new Pen(Color.Red, 2), points[i], points[i + 1]);
            }

            foreach (Function func in powell.Restrictions_g)
            {
                for (int i = -200; i < 201; i++)
                {
                    Point p = new Point(sx + i, sy - (int)(func.calculate(i)));
                    points2.Add(p);
                }
                for (int i = 0; i < points.Count - 1; i++)
                {
                    pictureBox3.CreateGraphics().DrawLine(new Pen(Color.FromArgb(i), 2), points2[i], points2[i + 1]);
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            points.Clear();
            points2.Clear();
            pictureBox3.Invalidate();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add(textBox2.Text);
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
    }
}
