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
    /// <summary>
    /// Store information abbout the variables
    /// </summary>
    public class Variable
    {
        private string _Name;//The name of the variable
        private string _DisplayName;//The name of the variable to be displayed in GUI
        private double _Value;//the value of the variable
        private double _Maximum;//the maximal possible value of the variable
        private double _Minimum;//the minimal possible value of the variable
        /// <summary>
        /// The name of the variable
        /// </summary>
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }
        /// <summary>
        /// The name of the variable to be shown in the GUI.
        /// </summary>
        public string DisplayName
        {
            get {
                if (this._DisplayName != null)
                {
                    return this._DisplayName;
                }
                else
                {
                    return this._Name;
                }
            }
            set { this._DisplayName = value; }
        }
        /// <summary>
        /// The value of the variable
        /// </summary>
        public double Value
        {
            get { return this._Value; }
            set { this._Value = value; }
        }
        /// <summary>
        /// The maximal possible value of the variable
        /// </summary>
        public double Maximum
        {
            get { return this._Maximum; }
            set { this._Maximum = value; }
        }
        /// <summary>
        /// The minimal possible value of the variable
        /// </summary>
        public double Minimum
        {
            get { return this._Minimum; }
            set { this._Minimum = value; }
        }
        /// <summary>
        /// Declare empty variable
        /// </summary>
        public Variable()
        {

        }
        /// <summary>
        /// Declare variable with predefined parameters
        /// </summary>
        /// <param name="Name">The name of the variable as string</param>
        /// <param name="Value">The value of the variable as double</param>
        /// <param name="Maximum">The maximal possible value of the variable as double</param>
        /// <param name="Minimum">The minimal possible value of the variable as double</param>
        /// <param name="DisplayName">The name of the variable as string to be shown in the GUI</param>
        public Variable(string Name, double Value, double Maximum, double Minimum, string DisplayName = null)
        {
            this._Name = Name;
            this._DisplayName = DisplayName;
            this._Value = Value;
            this._Maximum = Maximum;
            this._Minimum = Minimum;
        }
        /// <summary>
        /// Returns description of the variable
        /// </summary>
        public string Details
        {
            get
            {
                return "Name: " + Name +
                         "\tValue: " + Value.ToString() +
                         "\tMaximum: " + Maximum.ToString() +
                         "\tMinimum: " + Minimum.ToString();
            }
        }
    }
}
