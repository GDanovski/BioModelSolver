using ODEModel;
using Radial_BandUnB_noPX.source.common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial_BandUnB_noPX.FRAP_MDC
{
    public class DiffusionSolver : SolverInterface
    {
        public string run_comment { get; set; }
        public double t { get; set; }
        public double dt { get; set; }
        public int laplacianchoice { get; set; }
        public int MaxPoints { get; set; }
        public double dr { get; set; }
        public double ns { get; set; }
        public double nq { get; set; }
        public int ntimes { get; set; }
        public int tmodulo { get; set; }
        public Storage.SiteParameters SiteParams { get; set; }
        public LaplacianMethod TheLaplacianMethod { get; set; }
        public IntegrationMethod TheIntegrationMethod { get; set; }
        public ReactionFunction TheReactionFunction { get; set; }
        public double[][] M_profile { get; set; }

        private const double M_PI = 3.141592653589793238;
        private int storeProfileIndex = 0;

        public DiffusionSolver(Data data = null)
        {
            ModelData = data;
            this.SiteParams = new Storage.SiteParameters();
            this.dr = 0;
            this.MaxPoints = 0;

            if (data == null) return;

            #region My variables
            //Declare variables here

            this.SiteParams.DDD_P = data.GetVariable("DDD_P").Value;
            this.SiteParams.DDD_A = data.GetVariable("DDD_A").Value;
            this.SiteParams.DDD_M = data.GetVariable("DDD_M").Value;
            //Site parameters
            this.SiteParams.k_0 = data.GetVariable("k_0").Value;// (/micro M sec)
            this.SiteParams.k_1 = data.GetVariable("k_on").Value;// (/micro M sec)
            this.SiteParams.k_on = data.GetVariable("k_on").Value; // (/sec)
            this.SiteParams.k_off = data.GetVariable("k_off").Value;// (/micro M sec)
            this.SiteParams.k_3 = data.GetVariable("k_3").Value;// (/micro M sec)
            this.SiteParams.k_d = data.GetVariable("k_d").Value;// (/micro M sec)
            this.SiteParams.k_4 = data.GetVariable("k_4").Value;// (/micro M sec)
            //this.SiteParams.k_5 = data.GetVariable("k_5").Value;// (/micro M sec)
            this.SiteParams.kfh = data.GetVariable("kfh").Value;// (/micro M sec)
            this.SiteParams.krh = data.GetVariable("krh").Value;// (/micro M sec)
            this.SiteParams.k_on_MDC = data.GetVariable("k_on_MDC").Value;// (/micro M sec)
            this.SiteParams.k_off_MDC = data.GetVariable("k_off_MDC").Value;// (/micro M sec)

            this.SiteParams.X0 = data.GetVariable("X0").Value; // (micro M)
            this.SiteParams.A0 = data.GetVariable("A0").Value; // (micro M)
            this.SiteParams.H0 = data.GetVariable("H0").Value; // (micro M)
            this.SiteParams.S0 = data.GetVariable("S0").Value; // (micro M)
            this.SiteParams.X1 = data.GetVariable("X1").Value;// (micro M)
            //this.SiteParams.X0 = data.GetVariable("X0_Ku").Value; // (micro M)
            //this.SiteParams.A0 = data.GetVariable("A0_Ku").Value; // (micro M)
            //this.SiteParams.X1 = data.GetVariable("X1_Ku").Value;// (micro M)
            this.SiteParams.R1 = data.GetVariable("R1").Value;
            this.SiteParams.t0 = data.GetVariable("t1").Value;
            this.SiteParams.S1 = data.GetVariable("S1").Value;
            this.SiteParams.S1_MDC = data.GetVariable("S1_MDC").Value;
            this.SiteParams.p = data.GetVariable("p").Value;
            //DMGOnlyInS
            this.SiteParams.DMGOnlyInS = (int)data.GetVariable("DMGOnlyInS").Value;
            //spatial grid parameters
            this.dr = data.GetVariable("dr").Value; // (micron)
            this.MaxPoints = (int)data.GetVariable("nr").Value;
            this.ns = (int)data.GetVariable("ndist_free").Value;
            this.nq = (int)data.GetVariable("nrad_free").Value;
            // time integration parameters
            //this.dt = data.GetVariable("dt").Value;
            //this.ntimes = (int)data.GetVariable("ntimes").Value;
            //this.tmodulo = (int)data.GetVariable("tmodulo").Value;
            //FRAP ATM
            //this.dt = data.GetVariable("F_dt").Value;
            //this.ntimes = (int)data.GetVariable("F_ntimes").Value;
            //this.tmodulo = (int)data.GetVariable("F_tmodulo").Value;
            //this.SiteParams.t_FRAP = data.GetVariable("t_FRAP").Value;
            //this.SiteParams.R_F = data.GetVariable("R_F_ATM").Value;
            //FRAP MDC
            this.dt = data.GetVariable("FM_dt").Value;
            this.ntimes = (int)data.GetVariable("FM_ntimes").Value;
            this.tmodulo = (int)data.GetVariable("FM_tmodulo").Value;
            this.SiteParams.t_FRAP = data.GetVariable("tM_FRAP").Value;
            this.SiteParams.R_F = data.GetVariable("R_F_MDC").Value;

            this.laplacianchoice = (int)data.GetVariable("laplacian").Value;
            //this.integrationchoice = data.GetVariable("integration").Value;

            //this.SiteParams. = data.GetVariable("ShowImg").Value;
            #endregion My variables
        }
        private void InitProfile(int length)
        {
            storeProfileIndex = 0;
            M_profile = new double[length][];
        }
        private void CopyProfile()
        {
            if (storeProfileIndex >= M_profile.Length) return;

            int k = storeProfileIndex;
            int length = MaxPoints * 2;
            var M_profile_out = new double[length];

            for (int i = 0, j = length - 1, x = MaxPoints - 1; i < MaxPoints; i++, j--, x--)
            {
                M_profile_out[i] = TheIntegrationMethod.Ml_array[x] + TheIntegrationMethod.Sl_array[x];

                M_profile_out[j] = TheIntegrationMethod.Ml_array[x] + TheIntegrationMethod.Sl_array[x];
            }

            M_profile[k] = M_profile_out;

            storeProfileIndex++;
        }
        public override void Dispose()
        {
            if (TheLaplacianMethod != null) TheLaplacianMethod.Dispose();
            if (TheIntegrationMethod != null) TheIntegrationMethod.Dispose();
            if (TheReactionFunction != null) TheReactionFunction.Dispose();
        }
        public override double[][] Start(bool printProfile, bool printResults, bool reportProgres, bool reportProgressForm, bool GetImages = false)
        {
            var startTime = DateTime.Now;

            TheReactionFunction = new LogisticReactionFunction(SiteParams);

            double A_cell = M_PI * Math.Pow(dr, 2);
            Console.WriteLine("nucleus area:\t" + A_cell);

            double A_site = M_PI * Math.Pow(SiteParams.S1, 2);
            Console.WriteLine("focus area:\t" + A_site);

            if (laplacianchoice == 1)
            {
                Console.WriteLine("using 4th order Laplacian");
                TheLaplacianMethod = new LaplacianMethod4(dr, MaxPoints);
            }
            else
            {
                Console.WriteLine("using 2th order Laplacian");
                TheLaplacianMethod = new LaplacianMethod2(dr, MaxPoints);
            }

            Console.WriteLine("using Euler integration");
            TheIntegrationMethod = new EulerIntegrationMethod(SiteParams,
                                      dt, dr, MaxPoints,
                                      TheReactionFunction,
                                      TheLaplacianMethod);

            Console.WriteLine("plot t interval: " + (tmodulo * dt).ToString());

            double DDD_max = Math.Max(SiteParams.DDD_A, SiteParams.DDD_P);
            DDD_max = Math.Max(DDD_max, SiteParams.DDD_M);
            Console.WriteLine("Figure of Merit for diffusion:\n2 DDD dt / dx^2 (<< 1): "
                + (2 * DDD_max * dt / (dr * dr)).ToString());

            // initial value
            TheIntegrationMethod.Initialize();

            // time dependence loop
            t = 0;
            double BoundATM_sum;
            double UnboundATM_sum;
            double BoundMDC_sum;
            double UnboundMDC_sum;
            // code check variables

            double Xm_Residual;
            double B_Residual;

            double Rmax = dr * (MaxPoints - 1);
            double AreaFull = M_PI * Math.Pow(Rmax, 2);
            double Area = M_PI * Math.Pow(SiteParams.S1, 2);
            double AreaMDC = M_PI * Math.Pow(SiteParams.S1_MDC, 2);

            Console.WriteLine("Processing...");

            ProgressBar progressBar1 = reportProgressForm ? new ProgressBar() : null;
            if (reportProgressForm)
            {
                progressBar1.Show();
                progressBar1.BringToFront();

                progressBar1.SetProgress(0, ntimes);
            }
            else if (reportProgres) ProgressBar(0, ntimes);
            int jk = 0;

            if ((printProfile || printResults) & !Directory.Exists("results")) Directory.CreateDirectory("results");

            StreamWriter BoundATM_stream = null;
            StreamWriter UnboundATM_stream = null;
            StreamWriter ATM_sumstream = printResults ? new StreamWriter(@"results\ATM12sums.dat") : null;
            List<double> Y1 = new List<double>();
            List<double> Y2 = new List<double>();
            List<double> Y3 = new List<double>();
            List<double> Y4 = new List<double>();
            List<double> Y5 = new List<double>();
            List<double> Y6 = new List<double>();
            InitProfile((int)(ntimes / tmodulo));

            double[] UnboundATM_arr = new double[MaxPoints];
            double[] UnboundMDC_arr = new double[MaxPoints];

            bool FRAP_On = false;

            for (int k = 0; k <= ntimes; k++)
            {
                TheIntegrationMethod.ManageSites(t);
                // update the plotting file if modulo is satisfied

                if ((k % tmodulo) == 0)
                {
                    //ManageFRAP
                    if (!FRAP_On && t >= SiteParams.t_FRAP)
                    {
                        FRAP_On = true;
                        TheIntegrationMethod.ManageFrapSites();
                    }

                    BoundATM_sum = 0;
                    UnboundATM_sum = 0;
                    BoundMDC_sum = 0;
                    UnboundMDC_sum = 0;

                    Xm_Residual = 0;
                    B_Residual = 0;

                    if (printProfile)
                    {
                        BoundATM_stream = new StreamWriter(@"results\BoundATM" + ZeroPadNumber((int)(k / tmodulo), 3) + ".dat");
                        UnboundATM_stream = new StreamWriter(@"results\UnboundATM" + ZeroPadNumber((int)(k / tmodulo), 3) + ".dat");
                    }


                    for (int jr = 0; jr < MaxPoints; jr++)
                    {
                        double R = dr * jr;
                        if (R <= SiteParams.S1)
                        {
                            BoundATM_sum += 2 * M_PI * R * dr
                                * TheIntegrationMethod.GetBoundATM(jr);

                            Xm_Residual += 2 * M_PI * R * dr
                                * (TheIntegrationMethod.GetX0(jr)
                                    - TheIntegrationMethod.GetXm(jr));
                            B_Residual += 2 * M_PI * R * dr
                                * (TheIntegrationMethod.GetX1(jr)
                                    - TheIntegrationMethod.GetB(jr));
                        }
                        if (R <= SiteParams.S1_MDC)
                        {
                            BoundMDC_sum += 2 * M_PI * R * dr
                                * (TheIntegrationMethod.GetBoundMDC(jr) + TheIntegrationMethod.GetUnboundMDC(jr));
                        }

                        UnboundATM_arr[jr] = TheIntegrationMethod.GetUnboundATM(jr);
                        UnboundMDC_arr[jr] = TheIntegrationMethod.GetUnboundMDC(jr);
                    }
                    UnboundATM_sum = RoiMeasure.measureOvalRoi(this.ns, this.nq, this.dr, UnboundATM_arr);
                    UnboundMDC_sum = RoiMeasure.measureOvalRoi(this.ns, this.nq, this.dr, UnboundMDC_arr);

                    if (BoundATM_stream != null) BoundATM_stream.Close();
                    if (UnboundATM_stream != null) UnboundATM_stream.Close();

                    //if (k == 0)
                    //MSumNorm = Mfree_sum;
                    if (printResults)
                        ATM_sumstream.WriteLine(string.Join("\t", new double[] {
                        t, BoundATM_sum / Area, UnboundATM_sum,
                        BoundMDC_sum / AreaMDC, UnboundMDC_sum,
                        Xm_Residual / (Area * SiteParams.X0),
                        B_Residual / (Area * SiteParams.X1)}));

                    Y1.Add(BoundATM_sum / Area);
                    Y2.Add(UnboundATM_sum);
                    Y3.Add(BoundMDC_sum / AreaMDC);
                    Y4.Add(UnboundMDC_sum);
                    Y5.Add(Xm_Residual / (Area * SiteParams.X0));
                    Y6.Add(B_Residual / (Area * SiteParams.X1));

                    CopyProfile();
                }

                TheIntegrationMethod.Increment();
                t += dt;
                //report progress
                if (reportProgressForm && k == jk)
                {
                    progressBar1.SetProgress(k, ntimes);
                    jk += tmodulo;

                    if (progressBar1.Exit)
                    {
                        TheIntegrationMethod.Dispose();
                        TheLaplacianMethod.Dispose();
                        TheReactionFunction.Dispose();

                        progressBar1.Hide();
                        progressBar1.Dispose();
                        progressBar1 = null;
                        Console.WriteLine("Interupted!");
                        return new double[][] {Y1.ToArray(), Y2.ToArray(), Y3.ToArray(), Y4.ToArray()
                        , Y5.ToArray(), Y6.ToArray()};
                    }
                }
                else if (reportProgres && k == jk)
                {
                    ProgressBar(k, ntimes);
                    jk += tmodulo;
                }

                //if (k == tmodulo + 1) break;
            }
            //TheIntegrationMethod.PrintArrays();

            if (ATM_sumstream != null) ATM_sumstream.Close();

            TheIntegrationMethod.Dispose();
            TheLaplacianMethod.Dispose();
            TheReactionFunction.Dispose();

            if (reportProgres) ProgressBar(ntimes, ntimes);

            var Time = DateTime.Now.Subtract(startTime);

            Console.WriteLine("Done!");
            Console.WriteLine("Finished in " + Time.ToString());

            if (reportProgressForm)
            {
                progressBar1.Hide();
                progressBar1.Dispose();
                progressBar1 = null;
            }
            this.Results = new double[][] { Y1.ToArray(), Y2.ToArray(), Y3.ToArray(), Y4.ToArray(), Y5.ToArray(), Y6.ToArray() };

            // Prepare FRAP results
            {
                var result = this.Results;
                const int M = 2;//Bound in F1
                const int S = 3;//Free

                int start = (int)(SiteParams.t_FRAP / (dt * tmodulo)) + 1;
                if (start > result[M].Length) start = result[M].Length;
                int length = result[M].Length - start;
                if (length < 0) length = 0;

                double[] bound = new double[length];
                Array.Copy(result[M], start, bound, 0, length);

                double[] bound_original = new double[length];
                double[] unbound_original = new double[length];

                for (int i = 0, ind = 0; i < length && ind < result[M].Length; i++, ind += (int)((result[M].Length - result[M].Length % ModelData.Xvalues.Length) / ModelData.Xvalues.Length))
                {
                    bound_original[i] = result[M][ind];
                    unbound_original[i] = result[S][ind];
                }
                this.Results = new double[][] { bound, bound_original, unbound_original };
            }
            return this.Results;
        }
        protected static void ProgressBar(int progress, int tot)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / tot;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + tot.ToString() + "    "); //blanks at the end remove any excess
        }
        protected string ZeroPadNumber(int num, int width)
        {
            string output = num.ToString();
            for (int i = output.Length; i < width; i++)
                output = "0" + output;
            return output;
        }
        public void ReadValuesFromFile(string dir = "ConfigATMC1.dat")
        {
            string[] lines = null;
            //check if the config file exists
            if (!File.Exists(dir))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Missing config file!");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            //read the config file
            try
            {
                lines = File.ReadAllLines(dir);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The config file is unavaliable!");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.WriteLine("Input parameters:");
            //read the variables from the config file
            foreach (string str in lines)
                GetVariable(str);

            lines = null;
        }
        private void GetVariable(string input)
        {
            string str = input;
            if (str == "" || !str.Contains(":")) return;

            int end = str.IndexOf(@"//");

            if (end > 0) str = str.Substring(0, end);

            str = str.Replace(" ", "").Replace("\t", "");

            string[] vals = str.Split(new string[] { ":" }, StringSplitOptions.None);
            if (vals.Length < 2) return;

            double value = 0d;
            switch (vals[0])
            {
                case "run_comment":
                    this.run_comment = vals[0];
                    Console.WriteLine(input);
                    break;
                case "DDD_P":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.DDD_P = value;
                    break;
                case "DDD_M":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.DDD_M = value;
                    break;
                case "DDD_A":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.DDD_A = value;
                    break;
                case "k_0":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.k_0 = value;
                    break;
                case "k_1":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.k_1 = value;
                    break;
                case "k_on":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.k_on = value;
                    break;
                case "k_off":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.k_off = value;
                    break;
                case "k_on_MDC":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.k_on_MDC = value;
                    break;
                case "k_off_MDC":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.k_off_MDC = value;
                    break;
                case "k_3":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.k_3 = value;
                    break;
                case "kfh":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.kfh = value;
                    break;
                case "krh":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.krh = value;
                    break;
                case "k_d":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.k_d = value;
                    break;
                case "k_4":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.k_4 = value;
                    break;
                case "p":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.p = value;
                    break;
                case "X0":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.X0 = value;
                    break;
                case "A0":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.A0 = value;
                    break;
                case "R0":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.R1 = value;
                    break;
                case "S0":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.S0 = value;
                    break;
                case "S":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.S1 = value;
                    break;
                case "H0":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.H0 = value;
                    break;
                case "X1":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.X1 = value;
                    break;
                case "t0":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.t0 = value;
                    break;
                case "dr":
                    StringToValue(vals[0], vals[1], out value);
                    this.dr = value;
                    break;
                case "dt":
                    StringToValue(vals[0], vals[1], out value);
                    this.dt = value;
                    break;
                case "laplacian":
                    StringToValue(vals[0], vals[1], out value);
                    this.laplacianchoice = (int)value;
                    break;
                case "integration":
                    StringToValue(vals[0], vals[1], out value);

                    break;
                case "ntimes":
                    StringToValue(vals[0], vals[1], out value);
                    this.ntimes = (int)value;
                    break;
                case "tmodulo":
                    StringToValue(vals[0], vals[1], out value);
                    this.tmodulo = (int)value;
                    break;
                case "MaxPoints":
                    StringToValue(vals[0], vals[1], out value);
                    this.MaxPoints = (int)value;
                    break;
                case "DMGOnlyInS":
                    StringToValue(vals[0], vals[1], out value);
                    this.SiteParams.DMGOnlyInS = (int)value;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error setting value -> " + str);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
        private bool StringToValue(string key, string input, out double output)
        {
            bool success = double.TryParse(input, out output);

            if (!success)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Value must be double: " + key);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine(key + ":\t" + output.ToString());
            }

            return success;
        }
    }
}
