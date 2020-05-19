using nzy3D.Chart;
using nzy3D.Colors;
using nzy3D.Colors.ColorMaps;
using nzy3D.Maths;
using nzy3D.Plot3D.Builder;
using nzy3D.Plot3D.Builder.Concrete;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Primitives.Axes.Layout;
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.Legends;
using nzy3D.Plot3D.Rendering.Legends.Colorbars;
using nzy3D.Plot3D.Rendering.View;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using WindowsFormsApp1.nzy3d_api.Plot3D.Primitives;

namespace nzy3d_wpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private nzy3D.Chart.Controllers.Thread.Camera.CameraThreadController t;
        private IAxeLayout axeLayout;
        List<Function> function;
        List<Coord3d> coord3Ds = new List<Coord3d>();

        public MainWindow(List<Function> fs, List<List<double>> xy, List<double> z)
        {
            InitializeComponent();
            function = fs;
            for (int i = 0; i < xy.Count; i++)
                coord3Ds.Add(new Coord3d(xy[i][0], xy[i][1], z[i]));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // MVVM made simple : DataContext is current object
            this.DataContext = this;

            // Create the interop host control.
            System.Windows.Forms.Integration.WindowsFormsHost host =
                new System.Windows.Forms.Integration.WindowsFormsHost();

            // Create the Renderer 3D control.
            Renderer3D renderer = new Renderer3D();

            // Assign the Renderer 3D control as the host control's child.
            host.Child = renderer;

            // Add the interop host control to the Grid 
            // control's collection of child controls. 
            this.MainGrid.Children.Add(host);

            // Create a range for the graph generation
            double from;
            if (!double.TryParse(od.Text, out from) || from == 0)
            {
                from = -10;
            }
            double to;
            if (!double.TryParse(@do.Text, out to))
            {
                to = 10;
            }
            int step;
            if (!int.TryParse(kroki.Text, out step))
            {
                step = 20;
            }
            Range range = new Range(from, to);
            int steps = step;

            // Build a nice surface to display with cool alpha colors 
            // (alpha 0.8 for surface color and 0.5 for wireframe)
            Chart chart = new Chart(renderer, Quality.Nicest);
   
            Shape surface;
            for (int i = 0; i < function.Count; i++)
            {
                surface = Builder.buildOrthonomal(new OrthonormalGrid(range, steps, range, steps), new MyMapper(function[i]));
                double k = Convert.ToDouble(i) + 1;
                surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.zmin, surface.Bounds.zmax, new nzy3D.Colors.Color(k / (function.Count + 1), k / (function.Count + 1), k / (function.Count + 1), 1d));
                surface.FaceDisplayed = true;
                surface.WireframeDisplayed = true;
                surface.WireframeColor = nzy3D.Colors.Color.random();
                surface.WireframeColor.mul(new nzy3D.Colors.Color(1, 1, 1, 0.5));
                surface.LegendDisplayed = true;
                ColorbarLegend legend3 = new ColorbarLegend(surface, new AxeBoxLayout());
                surface.Legend = legend3;
                // Create the chart and embed the surface within
                chart.Scene.Graph.Add(surface);
            }


            MultiColorScatter surface2 = new MultiColorScatter(coord3Ds.ToArray(), new nzy3D.Colors.Color[] { nzy3D.Colors.Color.BLACK }, new ColorMapper(new ColorMapRainbow(), -10f, 10f), 5f);
            surface2.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface2.Bounds.zmin, surface2.Bounds.zmax, nzy3D.Colors.Color.BLACK);
            ColorbarLegend legend = new ColorbarLegend(surface2, new AxeBoxLayout());
            surface2.Legend = legend;
            // Create the chart and embed the surface within
            chart.Scene.Graph.Add(surface2);
            /*
            foreach (var legend2 in chart.Scene.Graph.Legends)
             {
                 var webImage = legend2.Image;
                 var imageControl = new System.Windows.Controls.Image();
                 imageControl.Source = ImageSourceFromBitmap(webImage);
                 Images.Children.Add(imageControl);
             }

            axeLayout = chart.AxeLayout;
            DisplayXTicks = true;
            DisplayXAxisLabel = true;
            DisplayYTicks = true;
            DisplayYAxisLabel = true;
            DisplayZTicks = true;
            DisplayZAxisLabel = true;
            DisplayTickLines = true;
            */
            // Create a mouse control
            nzy3D.Chart.Controllers.Mouse.Camera.CameraMouseController mouse = new nzy3D.Chart.Controllers.Mouse.Camera.CameraMouseController();
            mouse.addControllerEventListener(renderer);
            chart.addController(mouse);

            // This is just to ensure code is reentrant (used when code is not in Form_Load but another reentrant event)
            DisposeBackgroundThread();

            // Create a thread to control the camera based on mouse movements
            t = new nzy3D.Chart.Controllers.Thread.Camera.CameraThreadController();
            t.addControllerEventListener(renderer);
            mouse.addSlaveThreadController(t);
            chart.addController(t);
            t.Start();

            // Associate the chart with current control
            renderer.setView(chart.View);
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisposeBackgroundThread();
        }

        private void DisposeBackgroundThread()
        {
            if ((t != null))
            {
                t.Dispose();
            }
        }

        private bool _DisplayTickLines;
        public bool DisplayTickLines
        {
            get
            {
                return _DisplayTickLines;
            }
            set
            {
                _DisplayTickLines = value;
                OnPropertyChanged("DisplayTickLines");
                if (axeLayout != null)
                {
                    axeLayout.TickLineDisplayed = value;
                }
            }
        }

        private bool _DisplayXTicks;
        public bool DisplayXTicks
        {
            get
            {
                return _DisplayXTicks;
            }
            set
            {
                _DisplayXTicks = value;
                OnPropertyChanged("DisplayXTicks");
                if (axeLayout != null)
                {
                    axeLayout.XTickLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayYTicks;
        public bool DisplayYTicks
        {
            get
            {
                return _DisplayYTicks;
            }
            set
            {
                _DisplayYTicks = value;
                OnPropertyChanged("DisplayYTicks");
                if (axeLayout != null)
                {
                    axeLayout.YTickLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayZTicks;
        public bool DisplayZTicks
        {
            get
            {
                return _DisplayZTicks;
            }
            set
            {
                _DisplayZTicks = value;
                OnPropertyChanged("DisplayZTicks");
                if (axeLayout != null)
                {
                    axeLayout.ZTickLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayXAxisLabel;
        public bool DisplayXAxisLabel
        {
            get
            {
                return _DisplayXAxisLabel;
            }
            set
            {
                _DisplayXAxisLabel = value;
                OnPropertyChanged("DisplayXAxisLabel");
                if (axeLayout != null)
                {
                    axeLayout.XAxeLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayYAxisLabel;
        public bool DisplayYAxisLabel
        {
            get
            {
                return _DisplayYAxisLabel;
            }
            set
            {
                _DisplayYAxisLabel = value;
                OnPropertyChanged("DisplayYAxisLabel");
                if (axeLayout != null)
                {
                    axeLayout.YAxeLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayZAxisLabel;
        public bool DisplayZAxisLabel
        {
            get
            {
                return _DisplayZAxisLabel;
            }
            set
            {
                _DisplayZAxisLabel = value;
                OnPropertyChanged("DisplayZAxisLabel");
                if (axeLayout != null)
                {
                    axeLayout.ZAxeLabelDisplayed = value;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e);
        }
    }
}
