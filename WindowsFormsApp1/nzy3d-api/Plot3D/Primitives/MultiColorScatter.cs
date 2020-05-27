using nzy3D.Colors;
using nzy3D.Events;
using nzy3D.Maths;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Primitives.Axes.Layout.Providers;
using nzy3D.Plot3D.Primitives.Axes.Layout.Renderers;
using nzy3D.Plot3D.Rendering.Legends.Colorbars;
using nzy3D.Plot3D.Rendering.View;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.nzy3d_api.Plot3D.Primitives
{
    class MultiColorScatter : AbstractDrawable, IMultiColorable
    {

        protected Coord3d[] coordinates;
        protected Color[] colors;
        protected float width;
        protected ColorMapper mapper;

        public ColorMapper ColorMapper
        {
            get { return mapper; }
            set
            {
                mapper = value;
                lock (coordinates)
                {
                    foreach (var c in coordinates)
                    {
                        IMultiColorable cIM = c as IMultiColorable;
                        ISingleColorable cIC = c as ISingleColorable;
                        if (cIM != null)
                        {
                            cIM.ColorMapper = value;
                        }
                        else if (cIC != null)
                        {
                            cIC.Color = value.Color(c);
                        }
                    }
                }
                fireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
            }
        }

        public MultiColorScatter(Coord3d[] coordinates, Color[] colors, ColorMapper mapper) : base()
        {
            this.coordinates = coordinates;
            this.colors = colors;
            this.mapper = mapper;
            this.width = 1.0f;
        }
        public MultiColorScatter(Coord3d[] coordinates, ColorMapper mapper) : base()
        {
            this.coordinates = coordinates;
            this.colors = null;
            this.mapper = mapper;
            this.width = 1.0f;
        }
        public MultiColorScatter(Coord3d[] coordinates, Color[] colors, ColorMapper mapper, float width) : base()
        {
            _bbox = new BoundingBox3d();
            setData(coordinates);
            setColors(colors);
            setWidth(width);
            setColorMapper(mapper);
        }

        public void clear()
        {
            coordinates = null;
            _bbox.reset();
        }

        public void enableColorBar(ITickProvider provider, ITickRenderer renderer)
        {
            Legend = (new ColorbarLegend(this, provider, renderer));
            LegendDisplayed = (true);
        }

        /**********************************************************************/

        public override void Draw(Camera cam)
        {
            if (_transform != null)
                _transform.Execute();

            GL.PointSize(width);
            GL.Begin(BeginMode.LineStrip);
            if (coordinates != null)
            {
                foreach (Coord3d coord in coordinates)
                {
                    Color color = mapper.Color(coord); // TODO: should store result in the point color
                    GL.Color4(color.r, color.g, color.b, color.a);
                    GL.Vertex3(coord.x, coord.y, coord.z);
                }
            }
            GL.End();
            GL.PointSize(width);
            GL.Begin(BeginMode.Points);
            if (coordinates != null)
            {
                for (int i = 0; i < coordinates.Length; i++)
                {
                    Color color = mapper.Color(coordinates[i]);
                    if (i == coordinates.Length - 1)
                        color = Color.RED;
                    GL.Color4(color.r, color.g, color.b, color.a);
                    GL.Vertex3(coordinates[i].x, coordinates[i].y, coordinates[i].z);
                }
            }
            GL.End();
        }

        /*********************************************************************/

        /** 
         * Set the coordinates of the point.
         * @param xyz point's coordinates
         */
        public void setData(Coord3d[] coordinates)
        {
            this.coordinates = coordinates;

            _bbox.reset();
            foreach (Coord3d c in coordinates)
                _bbox.add(c);
        }
        public Coord3d[] getData()
        {
            return coordinates;
        }
        public void setColors(Color[] colors)
        {
            this.colors = colors;

            fireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
        }

        public ColorMapper getColorMapper()
        {
            return mapper;
        }


        public void setColorMapper(ColorMapper mapper)
        {
            this.mapper = mapper;
        }

        /**
         * Set the width of the point.
         * @param width point's width
         */
        public void setWidth(float width)
        {
            this.width = width;
        }
    }
}
