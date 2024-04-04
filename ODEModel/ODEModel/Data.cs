/*
 ODEModel - software for mathematical modeling of biological processes
 Copyright (C) 2018  Georgi Danovski

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODEModel
{
    /// <summary>
    /// Store axis values, variables and mathematical model
    /// </summary>
    public class Data
    {
        private string _Dir;
        private string _Xtitle;
        private string _Ytitle;
        private double[] _Xvalues;
        private double[][] _Yvalues;
        private double[][] _fitYvalues;
        private Dictionary<string, Variable> _Variables;
        private Model _MyModel;
        /// <summary>
        /// The full path to the file
        /// </summary>
        public string FileDirectory
        {
            get { return this._Dir; }
            set { this._Dir = value; }
        }
        /// <summary>
        /// X axis title
        /// </summary>
        public string Xtitle
        {
            get { return this._Xtitle; }
            set { this._Xtitle = value; }
        }
        /// <summary>
        /// Y axis title
        /// </summary>
        public string Ytitle
        {
            get { return this._Ytitle; }
            set { this._Ytitle = value; }
        }
        /// <summary>
        /// X axis values as double array
        /// </summary>
        public double[] Xvalues
        {
            get { return this._Xvalues; }
            set { this._Xvalues = value; }
        }
        /// <summary>
        /// Y axis values as set of double arrays
        /// </summary>
        public double[][] Yvalues
        {
            get { return this._Yvalues; }
            set { this._Yvalues = value; }
        }
        /// <summary>
        /// Fit Y axis values as set of double arrays
        /// </summary>
        public double[][] fitYvalues
        {
            get { return this._fitYvalues; }
            set { this._fitYvalues = value; }
        }
        /// <summary>
        /// The variables of the model
        /// </summary>
        public Dictionary<string,Variable> Variables
        {
            get { return this._Variables; }
            set { this._Variables = value; }
        }
        /// <summary>
        /// Mathematical model used for the current data set
        /// </summary>
        public Model MyModel
        {
            get { return this._MyModel; }
            set { this._MyModel = value; }
        }
        /// <summary>
        /// Empty data set
        /// </summary>
        public Data()
        {
            this._Dir = "";
            this._Xtitle = "";
            this._Ytitle = "";
            this._Xvalues = new double[0];
            this._Yvalues = new double[0][];
            this._fitYvalues = new double[0][];
            this._Variables = new Dictionary<string, Variable>();
            this._MyModel = new Model();
        }
        /// <summary>
        /// Data set with predefined values
        /// </summary>
        /// <param name="FileDirectory"> Full path to the source file</param>
        /// <param name="Xtitle">X axis title</param>
        /// <param name="Ytitle"> Y axis title</param>
        /// <param name="Xvalues"> X values</param>
        /// <param name="Yvalues">Y axis values as set of double arrays </param>
        /// <param name="fitYvalues">Fit Y axis values as set of double arrays</param>
        public Data(string FileDirectory,string Xtitle,string Ytitle,double[] Xvalues,double[][] Yvalues,double[][] fitYvalues)
        {
            this._Dir = FileDirectory;
            this._Xtitle = Xtitle;
            this._Ytitle = Ytitle;
            this._Xvalues = Xvalues;
            this._Yvalues = Yvalues;
            this._fitYvalues = fitYvalues;
            this._Variables = new Dictionary<string, Variable>();
            this._MyModel = new Model();
        }
        /// <summary>
        /// Get variable by name
        /// </summary>
        /// <param name="Name"> Name of variable</param>
        /// <returns></returns>
        public Variable GetVariable(string Name)
        {
            if (!_Variables.ContainsKey(Name)) return null;

            return _Variables[Name];
        }
        /// <summary>
        /// Get variable by index
        /// </summary>
        /// <param name="Index"> Index of the variable</param>
        /// <returns></returns>
        public Variable GetVariable(int Index)
        {
            if (_Variables.Count < Index || Index < 0) return null;

            return _Variables.ElementAt(Index).Value;
        }
        /// <summary>
        /// Change the values of an variable
        /// </summary>
        public Variable SetVariable
        {
            set
            {
                if (_Variables.ContainsKey(value.Name))
                {
                    _Variables[value.Name].Name = value.Name;
                    _Variables[value.Name].Value = value.Value;
                    _Variables[value.Name].Maximum = value.Maximum;
                    _Variables[value.Name].Minimum = value.Minimum;
                }
                else
                {
                    _Variables.Add(value.Name, value);
                }
            }
        }
        /// <summary>
        /// Add new variable
        /// </summary>
        /// <param name="Name">Variable name</param>
        /// <param name="Value">Variable value</param>
        /// <param name="Maximum">Variable maximal possible value</param>
        /// <param name="Minimum">Variable minimal possible value</param>
        /// <param name="DisplayName">The name of the variable as string to be shown in the GUI</param>
        public void AddVariable(string Name, double Value, double Maximum, double Minimum, string DisplayName = null)
        {
            Variable value = new Variable(Name, Value, Maximum, Minimum, DisplayName);

            if (_Variables.ContainsKey(value.Name))
            {
                _Variables[value.Name].Name = value.Name;
                _Variables[value.Name].Value = value.Value;
                _Variables[value.Name].Maximum = value.Maximum;
                _Variables[value.Name].Minimum = value.Minimum;
                _Variables[value.Name].DisplayName = value.DisplayName;
            }
            else
            {
                _Variables.Add(value.Name, value);
            }
        }
        /// <summary>
        /// Solve the model with the selected values for the variables
        /// </summary>
        /// <returns>Solve the model and returns the StDev[Sum((Yval[n] - fitYval[n])^2)/N] as double</returns>
        public double SolveModel(double[] MultiplyMatrix)
        {
            MyModel.Calculations(this);

            double[] StDev = GetStDev();
            double oldStDev = StDev.Average();
            double newStDev = 0;
            if (MultiplyMatrix != null)
            {
                for (int i = 0; i < StDev.Length && i < MultiplyMatrix.Length; i++)
                    StDev[i] *= MultiplyMatrix[i];
                newStDev = StDev.Average();

                Console.WriteLine("StDev Scale: " + string.Join(", ", MultiplyMatrix));
                Console.WriteLine("StDev:\t" + oldStDev + "\tEffective StDev:\t" + newStDev);
                oldStDev = newStDev;
            }
            else
                Console.WriteLine("StDev:\t" + oldStDev);
            Console.WriteLine();

            return StDev.Average();
        }
        /// <summary>
        /// Returns the StDev[Sum((Yval[n] - fitYval[n])^2)/N] for each data series as double array
        /// </summary>
        /// <returns></returns>
        public double[] GetStDev()
        {
            if (Yvalues == null && fitYvalues == null) return new double[0];

            double[] StDev = new double[Yvalues.Length];
            int Counter;
            
            for(int i = 0; i< Yvalues.Length && i< fitYvalues.Length; i++)
            {
                Counter = 0;

                for(int j = 0; j<Yvalues[i].Length && j< fitYvalues[i].Length; j++, Counter++)
                    StDev[i] += Math.Pow(Yvalues[i][j] - fitYvalues[i][j],2);

                if(Counter>0) StDev[i] /= Counter;
            }

            if(MyModel != null)
            {
                double addDev = MyModel.GetStDev();
                if(addDev != -1d)
                {
                    List<double> StDev_list = StDev.ToList();
                    StDev_list.Add(addDev);
                    StDev = StDev_list.ToArray();
                }
            }

            return StDev;
        }
    }
}
