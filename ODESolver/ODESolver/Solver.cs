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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation;
using Microsoft.SolverFoundation.Services;

namespace ODESolver
{
    public class Solver
    {
        private bool _Abort;
        private BackgroundWorker bgw;
        private ODEModel.Data data;       
        private double[] StDevScale;
        public Microsoft.SolverFoundation.Solvers.NelderMeadSolver solver;
        private Microsoft.SolverFoundation.Solvers.NelderMeadSolverParams param;
        public bool Abort
        {
            set
            {
                this._Abort = value;
            }
            get
            {
                return this._Abort;
            }
        }
        public BackgroundWorker Solve(ODEModel.Data data, int iterations, double[] StDevScale)
        {             
            this.StDevScale = StDevScale;
            
            //background worker
            this.bgw = new BackgroundWorker();
            this.bgw.WorkerReportsProgress = true;
            this.Abort = false;
            //define constants
            this.data = data;

            NelderMeadSolver_Start(iterations);

            return bgw;
        }
        private void NelderMeadSolver_Start(int iterations)
        {
            param = new Microsoft.SolverFoundation.Solvers.NelderMeadSolverParams();
            int[] constants;
            //Solver
            solver = new Microsoft.SolverFoundation.Solvers.NelderMeadSolver();
            
            // Objective function.
            int objId;
            solver.AddRow("obj", out objId);
            solver.AddGoal(objId, 0, true);

            // Define variables.
            constants = new int[data.Variables.Count];

            for (int i = 0; i < data.Variables.Count; i++)
            {
                ODEModel.Variable v = data.Variables.ElementAt(i).Value;
                if (v.Maximum == v.Minimum) continue;

                solver.AddVariable(v.Name, out constants[i]);
                solver.SetLowerBound(constants[i], v.Minimum);
                solver.SetUpperBound(constants[i], v.Maximum);
                solver.SetValue(constants[i], v.Value);
            }

            // Assign objective function delegate.
            solver.FunctionEvaluator = FunctionValue;

            // Solve.            
           param.IterationLimit = iterations;
            Microsoft.SolverFoundation.Services.INonlinearSolution solution = null;
            bgw.DoWork += new DoWorkEventHandler(delegate (Object o, DoWorkEventArgs a)
            {
                solution = solver.Solve(param);

                bgw.ReportProgress(2);
                bgw.ReportProgress(1);
            });

            bgw.ProgressChanged += new ProgressChangedEventHandler(delegate (Object o, ProgressChangedEventArgs a)
            {
                if (a.ProgressPercentage == 2)
                {
                    for (int i = 0; i < data.Variables.Count; i++)
                    {
                        ODEModel.Variable v = data.Variables.ElementAt(i).Value;
                        if (v.Maximum == v.Minimum) continue;
                        v.Value = solution.GetValue(constants[i]);
                    }
                }
            });
        }

        private double FunctionValue(INonlinearModel model, int rowVid,
                ValuesByIndex values, bool newValues)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("New values:");
            foreach (var c in data.Variables)
            {
                if (c.Value.Maximum == c.Value.Minimum) continue;
                c.Value.Value = values[model.GetIndexFromKey(c.Value.Name)];
                Console.WriteLine(c.Value.Name + "\t" + c.Value.Value);
            }
            Console.WriteLine();
            bgw.ReportProgress(0);

            if (this.Abort) param.Abort = true;

            return data.SolveModel(StDevScale);
        }
    }
}
