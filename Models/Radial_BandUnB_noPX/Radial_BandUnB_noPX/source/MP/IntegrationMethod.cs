using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial_BandUnB_noPX.MP_ATM_MDC
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
        public double[] X1_T_array { get; set; }
        public double[] Xm_array { get; set; }
        public double[] B_array { get; set; }
        public double[] A_array { get; set; }
        public double[] AX_array { get; set; }
        public double[] PX_array { get; set; }
        public double[] P_array { get; set; }
        public double[] A_T_array { get; set; }
        public double[] X0_profile { get; set; }
        public double[] X1_profile { get; set; }
        public double[] A_Lap_array { get; set; }
        public double[] P_Lap_array { get; set; }
        public double[] H_array { get; set; }
        public double[] Y_array { get; set; }
        public double[] M_array { get; set; }
        public double[] S_array { get; set; }
        public double[] S_Lap_array { get; set; }

        protected ReactionFunction TheReactionFunction;
        protected LaplacianMethod TheLaplacianMethod;
        public IntegrationMethod(Storage.SiteParameters SiteParams, double dt,
            double dr, int MaxPoints, ReactionFunction ReactionFunction,
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
            this.B_array = new double[MaxPoints];
            this.A_array = new double[MaxPoints];
            this.AX_array = new double[MaxPoints];
            this.P_array = new double[MaxPoints];
            this.PX_array = new double[MaxPoints];
            this.A_T_array = new double[MaxPoints];
            this.X0_profile = new double[MaxPoints];
            this.X1_profile = new double[MaxPoints];
            this.A_Lap_array = new double[MaxPoints];
            this.P_Lap_array = new double[MaxPoints];

            this.H_array = new double[MaxPoints];
            this.Y_array = new double[MaxPoints];
            this.M_array = new double[MaxPoints];
            this.S_array = new double[MaxPoints];
            this.S_Lap_array = new double[MaxPoints];

            this.TheReactionFunction = ReactionFunction;
            this.TheLaplacianMethod = LaplacianMethod;
        }
        public void PrintArrays()
        {
            PrintArray(X0_T_array, "X0_T");
            PrintArray(X1_T_array, "X1_T");
            PrintArray(Xm_array, "Xm");
            PrintArray(B_array, "B");
            PrintArray(A_array, "A");
            PrintArray(AX_array, "AX");
            PrintArray(PX_array, "pr");
            PrintArray(P_array, "P");
            PrintArray(A_T_array, "A_T");
            PrintArray(X0_profile, "X0");
            PrintArray(X1_profile, "X1");
            PrintArray(A_Lap_array, "A_Lap");
            PrintArray(P_Lap_array, "P_Lap");

            PrintArray(this.H_array, "H");
            PrintArray(this.Y_array, "Y");
            PrintArray(this.M_array, "M");
            PrintArray(this.S_array, "S");
            PrintArray(this.S_Lap_array, "S_Lap");
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
            B_array = null;
            A_array = null;
            AX_array = null;
            P_array = null;
            PX_array = null;
            A_T_array = null;
            A_Lap_array = null;
            P_Lap_array = null;
            X0_profile = null;
            X1_profile = null;
            this.H_array = null;
            this.Y_array = null;
            this.M_array = null;
            this.S_array = null;
            this.S_Lap_array = null;

            TheReactionFunction.Dispose();
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
                this.AX_array[jr] = 0d;
                this.P_array[jr] = 0d;
                this.PX_array[jr] = 0d;
                this.B_array[jr] = 0d;
                this.A_T_array[jr] = SiteParams.A0;

                this.H_array[jr] = SiteParams.H0;
                this.Y_array[jr] = 0d;
                this.M_array[jr] = 0d;
                this.S_array[jr] = SiteParams.S0;
                this.S_Lap_array[jr] = 0d;
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
            }
        }
        public virtual void Increment() { }
        public double GetUnboundATM(int jr)
        {
            return A_T_array[jr]
                - AX_array[jr] - PX_array[jr] - B_array[jr];
        }

        public double GetBoundATM(int jr)
        {
            return AX_array[jr]
                + PX_array[jr] + B_array[jr];
        }
        public double GetUnboundMDC(int jr)
        {
            return S_array[jr];
        }
        public double GetBoundMDC(int jr)
        {
            return M_array[jr];
        }

        public double GetXm(int jr) { return Xm_array[jr]; }

        public double GetB(int jr) { return B_array[jr]; }

        public double GetX0(int jr) { return X0_T_array[jr]; }

        public double GetX1(int jr) { return X1_T_array[jr]; }

    }
    public class EulerIntegrationMethod : IntegrationMethod
    {
        public EulerIntegrationMethod(Storage.SiteParameters SiteParams, double dt,
            double dr, int MaxPoints, ReactionFunction ReactionFunction,
            LaplacianMethod LaplacianMethod)
            : base(SiteParams, dt, dr,
                  MaxPoints, ReactionFunction, LaplacianMethod)
        { }
        public override void Increment()
        {
            for (int jr = 0; jr < MaxPoints; jr++)
            {
                // diffusion of free ATM

                A_array[jr]
                  = A_T_array[jr]
                  - P_array[jr]
                  - AX_array[jr]
                  - PX_array[jr]
                  - B_array[jr];
            }

            TheLaplacianMethod.GetLaplacian(A_array, A_Lap_array, P_array, P_Lap_array, S_array, S_Lap_array);

            Parallel.ForEach(TheLaplacianMethod.LapPoints, (pr) =>
            {
                int jr;
                jr = pr.index;

                // calculate all derivatives first before updating concentrations

                double dAX_dt = TheReactionFunction.F_AX(
                    Xm_array[jr],
                    AX_array[jr],
                    PX_array[jr],
                    P_array[jr],
                    B_array[jr],
                    A_T_array[jr],
                    X0_T_array[jr]);

                // double dpr_dt = TheReactionFunction.F_PX(
                //    AX_array[jr],
                //    PX_array[jr]);

                double dXm_dt = TheReactionFunction.F_Xm(AX_array[jr]);

                double dP_dt = TheReactionFunction.F_P(
                    AX_array[jr],
                    P_array[jr])
                  + DDD_P * P_Lap_array[jr];

                double dB_dt = TheReactionFunction.F_B(AX_array[jr],
                                      PX_array[jr],
                                      P_array[jr],
                                      B_array[jr],
                                      A_T_array[jr],
                                      X1_T_array[jr]);

                double dH_dt = TheReactionFunction.F_H(P_array[jr], PX_array[jr], H_array[jr], Y_array[jr]);
                double dY_dt = TheReactionFunction.F_Y(P_array[jr], PX_array[jr], H_array[jr], M_array[jr], Y_array[jr], S_array[jr]);
                double dM_dt = TheReactionFunction.F_M(M_array[jr], Y_array[jr], S_array[jr]);
                double dS_dt = TheReactionFunction.F_S(M_array[jr], Y_array[jr], S_array[jr]) + DDD_M * S_Lap_array[jr];
                
                Xm_array[jr] += dt * dXm_dt;
                AX_array[jr] += dt * dAX_dt;
                // PX_array[jr] += dt * dpr_dt;
                P_array[jr] += dt * dP_dt;
                B_array[jr] += dt * dB_dt;

                H_array[jr] += dt * dH_dt;
                Y_array[jr] += dt * dY_dt;
                M_array[jr] += dt * dM_dt;
                S_array[jr] += dt * dS_dt;

                A_T_array[jr] += dt * (DDD_A * A_Lap_array[jr]
                               + DDD_P * P_Lap_array[jr]);

            });
        }
    }
}
