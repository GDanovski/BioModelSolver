using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Radial_BandUnB_noP_phosph.FRAP_ATM
{
    public class IntegrationMethod : IDisposable
    {
        protected Storage.SiteParameters SiteParams;
        protected int Site_1_On;
        protected int Site_2_On;
        protected double DDD_A;
        protected double DDD_P;
        protected double DDD_M;
        protected double dt;
        protected int MaxPoints;
        protected double dr;

        public double[] X0_T_array { get; set; }
        public double[] X1_T_array{ get; set; }
        public double[] Xm_array{ get; set; }        
        public double[] X0_profile{ get; set; }
        public double[] X1_profile{ get; set; }

        public double[] Bd_array { get; set; }
        public double[] Ad_array { get; set; }
        public double[] AXd_array { get; set; }
        public double[] PXd_array { get; set; }
        public double[] Pd_array { get; set; }
        public double[] Ad_T_array { get; set; }
        public double[] Ad_Lap_array{ get; set; }
        public double[] Pd_Lap_array{ get; set; }

        public double[] Bl_array { get; set; }
        public double[] Al_array { get; set; }
        public double[] AXl_array { get; set; }
        public double[] PXl_array { get; set; }
        public double[] Pl_array { get; set; }
        public double[] Al_T_array { get; set; }
        public double[] Al_Lap_array { get; set; }
        public double[] Pl_Lap_array { get; set; }

        protected LaplacianMethod TheLaplacianMethod;
        public IntegrationMethod(Storage.SiteParameters SiteParams, double dt,
            double dr, int MaxPoints,
            LaplacianMethod LaplacianMethod)
        {
            this.SiteParams = SiteParams;
            this.DDD_A = SiteParams.DDD_A;
            this.DDD_P = SiteParams.DDD_P;
            this.DDD_M = SiteParams.DDD_M;
            this.dt = dt;
            this.dr = dr;
            this.MaxPoints = MaxPoints;
            this.Site_1_On = 0;
            this.Site_2_On = 0;
            this.X0_T_array = new double[MaxPoints];
            this.X1_T_array = new double[MaxPoints];
            this.Xm_array = new double[MaxPoints];
            this.X0_profile = new double[MaxPoints];
            this.X1_profile = new double[MaxPoints];

            this.Ad_Lap_array = new double[MaxPoints];
            this.Pd_Lap_array = new double[MaxPoints];
            this.Bd_array = new double[MaxPoints];
            this.Ad_array = new double[MaxPoints];
            this.AXd_array = new double[MaxPoints];
            this.Pd_array = new double[MaxPoints];
            this.PXd_array = new double[MaxPoints];
            this.Ad_T_array = new double[MaxPoints];

            this.Al_Lap_array = new double[MaxPoints];
            this.Pl_Lap_array = new double[MaxPoints];
            this.Bl_array = new double[MaxPoints];
            this.Al_array = new double[MaxPoints];
            this.AXl_array = new double[MaxPoints];
            this.Pl_array = new double[MaxPoints];
            this.PXl_array = new double[MaxPoints];
            this.Al_T_array = new double[MaxPoints];

            this.TheLaplacianMethod = LaplacianMethod;
        }
        public void PrintArrays()
        {
            PrintArray(X0_T_array, "X0_T");
            PrintArray(X1_T_array, "X1_T");
            PrintArray(Xm_array, "Xm");
            PrintArray(X0_profile, "X0");
            PrintArray(X1_profile, "X1");

            PrintArray(Bd_array, "Bd");
            PrintArray(Ad_array, "Ad");
            PrintArray(AXd_array, "AXd");
            PrintArray(PXd_array, "prd");
            PrintArray(Pd_array, "Pd");
            PrintArray(Ad_T_array, "Ad_T");
            PrintArray(Ad_Lap_array, "Ad_Lap");
            PrintArray(Pd_Lap_array, "Pd_Lap");

            PrintArray(Bl_array, "Bl");
            PrintArray(Al_array, "Al");
            PrintArray(AXl_array, "AXl");
            PrintArray(PXl_array, "prl");
            PrintArray(Pl_array, "Pl");
            PrintArray(Al_T_array, "Al_T");
            PrintArray(Al_Lap_array, "Al_Lap");
            PrintArray(Pl_Lap_array, "Pl_Lap");

        }
        private void PrintArray(double[] values, string Name)
        {
            if (!Directory.Exists("results")) Directory.CreateDirectory("results");

            using (StreamWriter ATM_sumstream = new StreamWriter(@"results\" + Name + "_CPU.txt"))
            {
                ATM_sumstream.WriteLine(string.Join("\t", values));
                ATM_sumstream.Close();
            }
        }
        public void Dispose()
        {
            Xm_array = null;
            X1_T_array = null;
            X0_T_array = null;

            Bd_array = null;
            Ad_array = null;
            AXd_array = null;
            Pd_array = null;
            PXd_array = null;
            Ad_T_array = null;
            Ad_Lap_array = null;
            Pd_Lap_array = null;

            Bl_array = null;
            Al_array = null;
            AXl_array = null;
            Pl_array = null;
            PXl_array = null;
            Al_T_array = null;
            Al_Lap_array = null;
            Pl_Lap_array = null;

            X0_profile = null;
            X1_profile = null;

            TheLaplacianMethod.Dispose();
        }
        public void Initialize()
        {
            int jr;

            for (jr = 0; jr < MaxPoints; jr++)
            {
                this.X0_T_array[jr] = 0d;
                this.X1_T_array[jr] = 0d;
                this.Xm_array[jr] = 0d;

                this.AXd_array[jr] = 0d;
                this.Pd_array[jr] = 0d;
                this.PXd_array[jr] = 0d;
                this.Bd_array[jr] = 0d;
                this.Ad_T_array[jr] = 0d;

                this.AXl_array[jr] = 0d;
                this.Pl_array[jr] = 0d;
                this.PXl_array[jr] = 0d;
                this.Bl_array[jr] = 0d;
                this.Al_T_array[jr] = SiteParams.A0;

                this.Al_Lap_array[jr] = 0d;
                this.Pl_Lap_array[jr] = 0d;
                this.Ad_Lap_array[jr] = 0d;
                this.Pd_Lap_array[jr] = 0d;

                // initial damage site profiles

                double r = dr * jr;

                double Rsq = r * r;

                X0_profile[jr]
                    = SiteParams.X0 * Math.Exp(-Rsq / Math.Pow(SiteParams.R1, SiteParams.p));

                X1_profile[jr]
                    = SiteParams.X1 * Math.Exp(-Rsq / Math.Pow(SiteParams.R1, SiteParams.p));

                if (SiteParams.DMGOnlyInS != 0 && Rsq > SiteParams.S1 * SiteParams.S1)
                {
                    X0_profile[jr] = 0;
                    X1_profile[jr] = 0;
                }

            }
        }
        public void ManageSites(double t)
        {
            // initial X0 distribution (focus 1)

            for (int jr = 0; jr < MaxPoints; jr++)
            {
                X0_T_array[jr]
                    = (1 - Math.Exp(-SiteParams.k_0 * (t - SiteParams.t0)))
                    * X0_profile[jr];
                X1_T_array[jr]
                    = (1 - Math.Exp(-SiteParams.k_0 * (t - SiteParams.t0)))
                    * X1_profile[jr];

                // diffusion of free ATM
                if (SiteParams.FRAP_On)
                    Ad_array[jr]
                      = Ad_T_array[jr]
                      - AXd_array[jr]
                      - PXd_array[jr]
                      - Bd_array[jr];

                Al_array[jr]
                  = Al_T_array[jr]
                  - AXl_array[jr]
                  - PXl_array[jr]
                  - Bl_array[jr];
            }
        }
        public void ManageFrapSites()
        {
            for (int jr = 0; jr < MaxPoints; jr++)
            {
                double r = dr * jr;
                double Rsq = r * r;

                if (Rsq <= SiteParams.R_F * SiteParams.R_F)
                {
                    Ad_T_array[jr] = Al_T_array[jr];
                    Al_T_array[jr] = 0.0d;
                    AXd_array[jr] = AXl_array[jr];
                    AXl_array[jr] = 0.0d;
                    PXd_array[jr] = PXl_array[jr];
                    PXl_array[jr] = 0.0d;
                    Bd_array[jr] = Bl_array[jr];
                    Bl_array[jr] = 0.0d;
                }
            }
        }
        public virtual void Increment() { }
        public double GetUnboundATM(int jr)
        {
            return Al_T_array[jr]
                - AXl_array[jr] - PXl_array[jr] - Bl_array[jr];
        }

        public double GetBoundATM(int jr)
        {
            return AXl_array[jr]
                + PXl_array[jr] + Bl_array[jr];
        }
        public double GetXm(int jr) { return Xm_array[jr]; }

        public double GetB(int jr) { return Bl_array[jr]; }

        public double GetX0(int jr) { return X0_T_array[jr]; }

        public double GetX1(int jr) { return X1_T_array[jr]; }

    }
    public class EulerIntegrationMethod : IntegrationMethod
    {
        public EulerIntegrationMethod(Storage.SiteParameters SiteParams, double dt,
            double dr, int MaxPoints,
            LaplacianMethod LaplacianMethod)
            : base(SiteParams, dt, dr,
                  MaxPoints, LaplacianMethod)
        { }
        public override void Increment()
        {
            /*
            for (int jr = 0; jr < MaxPoints; jr++)
            {
                // diffusion of free ATM
                if (SiteParams.FRAP_On)
                    Ad_array[jr]
                      = Ad_T_array[jr]
                      - Pd_array[jr]
                      - AXd_array[jr]
                      - PXd_array[jr]
                      - Bd_array[jr];

                Al_array[jr]
                  = Al_T_array[jr]
                  - Pl_array[jr]
                  - AXl_array[jr]
                  - PXl_array[jr]
                  - Bl_array[jr];
            }*/

            TheLaplacianMethod.GetLaplacian(Ad_array, Ad_Lap_array, Al_array, Al_Lap_array);

            Parallel.ForEach(TheLaplacianMethod.LapPoints, (pr) =>
            {
                int jr;
                jr = pr.index;

                double Xm = Xm_array[jr];

                double AXl = AXl_array[jr];
                double PXl = PXl_array[jr];
                double Bl = Bl_array[jr];
                double A_Tl = Al_T_array[jr];

                double AXd = AXd_array[jr];
                double PXd = PXd_array[jr];
                double Bd = Bd_array[jr];
                double A_Td = Ad_T_array[jr];

                double X0_T = X0_T_array[jr];

                // calculate all derivatives first before updating concentrations
                double AXPX = AXl
                    + AXd
                    + PXl
                    + PXd;

                double Al = A_Tl - AXl - PXl - Bl;
                double X = X0_T - AXPX - Xm;
                //F_AX
                double dAXl_dt = SiteParams.k_on * Al * X - SiteParams.k_off * AXl
                    - SiteParams.k_3 * AXl + SiteParams.k_d * PXl;
                //F_PX
                double dPXl_dt = SiteParams.k_3 * AXl - SiteParams.k_d * PXl
                    - SiteParams.k_4 * PXl;
                //F_Xm
                double dXm_dt = SiteParams.k_4 * PXl + SiteParams.k_4 * PXd;
                
                //F_B
                X = X1_T_array[jr] - (Bl + Bd);
                double dBl_dt = SiteParams.k_1 * Al * X;

                // calculate dark quantitites even if not needed so that the
                // code will parallelize
                double Ad = A_Td - AXd - PXd - Bd;
                X = X0_T - AXPX - Xm;
                //F_AX
                double dAXd_dt = SiteParams.k_on * Ad * X - SiteParams.k_off * AXd
                    - SiteParams.k_3 * AXd + SiteParams.k_d * PXd;
                //F_PX
                double dPXd_dt = SiteParams.k_3 * AXd - SiteParams.k_d * PXd
                    - SiteParams.k_4 * PXd;
                
                //F_B
                X = X1_T_array[jr] - (Bl + Bd);
                double dBd_dt = SiteParams.k_1 * Ad * X;


                Xm_array[jr] += dt * dXm_dt;
                AXl_array[jr] += dt * dAXl_dt;
                PXl_array[jr] += dt * dPXl_dt;
                Bl_array[jr] += dt * dBl_dt;

                Al_T_array[jr] += dt * (DDD_A * Al_Lap_array[jr]);

                AXd_array[jr] += dt * dAXd_dt;
                PXd_array[jr] += dt * dPXd_dt;
                Bd_array[jr] += dt * dBd_dt;

                Ad_T_array[jr] += dt * (DDD_A * Ad_Lap_array[jr]);
            });
        }        
    }
}
