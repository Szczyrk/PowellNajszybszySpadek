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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int sx = 486;
        int sy = 277;

        List<Point> points = new List<Point>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            //sx = pictureBox3.Width / 2;
            //sy = pictureBox3.Height / 2;
           
            Function f = new Function("f", textBox1.Text, "x");
            for (int i = -300; i < 301; i++)
            {
                Point p = new Point(sx+i,sy- (int)(f.calculate(i)));
                points.Add(p);
                mXparser.consolePrintln(f.calculate(i));
            }
            for (int i = 0; i < points.Count -1; i++)
            {
                cartesianChart1.cre.CreateGraphics().DrawLine(new Pen(Color.Red, 2), points[i], points[i+1]);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            points.Clear();
            pictureBox3.Invalidate();
        }
    }
}
