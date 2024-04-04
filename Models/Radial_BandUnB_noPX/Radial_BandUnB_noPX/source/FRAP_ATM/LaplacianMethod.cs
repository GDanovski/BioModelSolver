﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial_BandUnB_noPX.FRAP_ATM
{
    public class LaplacianMethod : IDisposable
    {
        protected double dr;
        protected int MaxPoints;
        protected double Rsq;
        public Storage.PointLap[] LapPoints { get; set; }

        public LaplacianMethod(double dr, int MaxPoints)
        {
            this.dr = dr;
            this.MaxPoints = MaxPoints;
            this.Rsq = dr * dr;

            this.LapPoints = new Storage.PointLap[MaxPoints];

            LapPoints_Calculate(this.LapPoints);
        }
        public virtual void Dispose()
        {
            this.LapPoints = null;
        }
        public virtual double GetLaplacian(double[] Ad_array, double[] Ad_Lap_array, double[] Pd_array, double[] Pd_Lap_array,
            double[] Al_array, double[] Al_Lap_array, double[] Pl_array, double[] Pl_Lap_array)
        {
            return 0d;
        }
        private static void LapPoints_Calculate(Storage.PointLap[] LapPoints)
        {
            int length = LapPoints.Length;
            for (int i = 0; i < length; i++)
            {
                LapPoints[i] = new Storage.PointLap();
                LapPoints[i].m1 = Math.Abs(i - 1);
                LapPoints[i].m2 = Math.Abs(i - 2);
                LapPoints[i].p1 = length - 1 - Math.Abs(i + 2 - length);
                LapPoints[i].p2 = length - 1 - Math.Abs(i + 3 - length);
                LapPoints[i].index = i;
            }
        }
    }
    /// <summary>
    /// 2nd order Laplacian
    /// </summary>
    public class LaplacianMethod2 : LaplacianMethod
    {
        public LaplacianMethod2(double dr, int MaxPoints)
            : base(dr, MaxPoints) { }
        public override double GetLaplacian(double[] Ad_array, double[] Ad_Lap_array, double[] Pd_array, double[] Pd_Lap_array,
            double[] Al_array, double[] Al_Lap_array, double[] Pl_array, double[] Pl_Lap_array)
        {
            Parallel.ForEach(this.LapPoints, (pr) =>
            {
                int jrm1 = pr.m1;
                int jrp1 = pr.p1;
                int jr = pr.index;

                LapFunction(Ad_array, Ad_Lap_array,
                    jr, jrm1, jrp1);

                LapFunction(Pd_array, Pd_Lap_array,
                    jr, jrm1, jrp1);

                LapFunction(Al_array, Al_Lap_array,
                    jr, jrm1, jrp1);

                LapFunction(Pl_array, Pl_Lap_array,
                    jr, jrm1, jrp1);
            });
            return 0d;
        }
        private void LapFunction(double[] M_array, double[] Lap_array,
            int jr, int jrm1, int jrp1)
        {
            double r = dr * jr;

            double L1;
            if (jr != 0)
                L1 = (M_array[jrp1] - M_array[jrm1]) / (r * (2 * dr));
            else
                L1 = (M_array[1] - M_array[0]) / Rsq;

            Lap_array[jr]
                = (M_array[jrp1] + M_array[jrm1]
                    - 2 * M_array[jr]) / Rsq
                + L1;
        }
    }
    /// <summary>
    /// 4th order Laplacian
    /// </summary>
    public class LaplacianMethod4 : LaplacianMethod
    {
        public LaplacianMethod4(double dr, int MaxPoints)
            : base(dr, MaxPoints) { }
        public override double GetLaplacian(double[] Ad_array, double[] Ad_Lap_array, double[] Pd_array, double[] Pd_Lap_array,
            double[] Al_array, double[] Al_Lap_array, double[] Pl_array, double[] Pl_Lap_array)
        {
            Parallel.ForEach(this.LapPoints, (pr) =>
            {
                int jrm1 = pr.m1;
                int jrm2 = pr.m2;
                int jrp1 = pr.p1;
                int jrp2 = pr.p2;
                int jr = pr.index;

                LapFunction(Ad_array, Ad_Lap_array,
                    jr, jrm1, jrm2, jrp1, jrp2);

                LapFunction(Pd_array, Pd_Lap_array,
                     jr, jrm1, jrm2, jrp1, jrp2);

                LapFunction(Al_array, Al_Lap_array,
                    jr, jrm1, jrm2, jrp1, jrp2);

                LapFunction(Pl_array, Pl_Lap_array,
                     jr, jrm1, jrm2, jrp1, jrp2);
            });
            return 0d;
        }
        private void LapFunction(double[] M_array, double[] Lap_array,
            int jr, int jrm1, int jrm2, int jrp1, int jrp2)
        {
            double r = dr * jr;
            double L1;

            if (jr != 0)
                L1 = (M_array[jrp1] - M_array[jrm1]) / (r * (2 * dr));
            else
                L1 = (M_array[1] - M_array[0]) / Rsq;

            Lap_array[jr]
                = (-M_array[jrm2] / 12
                    + 4 * M_array[jrm1] / 3
                    - 5 * M_array[jr] / 2
                    + 4 * M_array[jrp1] / 3
                    - M_array[jrp2] / 12) / Rsq
                + L1;
        }
    }
}
