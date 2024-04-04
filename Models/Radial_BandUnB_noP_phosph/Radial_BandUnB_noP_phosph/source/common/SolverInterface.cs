﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ODEModel;

namespace Radial_BandUnB_noP_phosph.source.common
{
    public class SolverInterface: IDisposable
    {
        public SolverInterface() { }
        public virtual double[][] Start(bool printProfile, bool printResults, bool reportProgres, bool reportProgressForm, bool GetImages = false) {
            return null;
        }
        public virtual double[][] Results { get; set; }
        public Data ModelData { get; set; }
        public virtual void Dispose() { }
    }
}
