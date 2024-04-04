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
namespace ODEModel
{
    public class Model
    {
        /// <summary>
        /// Returns the name of the model as string
        /// </summary>
        /// <returns></returns>
        public virtual string GetName()
        {
            return "MyModel";
        }
        /// <summary>
        /// Returns the mathematical model as string
        /// </summary>
        /// <returns></returns>
        public virtual string GetModel()
        {
            return "f(x) = A*x";
        }
        /// <summary>
        /// Open video form
        /// </summary>
        /// <returns>True if video is available</returns>
        public virtual bool GetVideo()
        {
            return false;
        }
        /// <summary>
        /// Open Profile form
        /// </summary>
        /// <returns>True if video is available</returns>
        public virtual bool GetProfile()
        {
            return false;
        }
        /// <summary>
        /// Returns additional StDev
        /// </summary>
        /// <returns></returns>
        public virtual double GetStDev()
        {
            return -1d;
        }
        public virtual void RefreshGUI()
        {
        }
        /// <summary>
        /// Declaring the variables for the model
        /// </summary>
        /// <param name="data">Input data set</param>
        public virtual void AddVariables(Data data)
        {
            #region My variables

            //Declare variables here
            data.AddVariable("A", 10, 0, 20);

            #endregion My variables
        }
        /// <summary>
        /// Declare the matematical function and calculate the fit values
        /// </summary>
        /// <param name="data">Input data set</param>
        public virtual void Calculations(Data data)
        {
            #region My variables

            //Assign variables here
            Variable A = data.GetVariable("A");

            #endregion My variables

            for (int i = 0; i < data.Xvalues.Length && i < data.Yvalues.Length && i < data.fitYvalues.Length; i++)
                for (int j = 0; j < data.Xvalues.Length && j < data.Yvalues[i].Length && j < data.fitYvalues[i].Length; j++)
                {
                    #region My function

                    //Write the math here
                    data.fitYvalues[i][j] = data.Xvalues[j] * A.Value;

                    #endregion My function
                }
        }
        
    }
}
