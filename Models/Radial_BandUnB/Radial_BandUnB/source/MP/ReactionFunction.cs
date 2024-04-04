using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial_BandUnB.MP_ATM_MDC
{
    public class ReactionFunction : IDisposable
    {
        public ReactionFunction() { }
        public virtual void Dispose() { }
        public virtual double F_AX(double Xm, double AX, double PX, double P, double B, double A_T, double X0_T)
        {
            return 0d;
        }
        public virtual double F_PX(double AX, double PX)
        {
            return 0d;
        }
        public virtual double F_Xm(double PX)
        {
            return 0d;
        }
        public virtual double F_P(double PX, double P)
        {
            return 0d;
        }
        public virtual double F_B(double AX, double PX, double P, double B, double A_T, double X1_T)
        {
            return 0d;
        }
        public virtual double F_H(double P, double PX, double H, double Y)
        {
            return 0d;
        }
        public virtual double F_Y(double P, double PX, double H, double M, double Y, double S)
        {
            return 0d;
        }
        public virtual double F_M(double M, double Y, double S)
        {
            return 0d;
        }
        public virtual double F_S(double M, double Y, double S)
        {
            return 0d;
        }
    }
    /// <summary>
    /// Reaction function with logistic map and explicit x dependence
    /// </summary>
    public class LogisticReactionFunction : ReactionFunction
    {
        private Storage.SiteParameters SiteParams;

        public LogisticReactionFunction(Storage.SiteParameters SiteParams)
        {
            this.SiteParams = SiteParams;
        }
        // reaction functions
        // note the following constraints:

        // A_T = A + P + [AX] + [PX] + B
        // X0_T = X0 + [AX] + [PX]
        // X1_T = X1 + B
        public override double F_AX(double Xm, double AX, double PX, double P, double B,
        double A_T, double X0_T)
        {
            // d_dt [AX] = k_on A X - k_off [AX] - k_3 [AX] + k_d [PX]

            double A = A_T - P - AX - PX - B;
            double X = X0_T - AX - PX - Xm;

            return SiteParams.k_on * A * X - SiteParams.k_off * AX
                - SiteParams.k_3 * AX + SiteParams.k_d * PX;
        }
        public override double F_PX(double AX, double PX)
        {
            // d_dt [PX] = k_3 [AX] - k_d [PX] - k_4 [PX]

            return SiteParams.k_3 * AX - SiteParams.k_d * PX
                - SiteParams.k_4 * PX;
        }
        public override double F_Xm(double PX)
        {
            // d_dt Xm = k_4 [PX]
            return SiteParams.k_4 * PX;
        }
        public override double F_P(double PX, double P)
        {
            // d_dt P = k_4 [PX] - k_d P [+ Lap(P)]
            return SiteParams.k_4 * PX - SiteParams.k_d * P;

        }
        public override double F_B(double AX, double PX, double P, double B,
        double A_T, double X1_T)
        {
            // d_dt B = k_1 * A X
            double A = A_T - P - AX - PX - B;
            double X = X1_T - B;

            return SiteParams.k_1 * A * X;
        }
        public override double F_H(double P, double PX, double H, double Y)
        {
            return -SiteParams.kfh * (P + PX) * H + SiteParams.krh * Y;

        }
        public override double F_Y(double P, double PX, double H, double M, double Y, double S)
        {
            return SiteParams.kfh * (P + PX) * H - SiteParams.krh * Y + SiteParams.k_off_MDC * M - SiteParams.k_on_MDC * Y * S;

        }
        public override double F_M(double M, double Y, double S)
        {
            return -SiteParams.k_off_MDC * M + SiteParams.k_on_MDC * Y * S;

        }
        public override double F_S(double M, double Y, double S)
        {
            return SiteParams.k_off_MDC * M - SiteParams.k_on_MDC * Y * S;

        }
    }
}
