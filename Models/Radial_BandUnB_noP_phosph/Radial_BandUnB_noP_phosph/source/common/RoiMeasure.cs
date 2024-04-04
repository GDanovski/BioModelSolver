using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial_BandUnB_noP_phosph
{
    class RoiMeasure
    {
        /// <summary>
        /// Get mean intensity value in an oval ROI.
        /// </summary>
        /// <param name="ns">distance to center in pxls</param>
        /// <param name="nq">radius in pxls</param>
        /// <param name="dr">Size of 1 pxl in um</param>
        /// <param name="f">1D array with intensity values</param>
        /// <returns></returns>
        public static double measureOvalRoi(double ns, double nq, double dr, double[] f)
        {
            double Total = 0.0d;
            double Area = 0.0d;

            double val = 0;

            for (int x = 0, y = 0; x < f.Length * 2 - 1; x++)
            {
                for (y = 0; y < f.Length * 2 - 1; y++)
                {
                    var x1 = x - f.Length;
                    var y1 = y - f.Length;

                    if ((x1 - ns) * (x1 - ns) + y1 * y1 < nq * nq)
                    {
                        val = (int)Math.Round(f.Length - GetDistance(x, y, f.Length, f.Length));
                        if (val < 0) val = 0;
                        if (val >= f.Length) val = f.Length - 1;
                        val = f[f.Length - 1 - (int)val];

                        Total += val;
                        Area += 1;
                    }
                }
            }

            return Area != 0.0d ? Total / Area : 0.0d;
        }
        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
        /*
        /// <summary>
        /// Get mean intensity value in an oval ROI.
        /// </summary>
        /// <param name="ns">distance to center in pxls</param>
        /// <param name="nq">radius in pxls</param>
        /// <param name="dr">Size of 1 pxl in um</param>
        /// <param name="f">1D array with intensity values</param>
        /// <returns></returns>
        public static double measureOvalRoi(double ns, double nq, double dr, double[] f)
        {
            double Total = 0.0d;
            double Area = 0.0d;
            double s = ns * dr;
            double q = nq * dr;

            for (double i = ns - nq; i <= ns + nq; i++)
            {
                double u = i * dr;

                double a = ((u * u + s * s - q * q) / (2.0d * u * s));

                if (a > 1)
                    a = 1;
                else if (a < -1)
                    a = -1;

                Total += f[(int)i] * 2.0d * u * Math.Acos(a);
                Area += 2.0d * u * Math.Acos(a);
            }

            return Area != 0.0d ? Total / Area : 0.0d;
        }
        */
    }
}
