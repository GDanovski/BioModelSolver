using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ODESolver
{
    class FileEncoder
    {
        public static void ReadFile(string dir, out ODEModel.Data data)
        {
            if (!File.Exists(dir)) throw null;
            //Declare variables
            string XTitle = "";
            string YTitle = "";
            List<double> XVals = new List<double>();
            List<List<double>> YVals = new List<List<double>>();
            string str;
            string[] vals = null;
            double[] microYvals;
            //read the file
            using (StreamReader sr = new StreamReader(dir))
            {
                str = sr.ReadLine();//Read Axis Titles

                if (str.Contains("\t"))//Reads TAB delimited files
                {
                    vals = str.Split(new string[] { "\t" }, StringSplitOptions.None);
                    XTitle = vals[0];
                    YTitle = vals[1];

                }
                else if (str.Contains(","))//reads Comma delimited files
                {
                    vals = str.Split(new string[] { "," }, StringSplitOptions.None);
                    XTitle = vals[0];
                    YTitle = vals[1];
                }

                for (int i = 1; i < vals.Length; i++)
                    YVals.Add(new List<double>());

                //Process the rest of the file
                str = sr.ReadLine();
                while (str != null)
                {
                    try
                    {
                        if (str.Contains("\t"))//Reads TAB delimited files
                        {
                            vals = str.Split(new string[] { "\t" }, StringSplitOptions.None);

                            XVals.Add(double.Parse(vals[0]));


                            microYvals = new double[vals.Length - 1];

                            for (int i = 1; i < vals.Length; i++)
                                YVals[i - 1].Add(double.Parse(vals[i]));
                        }
                        else if (str.Contains(","))//reads Comma delimited files
                        {
                            vals = str.Split(new string[] { "," }, StringSplitOptions.None);
                            XVals.Add(double.Parse(vals[0]));

                            for (int i = 1; i < vals.Length; i++)
                                YVals[i - 1].Add(double.Parse(vals[i]));
                        }
                    }
                    catch { }

                    str = sr.ReadLine();
                }

                double[][] YvalsArr = new double[YVals.Count][];
                double[][] fitYvalsArr = new double[YVals.Count][];

                for (int i = 0; i < YVals.Count; i++)
                {
                    YvalsArr[i] = YVals[i].ToArray();
                    fitYvalsArr[i] = new double[YVals[i].Count()];
                }
                //Fill the data variable
                data = new ODEModel.Data(dir, XTitle, YTitle, XVals.ToArray(), YvalsArr, fitYvalsArr);

                //Clear
                XVals = null;
                YVals = null;
                vals = null;
                fitYvalsArr = null;
                YvalsArr = null;
                microYvals = null;
            }
        }
    }
}
