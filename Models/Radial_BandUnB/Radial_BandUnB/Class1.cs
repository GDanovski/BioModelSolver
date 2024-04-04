using ODEModel;
using Radial_BandUnB.source.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial_BandUnB
{
    public class Main
    {
        /// <summary>
        /// Returns the mathematical model
        /// </summary>
        /// <returns></returns>
        public Model GetModel()
        {
            return new MyModel();
        }
    }
    /// <summary>
    /// Model for solving the equation of a Line
    /// </summary>
    public class MyModel : Model
    {
        private string info = "";
        Dictionary<string, double[][]> images = null;
        M_Profile_Form m_Profile_Form = new M_Profile_Form();
        int width = 0;
        int height = 0;
        /// <summary>
        /// Returns the name of the model as string
        /// </summary>
        /// <returns></returns>
        public override string GetName()
        {
            return "Standard aATM diffusion model";

        }
        /// <summary>
        /// Returns the mathematical model as string
        /// </summary>
        /// <returns></returns>
        public override string GetModel()
        {
            return string.Join("\n", new string[] {
                "Standard αATM diffusion model",
                "",
                "C# radial version of the model 1.0.1.0",
                info,
                "ShowImg - 1 for image array calculation; 0 to disable image calculation"
            });
        }
        /// <summary>
        /// Open video form
        /// </summary>
        /// <returns>True if video is available</returns>
        public override bool GetVideo()
        {
            if (images != null && System.Windows.Forms.MessageBox.Show("Do you want to calculate output images?",
               "Result images", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                var modelCell = new ModelCellTool(this.width, this.height);
                modelCell.SetData = images;
                //Console.WriteLine(images.Count);
                modelCell.LoadGUI();
                return true;
            }
            
            return false;
        }
        /// <summary>
        /// Open Profile form
        /// </summary>
        /// <returns>True if video is available</returns>
        public override bool GetProfile()
        {
            m_Profile_Form.LoadGUI();
            RefreshGUI();
            return true;
        }
        /// <summary>
        /// Declaring the variables for the model
        /// </summary>
        /// <param name="data">Input data set</param>
        public override void AddVariables(Data data)
        {
            #region My variables
            //Declare variables here

            data.AddVariable("t1", 1, 1, 1);
            data.AddVariable("p", 1.8, 1.8, 1.8);
            //DMGOnlyInS
            data.AddVariable("DMGOnlyInS", 0, 0, 0);
            //spatial grid parameters
            data.AddVariable("dr", 0.18463, 0.18463, 0.18463); // (micron)
            data.AddVariable("nr", 50, 50, 50);
            data.AddVariable("ndist_free", 30, 30, 30);
            data.AddVariable("nrad_free", 5, 5, 5);
            // time integration parameters
            data.AddVariable("dt", 0.01, 0.01, 0.01);
            data.AddVariable("ntimes", 185000, 185000, 185000);
            data.AddVariable("tmodulo", 500, 500, 500);
            //FRAP ATM
            data.AddVariable("F_dt", 0.01, 0.01, 0.01, "dt_FRAP_ATM");
            data.AddVariable("F_ntimes", 60000, 60000, 60000, "ntimes_FRAP_ATM");
            data.AddVariable("F_tmodulo", 15, 15, 15, "tmodulo_FRAP_ATM");
            data.AddVariable("t_FRAP", 200, 200, 200, "t_FRAP_ATM");
            //FRAP MDC
            data.AddVariable("FM_dt", 0.01, 0.01, 0.01, "dt_FRAP_MDC1");
            data.AddVariable("FM_ntimes", 100000, 100000, 100000, "ntimes_FRAP_MDC1");
            data.AddVariable("FM_tmodulo", 100, 100, 100, "tmodulo_FRAP_MDC1");
            data.AddVariable("tM_FRAP", 500, 500, 500, "t_FRAP_MDC1");

            data.AddVariable("R_F_MDC", 1.25, 1.25, 1.25, "R_FRAP_MDC1");
            data.AddVariable("R_F_ATM", 1.25, 1.25, 1.25, "R_FRAP_ATM");

            data.AddVariable("laplacian", 0, 0, 0);
            data.AddVariable("integration", 0, 0, 0);

            data.AddVariable("DDD_P", 0.0708900616131959, 0.9, 0.01, "Dp");
            data.AddVariable("DDD_A", 0.663615137, 0.9, 0.01, "Da");
            data.AddVariable("DDD_M", 0.0607646544239966, 0.9, 0.001, "Dm");
            //Site parameters
            data.AddVariable("k_0", 0.0116827165988261, 1, 0.000001);// (/micro M sec)
            // data.AddVariable("k_1", 0.58, 1, 0.000001);// (/micro M sec)
            data.AddVariable("k_on", 8.15213303835297, 1, 0.000001); // (/sec)
            data.AddVariable("k_off", 0.238365032115081, 1, 0.000001);// (/micro M sec)
            data.AddVariable("k_3", 0.00231573252087517, 1, 0.000001);// (/micro M sec)
            data.AddVariable("k_d", 0.365020409304909, 1, 0.000001);// (/micro M sec)
            data.AddVariable("k_4", 0.0012373390697014, 1, 0.000001);// (/micro M sec)
            data.AddVariable("kfh", 21.1832671956066, 1, 0.000001);// (/micro M sec)
            data.AddVariable("krh", 0.0, 1, 0.000001);// (/micro M sec)
            data.AddVariable("k_on_MDC", 0.822838999160859, 1, 0.000001);// (/micro M sec)
            data.AddVariable("k_off_MDC", 3.06084808652938, 1, 0.000001);// (/micro M sec)

            data.AddVariable("A0", 0.165023686, 1, 0.0001, "fATM"); // (micro M)
            data.AddVariable("H0", 20.345724013612, 1, 0.0001, "H2AX"); // (micro M)
            data.AddVariable("S0", 0.302878657, 1, 0.0001, "fMDC1"); // (micro M)
            data.AddVariable("X1", 0.127089799059051, 1, 0.0001);// (micro M)
            data.AddVariable("X0", 1.16445504708788, 1, 0.0001); // (micro M)
            data.AddVariable("R1", 0.8, 0.8, 0.8);
            data.AddVariable("S1", 0.9, 0.9, 0.9);
            data.AddVariable("S1_MDC", 0.9, 0.9, 0.9);

            data.AddVariable("ShowImg", 1, 1, 1);
            #endregion My variables
        }
        /// <summary>
        /// Declare the matematical function and calculate the fit values
        /// </summary>
        /// <param name="data">Input data set</param>
        public override void Calculations(Data data)
        {
            var startTime = DateTime.Now;

            double DDD_P = data.GetVariable("DDD_P").Value;
            double DDD_A = data.GetVariable("DDD_A").Value;
            double DDD_M = data.GetVariable("DDD_M").Value;
            double dr = data.GetVariable("dr").Value;
            double dt = data.GetVariable("dt").Value;

            double DDD_max = Math.Max(Math.Max(DDD_A, DDD_P), DDD_M);

            this.info = "Figure of Merit for diffusion:\n2 DDD dt / dx^2 (<< 1): " +
                (2 * DDD_max * dt / (dr * dr)).ToString();

            int ShowImg = (int)data.GetVariable("ShowImg").Value;
            //Prepare images variable
            if (images != null) images.Clear();

            if (ShowImg == 0)
                images = null;
            else
                images = new Dictionary<string, double[][]>();

            // Prepare solvers
            SolverInterface MP_ATM_MDC_solver = new MP_ATM_MDC.DiffusionSolver(data);
            SolverInterface FRAP_MDC_solver = new FRAP_MDC.DiffusionSolver(data);
            SolverInterface FRAP_ATM_solver = new FRAP_ATM.DiffusionSolver(data);

            var solvers = new SolverInterface[] {
                MP_ATM_MDC_solver,
                FRAP_MDC_solver,
                FRAP_ATM_solver
            };

            Parallel.ForEach(solvers, sol =>
            {
                sol.Start(false, false, false, false);
            });

            //solve the model for a range of values

            data.fitYvalues = new double[][] {
                DuplicateArray(MP_ATM_MDC_solver.Results[0]), // ATM bound
                DuplicateArray(MP_ATM_MDC_solver.Results[1]), // ATM free
                DuplicateArray(FRAP_ATM_solver.Results[0]), // ATM FRAP
                //DuplicateArray(FRAP_ATM_solver.Results[1]), // ATM FRAP foci all
                //DuplicateArray(FRAP_ATM_solver.Results[2]), // ATM FRAP unbound all
                DuplicateArray(MP_ATM_MDC_solver.Results[2]), // MDC1 bound
                DuplicateArray(MP_ATM_MDC_solver.Results[3]), // MDC1 free
                DuplicateArray(FRAP_MDC_solver.Results[0]) // MDC FRAP
                //DuplicateArray(FRAP_MDC_solver.Results[1]), // MDC1 FRAP foci all
                //DuplicateArray(FRAP_MDC_solver.Results[2]), // MDC1 FRAP unbound all
                
            };

            DuplicateImages((MP_ATM_MDC.DiffusionSolver)MP_ATM_MDC_solver);
            DuplicateImages((FRAP_MDC.DiffusionSolver)FRAP_MDC_solver);
            DuplicateImages((FRAP_ATM.DiffusionSolver)FRAP_ATM_solver);

            var Time = DateTime.Now.Subtract(startTime);
            Console.WriteLine("Finished in " + Time.ToString());

            MP_ATM_MDC_solver.Dispose();
        }
        private void DuplicateImages(MP_ATM_MDC.DiffusionSolver sol)
        {
            m_Profile_Form.M_profile_fit = DuplicateArray(sol.M_profile);
            m_Profile_Form.dmg_profile = DuplicateArray(sol.dmg_profile);
            m_Profile_Form.AX_PX_profile = DuplicateArray(sol.AX_PX_profile);
            m_Profile_Form.AX_profile = DuplicateArray(sol.AX_profile);
            m_Profile_Form.PX_profile = DuplicateArray(sol.PX_profile);
            m_Profile_Form.X0_T_profile = DuplicateArray(sol.X0_T_profile);
            m_Profile_Form.P_profile = DuplicateArray(sol.P_profile);
            m_Profile_Form.A_P_AX_PX_profile = DuplicateArray(sol.A_P_AX_PX_profile);

            if (images != null)
            {
                this.width = m_Profile_Form.M_profile_fit[0].Length + 1;
                this.height = this.width;

                this.images.Add("M", DuplicateArray(m_Profile_Form.M_profile_fit));
                this.images.Add("dmg", DuplicateArray(m_Profile_Form.dmg_profile));
                this.images.Add("AX_PX", DuplicateArray(m_Profile_Form.AX_PX_profile));
                this.images.Add("AX", DuplicateArray(m_Profile_Form.AX_profile));
                this.images.Add("PX", DuplicateArray(m_Profile_Form.PX_profile));
                this.images.Add("X0_T", DuplicateArray(m_Profile_Form.X0_T_profile));
                this.images.Add("P", DuplicateArray(m_Profile_Form.P_profile));
                this.images.Add("A_P_AX_PX", DuplicateArray(m_Profile_Form.A_P_AX_PX_profile));
            }
        }
        private void DuplicateImages(FRAP_MDC.DiffusionSolver sol)
        {

            int start = (int)(sol.SiteParams.t_FRAP / (sol.dt * sol.tmodulo));
            double[][] temp = DuplicateArray(sol.M_profile);

            m_Profile_Form.M_FRAP_profile_fit = new double[m_Profile_Form.M_profile_fit.Length][];
            for (int i = 0, j = start; i < m_Profile_Form.M_FRAP_profile_fit.Length; i++, j++)
            {
                if (j > temp.Length || j < 0)
                {
                    m_Profile_Form.M_FRAP_profile_fit[i] = new double[m_Profile_Form.M_profile_fit[0].Length];
                }
                else
                {
                    m_Profile_Form.M_FRAP_profile_fit[i] = temp[j];
                }
            }

            temp = null;

            if (images != null)
            {
                this.images.Add("M_FRAP", DuplicateArray(sol.M_profile));
            }
        }
        private void DuplicateImages(FRAP_ATM.DiffusionSolver sol)
        {
            int start = (int)(sol.SiteParams.t_FRAP / (sol.dt * sol.tmodulo));
            double[][] temp = DuplicateArray(sol.ATM_profile);

            m_Profile_Form.ATM_FRAP_profile_fit = new double[m_Profile_Form.M_profile_fit.Length][];
            for (int i = 0, j = start; i < m_Profile_Form.ATM_FRAP_profile_fit.Length; i++, j++)
            {
                if (j > temp.Length || j < 0)
                {
                    m_Profile_Form.ATM_FRAP_profile_fit[i] = new double[m_Profile_Form.M_profile_fit[0].Length];
                }
                else
                {
                    m_Profile_Form.ATM_FRAP_profile_fit[i] = temp[j];
                }
            }

            temp = null;

            if (images != null)
            {
                this.images.Add("ATM_FRAP", DuplicateArray(sol.ATM_profile));
            }
        }
        public override void RefreshGUI()
        {
            try
            {
                m_Profile_Form.Refresh();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public override double GetStDev()
        {
            return m_Profile_Form.getStDev();
        }
        private double[] DuplicateArray(double[] input)
        {
            double[] output = new double[input.Length];
            Array.Copy(input, output, input.Length);
            return output;
        }
        private double[][] DuplicateArray(double[][] input)
        {
            double[][] output = new double[input.Length][];

            for (int i = 0; i < input.Length; i++)
                output[i] = DuplicateArray(input[i]);

            return output;
        }
    }
}
