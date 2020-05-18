using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nzy3d_wpfDemo
{
    class MyMapper : nzy3D.Plot3D.Builder.Mapper
    {
        Function function;

        public MyMapper(Function function)
        {
            this.function = function;
        }

        public void setFunction(Function f)
        {
            function = f;
        }
        public override double f(double x, double y)
        {
            return function.calculate(x, y);
        }

    }
}
