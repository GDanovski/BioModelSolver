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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using ODEModel;

namespace ODESolver
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer aTimer;
        private string CurrentModel = "";
        private ODESolverCore core;
        private Solver solver;
        private ResultsExtractor_CTChart chart;
        private Data data;
        private string[] colorsMatrix = new string[] {"blue","red","#00b300", "#b300b3", "#00bfff", "#ffcc00", "#ff471a", "#cc6699", "#39e600"
                , "#00b3b3", "#ffcc66", "#7575a3", "#ff1a1a", "#ff0055", "#8a00e6", "#bf8040",
                "#53c68c", "#ace600", "#b33c00", "#ff6666"};
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = this.Text + " " + version;
            Console.WriteLine(this.Text + "\n");

            this.core = new ODESolverCore();
            this.solver = new Solver();
            this.chart = new ResultsExtractor_CTChart();
            this.chart.Dock = DockStyle.Fill;
            panel_OpenGL.Controls.Add(this.chart);
            this.chart.Build();
            //Load Models
            foreach (var model in core.Models)
                comboBox_Models.Items.Add(model.Key);
            // comboBox_Models.SelectedIndexChanged +=

            if (core.Models.Count > 0) comboBox_Models.SelectedIndex = 0;
            
            //enable GLControl
            this.chart.Loaded = true;
            this.Legend_TreeView.AfterCheck += new TreeViewEventHandler(TreeView1_NodeChecked);

            this.Legend_TreeView.MouseClick += new MouseEventHandler(delegate (object o, MouseEventArgs a) 
            {
                if (a.Button != MouseButtons.Right) return;

                if (((TreeView)o).Nodes.Count == 0) return;

                bool isChecked = !((TreeView)o).Nodes[0].Checked;
                ((TreeView)o).SuspendLayout();
                foreach (TreeNode tn in ((TreeView)o).Nodes)
                    tn.Checked = isChecked;
                ((TreeView)o).ResumeLayout();

                ((TreeView)o).Invalidate();
                ((TreeView)o).Update();
                ((TreeView)o).Refresh();

                LoadChart();
            });

            if (!this.core.DebugMode)
            {
                textBox_StDevScale.Visible = false;
                label8.Visible = false;
                button_Solve.Visible = false;
                button_Stop.Visible = false;
            }
            //Set autosave timer
            SetTimer();
        }
        private void SetTimer()
        {
            this.CurrentModel = (string)comboBox_Models.SelectedItem;
            richTextBox_Log.AppendText("------------------------------\nRestore variables:");
            richTextBox_Log.AppendText(System.IO.File.ReadAllText("temp.txt") + 
                "\n------------------------------\n");
            // Create a timer with a 10 min interval.
            aTimer = new System.Timers.Timer(600000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (data == null || data.Variables.Count == 0) return;
            
            Variable var = null;
            string[] lines = new string[data.Variables.Count + 1];
            lines[0] = CurrentModel;

            for (int i = 0; i < data.Variables.Count; i++)
            {
                var = data.Variables.ElementAt(i).Value;
                lines[i + 1] = string.Join("\t", new string[] { var.Name, var.Value.ToString(), var.Maximum.ToString(), var.Minimum.ToString() });
            }

            try
            {
                System.IO.File.WriteAllLines("temp.txt", lines);
            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }
            /*
            Console.WriteLine("------------------------------\nAuto save:\n" 
                + string.Join("\n",lines) + 
                "\n------------------------------\n");
            */
            var = null;
            lines = null;
        }
        private void button_Info_Click(object sender, EventArgs e)
        {
            if (!(comboBox_Models.SelectedIndex > -1)) return;

            string key = (string)comboBox_Models.SelectedItem;
            try
            {
                InfoForm infoForm = new InfoForm();
                infoForm.Text = key;
                infoForm.richTextBox.Text = core.Models[key].GetModel();

                infoForm.ShowDialog();
                infoForm.Dispose();
            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }
        }

        private void button_Load_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string dir = openFileDialog1.FileName;

                if (!System.IO.File.Exists(dir))
                {
                    MessageBox.Show("File not existing!");
                }

                try
                {
                    Data data = null;
                    FileEncoder.ReadFile(dir, out data);

                    this.data = data;

                    label_Dir.Text = "Name: " + dir;
                    label_Dir.Visible = true;
                    label_StDev.Visible = true;
                    label_YMax.Visible = true;
                    textBox_YMax.Visible = true;

                    if (!(comboBox_Models.SelectedIndex > -1)) return;

                    string key = (string)comboBox_Models.SelectedItem;
                    this.data.MyModel = core.Models[key];

                    AddVariables();
                    this.data.MyModel.Calculations(this.data);
                    LoadChart();

                    richTextBox_Log.AppendText("\n----------------------------------\nFile loaded: " + dir + "\n");
                }
                catch
                {
                    MessageBox.Show("File not avaliable!");
                }
            }
        }
        private void AddVariables()
        {
            panel_Variables.Controls.Clear();

            if (this.data == null || this.data.MyModel == null) return;

            this.data.Variables.Clear();
            this.data.MyModel.AddVariables(this.data);

            foreach (var v in this.data.Variables)
            {
                Panel_Variable p = new Panel_Variable(v.Value, checkBox_AutoRefresh.Checked);
                panel_Variables.Controls.Add(p);

                p.ReloadChartNeeded += ReloadChartNeeded_Activate;
            }

            checkVariablesVisibility();
        }
        private void ReloadChartNeeded_Activate(object sender, EventArgs e)
        {
            if (!((Panel_Variable)sender).Enabled || this.data == null) return;

            //this.data.MyModel.Calculations(this.data);
            //LoadChart();
            SingleModelRefresh();
        }
        private void comboBox_Models_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(comboBox_Models.SelectedIndex > -1) || data == null) return;

            string key = (string)comboBox_Models.SelectedItem;
            this.CurrentModel = key;
            this.data.MyModel = core.Models[key];

            AddVariables();
            //this.data.MyModel.Calculations(this.data);
            //LoadChart();
            SingleModelRefresh();
        }
        private void SingleModelRefresh()
        {
            BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            AutoResetEvent resetEvent = new AutoResetEvent(false);

            worker.DoWork += new DoWorkEventHandler(delegate (object sender, DoWorkEventArgs e)
            {
                this.data.MyModel.Calculations(this.data);
                resetEvent.Set();
            });
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate (object sender, RunWorkerCompletedEventArgs e)
            {
                LoadChart();
            });

            worker.WorkerReportsProgress = true;

            worker.RunWorkerAsync();
            resetEvent.WaitOne();
        }
        private void LoadChart()
        {
            //Refresh the legend tree view
            ReloadTreeView();
            if (data == null) return;            
            //Load series
            chart.ChartSeries.Clear();
            /*
            for (int i = 0, colInd = 0; i < this.data.Yvalues.Length; i++)
            {
                ResultsExtractor_CTChart.Series Yvals = new ResultsExtractor_CTChart.Series();
                if (colInd >= colorsMatrix.Length) colInd = 0;
                Yvals.Color = ColorTranslator.FromHtml(colorsMatrix[colInd]);
                colInd++;

                ResultsExtractor_CTChart.Series fitYvals = new ResultsExtractor_CTChart.Series();
                if (colInd >= colorsMatrix.Length) colInd = 0;
                fitYvals.Color = ColorTranslator.FromHtml(colorsMatrix[colInd]);
                colInd++;

                for (int j = 0; j < this.data.Yvalues[i].Length && j < this.data.Xvalues.Length; j++)
                {
                    Yvals.Points.AddXY(this.data.Xvalues[j], this.data.Yvalues[i][j]);
                    fitYvals.Points.AddXY(this.data.Xvalues[j], this.data.fitYvalues[i][j]);
                }

                chart.ChartSeries.Add(Yvals);
                chart.ChartSeries.Add(fitYvals);
            }*/
            int colInd = 0;

            for (int i = 0; i < this.data.Yvalues.Length; i++)
            {
                ResultsExtractor_CTChart.Series Yvals = new ResultsExtractor_CTChart.Series();
                if (colInd >= colorsMatrix.Length) colInd = 0;
                Yvals.Color = ColorTranslator.FromHtml(colorsMatrix[colInd]);               
                 colInd++;
                //start legend
                if (Legend_TreeView.Nodes[i].Text != "Raw " + i) Legend_TreeView.Nodes[i].Text = "Raw " + i;
                if (Legend_TreeView.Nodes[i].ForeColor != Yvals.Color) Legend_TreeView.Nodes[i].ForeColor = Yvals.Color;
                //end legend
                if (Legend_TreeView.Nodes[i].Checked)
                    for (int j = 0; j < this.data.Yvalues[i].Length && j < this.data.Xvalues.Length; j++)
                    {
                        Yvals.Points.AddXY(this.data.Xvalues[j], this.data.Yvalues[i][j]);
                    }

                chart.ChartSeries.Add(Yvals);
            }
            for (int i = 0, ind = this.data.Yvalues.Length; i < this.data.fitYvalues.Length; i++, ind++)
            {
                try
                {
                    ResultsExtractor_CTChart.Series fitYvals = new ResultsExtractor_CTChart.Series();
                    if (colInd >= colorsMatrix.Length) colInd = 0;
                    fitYvals.Color = ColorTranslator.FromHtml(colorsMatrix[colInd]);
                    colInd++;
                    //start legend
                    if (Legend_TreeView.Nodes[ind].Text != "Fit " + i) Legend_TreeView.Nodes[ind].Text = "Fit " + i;
                    if (Legend_TreeView.Nodes[ind].ForeColor != fitYvals.Color) Legend_TreeView.Nodes[ind].ForeColor = fitYvals.Color;
                    //end legend
                    if (Legend_TreeView.Nodes[ind].Checked)
                        for (int j = 0; j < this.data.fitYvalues[i].Length && j < this.data.Xvalues.Length; j++)
                        {
                            fitYvals.Points.AddXY(this.data.Xvalues[j], this.data.fitYvalues[i][j]);
                        }

                    chart.ChartSeries.Add(fitYvals);
                }
                catch
                {
                    Console.WriteLine("Error: load chart legend");
                }
            }
            //Start drawing
            chart.LoadData(data);
            
            chart.Update();
            chart.Invalidate();
            chart.Refresh();

            label_StDev.Text = "StDev: " + data.GetStDev().Average();
            label_StDev.Update();
            label_StDev.Invalidate();
            label_StDev.Refresh();

            data.MyModel.RefreshGUI();

            Application.DoEvents();
        }
        private void ReloadTreeView()
        {
            if (data == null)
            {
                Legend_TreeView.Nodes.Clear();
                return;
            }

            int length = this.data.Yvalues.Length + this.data.fitYvalues.Length;

            if(Legend_TreeView.Nodes.Count != length)
            {
                Legend_TreeView.Nodes.Clear();
                for (int i = 0; i < length; i++)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = i.ToString();
                    tn.Checked = true;
                    Legend_TreeView.Nodes.Add(tn);
                }
            }
        }
        private void TreeView1_NodeChecked(object sender, TreeViewEventArgs e)
        {
            LoadChart();
        }
        private double[] GetStDevScale()
        {
            string str = textBox_StDevScale.Text.Replace(" ", "");
            
            string[] vals = str.Split(new string[] { "&" }, StringSplitOptions.None);

            if (vals.Length == 0 || str == "") return null;

            double[] output = new double[vals.Length];
            double temp;

            for (int i = 0; i < vals.Length; i++)
                if (double.TryParse(vals[i], out temp))
                {
                    output[i] = temp;
                }
                else
                {
                    output[i] = 1d;
                    MessageBox.Show("Incorrect StDev correction value: " + vals[i]);
                }
            //report
            Console.WriteLine("StDev correction:\t" + string.Join("\t", output));
            return output;
        }
        private void button_Solve_Click(object sender, EventArgs e)
        {
            if (data == null) return;

            int iterations = 0;

            if (!int.TryParse(textBox_Iterations.Text, out iterations))
            {
                MessageBox.Show("The number of iterations is not correct!");
                return;
            }
            
            var bgw = solver.Solve(data, iterations, GetStDevScale());

            //Disable controls
            foreach (var contr in panel_Variables.Controls)
                ((Panel_Variable)contr).Enabled = false;

            comboBox_Models.Enabled = false;
            textBox_Iterations.Enabled = false;
            button_Load.Enabled = false;
            button_Solve.Enabled = false;
            button_Refresh.Enabled = false;
            checkBox_AutoRefresh.Enabled = false;
            openToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            exportChartToolStripMenuItem.Enabled = false;
            exportDataToolStripMenuItem.Enabled = false;

            int counter = 0;
            label_Counter.Text = "Count: " + counter;
            label_Counter.Visible = true;

            bool render = true;
            //add events
            bgw.ProgressChanged += new ProgressChangedEventHandler(delegate (Object o, ProgressChangedEventArgs a)
            {
                if (a.ProgressPercentage == 0 && render)
                {
                    render = false;

                    counter++;
                    //label_Counter.Text = "Count: " + counter;
                    if (solver.solver != null)
                        label_Counter.Text = "Count: " + solver.solver.IterationCount;
                    else
                        label_Counter.Text = "";

                    label_Counter.Update();
                    label_Counter.Invalidate();
                    label_Counter.Refresh();

                    LoadChart();

                    render = true;
                }
                else if (a.ProgressPercentage == 1)
                {
                    foreach (var contr in panel_Variables.Controls)
                    {
                        Panel_Variable p = (Panel_Variable)contr;
                        p.RefreshValues();
                        p.Enabled = true;
                    }
                    LoadChart();

                    comboBox_Models.Enabled = true;
                    textBox_Iterations.Enabled = true;
                    button_Load.Enabled = true;
                    button_Solve.Enabled = true;
                    label_Counter.Visible = false;
                    button_Refresh.Visible = true;
                    button_Refresh.Enabled = true;
                    checkBox_AutoRefresh.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    exportChartToolStripMenuItem.Enabled = true;
                    exportDataToolStripMenuItem.Enabled = true;

                    richTextBox_Log.AppendText(ResultsFromSolving(counter));
                }
            });

            //Run
            bgw.RunWorkerAsync();
        }
        private string ResultsFromSolving(int iterations)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\n----------------------------------\nThe Result StDev is "
                + this.data.GetStDev().Average() + ".");
            sb.AppendLine("Obtained for " + iterations + " iterations.");

            sb.AppendLine("\nConstats:");

            foreach (var kvp in this.data.Variables)
                sb.AppendLine(kvp.Value.Details);
            
            return sb.ToString();

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (data == null || data.Variables.Count == 0) return;

            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "BioModelSolver Variables (*.BioModelVariables)|*.BioModelVariables";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.OverwritePrompt = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Variable var = null;
                    string[] lines = new string[data.Variables.Count+1];
                    lines[0] = comboBox_Models.Text;

                    for (int i = 0; i < data.Variables.Count; i++)
                    {
                        var = data.Variables.ElementAt(i).Value;
                        lines[i+1] = string.Join("\t", new string[] { var.Name, var.Value.ToString(), var.Maximum.ToString(), var.Minimum.ToString() });
                    }

                    try
                    {
                        System.IO.File.WriteAllLines(saveFileDialog1.FileName, lines);
                    }
                    catch(Exception a)
                    {
                        MessageBox.Show(a.Message);
                    }
                   
                    var = null;
                    lines = null;
                }
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (data == null || data.Variables.Count == 0) return;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "BioModelSolver Variables (*.BioModelVariables)|*.BioModelVariables";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] lines = null;
                    string[] line=null;
                    double temp;

                    try
                    {
                        lines = System.IO.File.ReadAllLines(openFileDialog.FileName);
                    }
                    catch(Exception a)
                    {
                        MessageBox.Show(a.Message);
                        return;
                    }

                    string modelName = lines[0];

                    if(comboBox_Models.Text != modelName && comboBox_Models.Items.Contains(modelName))
                    {
                        comboBox_Models.SelectedIndex = comboBox_Models.Items.IndexOf(modelName);
                    }
                    if (data == null || data.Variables == null || data.Variables.Count == 0) return;

                   foreach(string str in lines)
                    {
                        line = str.Split(new string[] { "\t" }, StringSplitOptions.None);
                        if (line.Length != 4) continue;

                        foreach(var variable in data.Variables)
                            if(variable.Key == line[0])
                            {                                
                                if(double.TryParse(line[1], out temp)) variable.Value.Value = temp;
                                if (double.TryParse(line[2], out temp)) variable.Value.Maximum = temp;
                                if (double.TryParse(line[3], out temp)) variable.Value.Minimum = temp;
                                break;
                            }
                    }

                    checkVariablesVisibility();

                    foreach (var contr in panel_Variables.Controls)
                    {
                        Panel_Variable p = (Panel_Variable)contr;
                        p.RefreshValues();
                    }
                    this.data.MyModel.Calculations(this.data);
                    LoadChart();
                }
            }
        }

        private void exportChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (data == null || data.Variables.Count == 0) return;

            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "Bitmap image (*.bmp)|*.bmp";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.OverwritePrompt = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = chart.TakeScreenshot();
                    try
                    {
                        bmp.Save(saveFileDialog1.FileName);
                    }
                    catch(Exception a)
                    {
                        MessageBox.Show(a.Message);
                    }
                }
            }
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (data == null || data.Variables.Count == 0) return;

            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "TAB delimited text file (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.OverwritePrompt = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    int i, j, k;
                    string[] lines = new string[data.Xvalues.Length+1];
                    int length = data.Yvalues.Length + data.fitYvalues.Length + 1;
                    string[] line = new string[length];
                    line[0] = data.Xtitle;
                    for (j = 1, k = 0; j < length && k < data.Yvalues.Length; j++, k++)
                        line[j] = "Raw_" + k;
                    for (k = 0; j < length && k < data.fitYvalues.Length; j++, k++)
                        line[j] = "Fit_" + k;
                    lines[0] = string.Join("\t", line);

                    for (i = 0; i < data.Xvalues.Length; i++)
                    {
                        line[0] = data.Xvalues[i].ToString();

                        for (j = 1, k = 0; j < length && k < data.Yvalues.Length; j++, k++)
                            line[j] = data.Yvalues[k].Length > i ? data.Yvalues[k][i].ToString() : "0";
                        for (k = 0; j < length && k < data.fitYvalues.Length; j++, k++)
                            line[j] = data.fitYvalues[k].Length > i ? data.fitYvalues[k][i].ToString() : "0";

                        lines[i + 1] = string.Join("\t", line);
                    }

                    try
                    {
                        System.IO.File.WriteAllLines(saveFileDialog1.FileName, lines);
                    }
                    catch (Exception a)
                    {
                        MessageBox.Show(a.Message);
                    }

                    line = null;
                    lines = null;
                }
            }

        }

        private void textBox_YMax_TextChanged(object sender, EventArgs e)
        {
            double value = double.MinValue;
            if (double.TryParse(textBox_YMax.Text, out value))
                chart.SetConstMaxY = value;
            else
                chart.SetConstMaxY = double.MinValue;

            LoadChart();
        }

        private void button_Refresh_Click(object sender, EventArgs e)
        {
            foreach (var contr in panel_Variables.Controls)
            {
                Panel_Variable p = (Panel_Variable)contr;
                p.AssignValues();
            }
            //this.data.MyModel.Calculations(this.data);
            //LoadChart();
            SingleModelRefresh();
        }
        private void checkBox_AutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var contr in panel_Variables.Controls)
            {
                Panel_Variable p = (Panel_Variable)contr;
                p.SetAutoRefresh = checkBox_AutoRefresh.Checked;
            }

            button_Refresh.Visible = true;
            button_Refresh.Enabled = true;

            if (checkBox_AutoRefresh.Checked)
                button_Refresh_Click(button_Refresh, new EventArgs());
        }

        private void licenseAgreementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form licenseForm = new Form()
            {
                Text = "License Agreement",
                Size = new Size(500, 500),
                MinimumSize = new Size(400, 300)

            };

            licenseForm.Controls.Add(new RichTextBox()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Text = BioModelSolver.Properties.Resources.LicenseAgreement_txt
            });
            licenseForm.ShowDialog();
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            solver.Abort = true;
        }

        private void button_Video_Click(object sender, EventArgs e)
        {
            if (!(comboBox_Models.SelectedIndex > -1)) return;

            string key = (string)comboBox_Models.SelectedItem;
            try
            {
                bool res = core.Models[key].GetVideo();
                if (!res)
                {
                    MessageBox.Show("Video not available");
                }
            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }
        }

        private void button_Profile_Click(object sender, EventArgs e)
        {
            if (!(comboBox_Models.SelectedIndex > -1)) return;

            string key = (string)comboBox_Models.SelectedItem;
            try
            {
                bool res = core.Models[key].GetProfile();
                if (!res)
                {
                    MessageBox.Show("Profile not available");
                }
            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }
        }

        private void checkBox_ShowConst_CheckedChanged(object sender, EventArgs e)
        {
            checkVariablesVisibility();
        }
        private void checkVariablesVisibility()
        {
            bool state = checkBox_ShowConst.Checked;

            foreach (var contr in panel_Variables.Controls)
            {
                Panel_Variable p = (Panel_Variable)contr;
                if (p.Variable.Maximum == p.Variable.Minimum && p.Variable.Maximum == p.Variable.Value)
                {
                    p.Visible = state;
                }
                else
                {
                    p.Visible = true;
                }
            }
        }
    }
}
