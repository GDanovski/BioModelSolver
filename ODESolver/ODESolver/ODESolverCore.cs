/*
 ODESolver - software for mathematical modeling of biological processes
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
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace ODESolver
{
    public class ODESolverCore
    {
        private string[] _ReleasedModels = new string[]
        {
            "Minimal aATM diffusion model",
            "Confined aATM model",
            "Standard aATM diffusion model"
        };

        private string _ExeDir;
        private Dictionary<string, ODEModel.Model> _Models;
        
        /// <summary>
        /// Parent directory of the executable
        /// </summary>
        public string ExeDir
        {
            get { return _ExeDir; }
            set { _ExeDir = value; }
        }
        /// <summary>
        /// Dictionary with the avaliable models (Kay = model name, Value = model)
        /// </summary>
        public Dictionary<string, ODEModel.Model> Models
        {
            get { return _Models; }
            set { _Models = value; }
        }
        /// <summary>
        /// Initialize ODEsolver core and load the models
        /// </summary>
        public ODESolverCore()
        {
            //Get the exe parent directory
            string dir = System.Reflection.Assembly.GetExecutingAssembly().Location;
            dir = dir.Substring(0, dir.LastIndexOf("\\"));
            this.ExeDir = dir;
            //Create the directory for the models
            dir += "\\Models";
            //Load the models
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            LoadModels(dir);
        }
        /// <summary>
        /// Load all models dll files avaliable in the selected directory
        /// </summary>
        /// <param name="dir">choose directory from where the models must be loaded</param>
        private void LoadModels(string dir)
        {
            //create new models dictionary
            this.Models = new Dictionary<string, ODEModel.Model>();
            //Get all model dlls
            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            // Check if not released models should be loaded.
            bool loadUnreleasedModels = File.Exists(dir + "\\debug.txt");
            //search for models in the directory and load them to the models dictionary
            foreach (var file in directoryInfo.GetFiles())

                try
                {
                    var DLL = Assembly.LoadFile(file.FullName);

                    foreach (Type type in DLL.GetExportedTypes())
                    {
                        try
                        {
                            var c = Activator.CreateInstance(type);

                            try
                            {
                                ODEModel.Model m = (ODEModel.Model)type.InvokeMember("GetModel", BindingFlags.InvokeMethod, null, c, new object[] { });

                                if (!Models.ContainsKey(m.GetName()) && (_ReleasedModels.Contains(m.GetName()) || loadUnreleasedModels))
                                    Models.Add(m.GetName(), m);
                            }
                            catch { }

                            break;
                        }
                        catch { }

                    }
                }
                catch { }
        }
    }

}

