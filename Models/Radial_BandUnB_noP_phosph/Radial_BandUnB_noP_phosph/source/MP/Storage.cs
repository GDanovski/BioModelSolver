using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial_BandUnB_noP_phosph.MP_ATM_MDC
{
    public class Storage
    {
        public class PointLap
        {
            public int index { get; set; }

            public int m1 { get; set; }

            public int m2 { get; set; }

            public int p1 { get; set; }

            public int p2 { get; set; }

        }
        public class SiteParameters
        {
            public double DDD_P { get; set; }
            public double DDD_A { get; set; }
            public double DDD_M { get; set; }
            public double k_0 { get; set; }
            public double p { get; set; }
            public double k_1 { get; set; }
            public double k_on { get; set; }
            public double k_off { get; set; }
            public double k_3 { get; set; }
            public double k_d { get; set; }
            public double k_4 { get; set; }
            public double kfh { get; set; }
            public double krh { get; set; }
            public double k_on_MDC { get; set; }
            public double k_off_MDC { get; set; }
            public double X0 { get; set; }
            public double X1 { get; set; }
            public double A0 { get; set; }
            public double H0 { get; set; }
            public double R1 { get; set; }
            public double S0 { get; set; }
            public double S1 { get; set; }
            public double S1_MDC { get; set; }
            public double t0 { get; set; }
            public int DMGOnlyInS { get; set; }
        };
    }
}
