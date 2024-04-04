using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Radial_BandUnB_noPX
{
    class M_Profile_Form
    {
        Form myForm;
        TrackBar tb;
        Label StDevLab;
        Label YmaxLab;
        TextBox YmaxTBox;
        System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        TreeView lb;
        bool refreshLB = true;
        Dictionary<string, bool> serEnabledStatus;
        double[][] _M_profile_raw;
        double[][] _dmg_profile;
        double[][] _M_profile_raw_OffSet;
        double[][] _M_profile_fit;
        double[][] _AX_PX_profile;
        double[][] _AX_profile;
        double[][] _PX_profile;
        double[][] _X0_T_profile;
        double[][] _P_profile;
        double[][] _A_P_AX_PX_profile;

        double[][] _dmg_Ku_profile;
        double[][] _AX_Ku_profile;
        double[][] _X0_Ku_T_profile;
        double[][] _A_AX_Ku_profile;

        double[][] _M_FRAP_profile_fit;
        double[][] _ATM_FRAP_profile_fit;
        double[][] _ATM_FRAP_Ku_profile_fit;

        public double[][] dmg_Ku_profile
        {
            set
            {
                this._dmg_Ku_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._dmg_Ku_profile;
            }
        }
        public double[][] AX_Ku_profile
        {
            set
            {
                this._AX_Ku_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._AX_Ku_profile;
            }
        }
        public double[][] X0_Ku_T_profile
        {
            set
            {
                this._X0_Ku_T_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._X0_Ku_T_profile;
            }
        }
        public double[][] A_AX_Ku_profile
        {
            set
            {
                this._A_AX_Ku_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._A_AX_Ku_profile;
            }
        }

        public double[][] M_profile_raw
        {
            set
            {
                this._M_profile_raw = value;
                recalibrateArrays();
            }
            get
            {
                return this._M_profile_raw;
            }
        }
        public double[][] AX_PX_profile
        {
            set
            {
                this._AX_PX_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._AX_PX_profile;
            }
        }
        public double[][] AX_profile
        {
            set
            {
                this._AX_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._AX_profile;
            }
        }
        public double[][] PX_profile
        {
            set
            {
                this._PX_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._PX_profile;
            }
        }
        public double[][] X0_T_profile
        {
            set
            {
                this._X0_T_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._X0_T_profile;
            }
        }
        public double[][] P_profile
        {
            set
            {
                this._P_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._P_profile;
            }
        }
        public double[][] A_P_AX_PX_profile
        {
            set
            {
                this._A_P_AX_PX_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._A_P_AX_PX_profile;
            }
        }
        public double[][] dmg_profile
        {
            set
            {
                this._dmg_profile = value;
                recalibrateArrays();
            }
            get
            {
                return this._dmg_profile;
            }
        }
        public double[][] M_profile_raw_OffSet
        {
            set
            {
                this._M_profile_raw_OffSet = value;
                recalibrateArrays();
            }
            get
            {
                return this._M_profile_raw_OffSet;
            }
        }
        public double[][] M_profile_fit
        {
            set
            {
                this._M_profile_fit = value;
                recalibrateArrays();
            }
            get
            {
                return this._M_profile_fit;
            }
        }
        public double[][] M_FRAP_profile_fit
        {
            set
            {
                this._M_FRAP_profile_fit = value;
                recalibrateArrays();
            }
            get
            {
                return this._M_FRAP_profile_fit;
            }
        }
        public double[][] ATM_FRAP_profile_fit
        {
            set
            {
                this._ATM_FRAP_profile_fit = value;
                recalibrateArrays();
            }
            get
            {
                return this._ATM_FRAP_profile_fit;
            }
        }
        public double[][] ATM_FRAP_Ku_profile_fit
        {
            set
            {
                this._ATM_FRAP_Ku_profile_fit = value;
                recalibrateArrays();
            }
            get
            {
                return this._ATM_FRAP_Ku_profile_fit;
            }
        }
        public void LoadGUI()
        {
            if (myForm != null)
            {
                myForm.Show();
                myForm.BringToFront();
                return;
            }

            myForm = new Form()
            {
                Text = "Profiles",
                Width = 400,
                Height = 400
            };

            GroupBox legendGB = new GroupBox()
            {
                Dock = DockStyle.Left,
                Width = 100,
                Text = "Legend"
            };
            myForm.Controls.Add(legendGB);
            serEnabledStatus = new Dictionary<string, bool>();

            lb = new TreeView()
            {
                Dock = DockStyle.Fill,
                CheckBoxes = true,
                ShowLines = false
            };
            legendGB.Controls.Add(lb);

            lb.MouseClick += new MouseEventHandler(delegate (object o, MouseEventArgs a)
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
            });

            Panel topPanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 90
            };
            myForm.Controls.Add(topPanel);

            Button importBtn = new Button()
            {
                Text = "Import",
                Location = new Point(5, 5)
            };
            topPanel.Controls.Add(importBtn);

            Button saveBtn = new Button()
            {
                Text = "Export",
                Location = new Point(90, 5)
            };
            topPanel.Controls.Add(saveBtn);

            Button videoBtn = new Button()
            {
                Text = "Video",
                Location = new Point(180, 5)
            };
            topPanel.Controls.Add(videoBtn);

            Label frameLab = new Label()
            {
                Location = new Point(300, 7),
                Text = "Frame: 0"
            };
            topPanel.Controls.Add(frameLab);

            StDevLab = new Label()
            {
                Location = new Point(5, 30),
                Width = 500,
                Text = "StDev ---"
            };
            topPanel.Controls.Add(StDevLab);

            YmaxLab = new Label()
            {
                Location = new Point(5, 60),
                Width = 50,
                Text = "Ymax"
            };
            topPanel.Controls.Add(YmaxLab);

            YmaxTBox = new TextBox()
            {
                Location = new Point(60, 58),
            };
            topPanel.Controls.Add(YmaxTBox);

            tb = new TrackBar()
            {
                Minimum = 0,
                Maximum = 1,
                Value = 0,
                Dock = DockStyle.Bottom,
                TickStyle = TickStyle.None
            };
            myForm.Controls.Add(tb);

            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();

            chartArea1.Name = "ChartArea1";
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MinorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MinorGrid.Enabled = false;
            chartArea1.AxisY.Title = "Concentration [uM]";
            chartArea1.AxisX.Title = "Pixels";
            chartArea1.AxisY.LabelStyle.Format = "#.00E+0";
            chartArea1.AxisX.LabelStyle.Format = "#";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            //this.chart1.Location = new System.Drawing.Point(0, 50);
            this.chart1.Name = "chart1";
            // this.chart1.Size = new System.Drawing.Size(284, 212);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";

            myForm.Controls.Add(chart1);
            chart1.BringToFront();
            //events
            tb.ValueChanged += new EventHandler(delegate (object sender, EventArgs e)
            {
                if (M_profile_fit == null && M_profile_raw == null) return;

                Refresh();

                frameLab.Text = "Frame: " + tb.Value;

                myForm.Update();
                myForm.Invalidate();
                myForm.Refresh();
                Application.DoEvents();
            });

            YmaxTBox.TextChanged += new EventHandler(delegate (object sender, EventArgs e)
            {
                if (M_profile_fit == null && M_profile_raw == null) return;

                Refresh();

                myForm.Update();
                myForm.Invalidate();
                myForm.Refresh();
                Application.DoEvents();
            });
            saveBtn.Click += new EventHandler(delegate (object sender, EventArgs e)
            {
                if (M_profile_fit == null && M_profile_raw == null) return;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save file";
                saveFileDialog1.Filter = "Tab delimited text file(*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.CheckFileExists = false;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ExportFile(saveFileDialog1.FileName);
                }

            });

            videoBtn.Click += new EventHandler(delegate (object sender, EventArgs e)
            {
                if (M_profile_fit == null && M_profile_raw == null) return;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save video file";
                saveFileDialog1.Filter = "TIF file(*.tif)|*.tif";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.CheckFileExists = false;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ExportVideoFile(saveFileDialog1.FileName);
                }
            });

            importBtn.MouseDown += new MouseEventHandler(delegate (object sender, MouseEventArgs e)
            {
                this.ImportFile(e.Button == MouseButtons.Right);
            });

            lb.AfterCheck += new TreeViewEventHandler(delegate (object o, TreeViewEventArgs a)
            {
                if (refreshLB)
                {
                    getChartSerStatus();

                    foreach (var ser in chart1.Series)
                        if (serEnabledStatus.Keys.Contains(ser.Name))
                        {
                            ser.Enabled = serEnabledStatus[ser.Name];
                        }
                }
            });

            SetTrackBarmaximum();

            this.myForm.FormClosing += new FormClosingEventHandler(delegate (object o, FormClosingEventArgs e)
            {
                this.myForm.Hide();
                e.Cancel = true;
            });

            myForm.Show();
            myForm.BringToFront();
        }

        private Bitmap getBitmap(int frame)
        {
            Bitmap bmp = new Bitmap(this.chart1.Width, this.chart1.Height);

            if (frame < this.tb.Minimum || frame > this.tb.Maximum) return bmp;

            this.tb.Value = frame;

            Refresh();

            // frameLab.Text = "Frame: " + tb.Value;

            myForm.Update();
            myForm.Invalidate();
            myForm.Refresh();
            Application.DoEvents();

            this.chart1.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

            return bmp;
        }
        private void ExportVideoFile(string dir)
        {
            try
            {
                Bitmap MasterBitmap = getBitmap(0); //Start page of document(master)
                Image imageAdd = getBitmap(0);  //Frame Image that will be added to the master          
                Guid guid = imageAdd.FrameDimensionsList[0]; //GUID
                FrameDimension dimension = new FrameDimension(guid);

                EncoderParameters ep = new EncoderParameters(1);

                //Get Image Codec Information
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo codecInfo = codecs[3]; //image/tiff

                //MultiFrame Encoding format
                EncoderParameter epMultiFrame = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                ep.Param[0] = epMultiFrame;
                MasterBitmap.SelectActiveFrame(dimension, 0);
                MasterBitmap.Save(dir, codecInfo, ep);//create master document

                //FrameDimensionPage Encoding format
                EncoderParameter epFrameDimensionPage = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                ep.Param[0] = epFrameDimensionPage;

                for (int i = this.tb.Minimum; i <= this.tb.Maximum; i++)
                {
                    imageAdd.SelectActiveFrame(dimension, i);//select next frame
                    MasterBitmap.SaveAdd(getBitmap(i), ep);//add it to the master
                }

                //Flush Encoding format
                EncoderParameter epFlush = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
                ep.Param[0] = epFlush;
                MasterBitmap.SaveAdd(ep); //flush the file                   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ExportFile(string dir)
        {
            if (M_profile_fit != null)
                try
                {
                    using (StreamWriter sw = new StreamWriter(dir))
                    {
                        foreach (var vals in M_profile_fit)
                            if (vals != null)
                                sw.WriteLine(string.Join("\t", vals));

                        sw.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
        }
        public void ImportFile(bool loadOffSet)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse TAB-Delimited Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 0,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImportFile(openFileDialog1.FileName, loadOffSet);

                Refresh();
            }
        }
        private void recalibrateArrays()
        {
            if (M_profile_fit == null || M_profile_raw == null) return;

            if (M_profile_fit[0].Length > M_profile_raw[0].Length)
            {
                for (int i = 0; i < M_profile_raw.Length && i < _M_profile_fit.Length; i++)
                {
                    var new_raw = new double[M_profile_fit[i].Length];
                    int dev = Math.Abs(M_profile_fit[i].Length - M_profile_raw[i].Length) / 2;
                    Array.Copy(M_profile_raw[i], 0, new_raw, dev, M_profile_raw[i].Length);
                    M_profile_raw[i] = new_raw;
                }
            }
            else if (M_profile_fit[0].Length < M_profile_raw[0].Length)
            {
                for (int i = 0; i < M_profile_raw.Length && i < _M_profile_fit.Length; i++)
                {
                    /*
                    //recalibrate fit
                    var new_fit = new double[M_profile_raw[i].Length];
                    int dev = Math.Abs(M_profile_fit[i].Length - M_profile_raw[i].Length) / 2;
                    Array.Copy(M_profile_fit[i], 0, new_fit, dev, M_profile_fit[i].Length);
                    M_profile_fit[i] = new_fit;
                    //recalibrate dmg
                    if (dmg_profile != null)
                    {
                        new_fit = new double[M_profile_raw[i].Length];
                        Array.Copy(dmg_profile[i], 0, new_fit, dev, dmg_profile[i].Length);
                        dmg_profile[i] = new_fit;
                    }
                    */
                    M_profile_fit[i] = RecalibrateArray(M_profile_fit[i], M_profile_raw[i]);
                    dmg_profile[i] = RecalibrateArray(dmg_profile[i], M_profile_raw[i]);
                    AX_PX_profile[i] = RecalibrateArray(AX_PX_profile[i], M_profile_raw[i]);
                    AX_profile[i] = RecalibrateArray(AX_profile[i], M_profile_raw[i]);
                    PX_profile[i] = RecalibrateArray(PX_profile[i], M_profile_raw[i]);
                    X0_T_profile[i] = RecalibrateArray(X0_T_profile[i], M_profile_raw[i]);
                    P_profile[i] = RecalibrateArray(P_profile[i], M_profile_raw[i]);
                    A_P_AX_PX_profile[i] = RecalibrateArray(A_P_AX_PX_profile[i], M_profile_raw[i]);

                    dmg_Ku_profile[i] = RecalibrateArray(dmg_Ku_profile[i], M_profile_raw[i]);
                    AX_Ku_profile[i] = RecalibrateArray(AX_Ku_profile[i], M_profile_raw[i]);
                    X0_Ku_T_profile[i] = RecalibrateArray(X0_Ku_T_profile[i], M_profile_raw[i]);
                    A_AX_Ku_profile[i] = RecalibrateArray(A_AX_Ku_profile[i], M_profile_raw[i]);

                    M_FRAP_profile_fit[i] = RecalibrateArray(M_FRAP_profile_fit[i], M_profile_raw[i]);
                    ATM_FRAP_profile_fit[i] = RecalibrateArray(ATM_FRAP_profile_fit[i], M_profile_raw[i]);
                    ATM_FRAP_Ku_profile_fit[i] = RecalibrateArray(ATM_FRAP_Ku_profile_fit[i], M_profile_raw[i]);
                }
            }

            if (M_profile_raw_OffSet != null)
            {
                if (M_profile_raw_OffSet[0].Length < M_profile_fit[0].Length)
                {
                    for (int i = 0; i < M_profile_raw.Length && i < _M_profile_fit.Length; i++)
                    {
                        var new_raw = new double[M_profile_fit[i].Length];
                        int dev = Math.Abs(M_profile_fit[i].Length - M_profile_raw_OffSet[i].Length) / 2;
                        Array.Copy(M_profile_raw_OffSet[i], 0, new_raw, dev, M_profile_raw_OffSet[i].Length);
                        M_profile_raw_OffSet[i] = new_raw;
                    }
                }
                else
                {
                    for (int i = 0; i < M_profile_raw.Length && i < _M_profile_fit.Length; i++)
                    {
                        var new_raw = new double[M_profile_fit[i].Length];
                        int dev = Math.Abs(M_profile_fit[i].Length - M_profile_raw_OffSet[i].Length) / 2;
                        Array.Copy(M_profile_raw_OffSet[i], dev, new_raw, 0, M_profile_fit[i].Length);
                        M_profile_raw_OffSet[i] = new_raw;
                    }
                }
            }
        }
        private static double[] RecalibrateArray(double[] array1, double[] array2)
        {
            if (null == array1 || null == array1) return null;
            if (array1.Length == array2.Length) return null;

            double[] longer = array1.Length > array2.Length ? array1 : array2;
            double[] shorter = array1.Length < array2.Length ? array1 : array2;

            double[] output = new double[longer.Length];

            int dev = Math.Abs(longer.Length - shorter.Length) / 2;

            Array.Copy(shorter, 0, output, dev, shorter.Length);

            return output;
        }
        private void ImportFile(string dir, bool importOffSet)
        {
            try
            {
                List<double[]> output = new List<double[]>();
                using (StreamReader sr = new StreamReader(dir))
                {
                    string line = sr.ReadLine();
                    string[] input;
                    double[] vals;

                    while (line != null)
                    {
                        input = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                        vals = new double[input.Length];

                        for (int i = 0; i < input.Length; i++)
                            double.TryParse(input[i], out vals[i]);

                        output.Add(vals);

                        line = sr.ReadLine();
                    }

                    if (importOffSet)
                    {
                        this.M_profile_raw_OffSet = output.ToArray();
                        Console.WriteLine("Load offset profile");
                    }
                    else
                    {
                        this.M_profile_raw = output.ToArray();
                        Console.WriteLine("Load profile");
                    }

                    input = null;
                    vals = null;
                    output = null;
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public double getStDev()
        {
            if (M_profile_fit == null || M_profile_raw == null)
            {
                return -1;
            }
            int count = 0;
            double avg = 0;

            for (int i = 0; i < M_profile_fit.Length && i < M_profile_raw.Length; i++)
                if (M_profile_raw[i] != null && _M_profile_fit[i] != null)
                    for (int j = 0; j < M_profile_fit[i].Length && j < M_profile_raw[i].Length; j++)
                        if (M_profile_fit[i][j] != 0 && M_profile_raw[i][j] != 0)
                        {
                            avg += Math.Pow(M_profile_fit[i][j] - M_profile_raw[i][j], 2d);
                            count++;
                        }
            if (count != 0) avg /= count;

            return avg;
        }
        public void Refresh()
        {
            if (this.myForm == null) return;

            SetTrackBarmaximum();

            if (StDevLab != null)
            {
                StDevLab.Text = "StDev " + getStDev();

                if (StDevLab.Text == "StDev -1") StDevLab.Text = "StDev ---";
            }

            int T = tb.Value;

            var ser_Raw = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Raw",
                Color = System.Drawing.Color.Red,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.M_profile_raw, ser_Raw);

            var ser_dmg = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "dmg",
                Color = System.Drawing.Color.Orange,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.dmg_profile, ser_dmg);

            var ser_AX_PX = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "AX_PX",
                Color = System.Drawing.Color.Purple,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.AX_PX_profile, ser_AX_PX);

            var ser_PX = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "PX",
                Color = System.Drawing.Color.Cyan,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.PX_profile, ser_PX);

            var ser_AX = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "AX",
                Color = System.Drawing.Color.Magenta,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.AX_profile, ser_AX);

            var ser_P = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "P",
                Color = System.Drawing.Color.LightBlue,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.P_profile, ser_P);

            var ser_A_P_AX_PX = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "A_P_AX_PX",
                Color = System.Drawing.Color.CornflowerBlue,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.A_P_AX_PX_profile, ser_A_P_AX_PX);


            var ser_X0_T = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "X0_T",
                Color = System.Drawing.Color.DarkGray,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.X0_T_profile, ser_X0_T);


            var ser_Raw_OffSet = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Raw_OffSet",
                Color = System.Drawing.Color.Green,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.M_profile_raw_OffSet, ser_Raw_OffSet);


            var ser_Fit = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "M Fit",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.M_profile_fit, ser_Fit);

            var ser_A_AX_Ku = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "A_AX_Ku",
                Color = System.Drawing.Color.CornflowerBlue,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.A_AX_Ku_profile, ser_A_AX_Ku);

            var ser_AX_Ku = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "AX_Ku",
                Color = System.Drawing.Color.Magenta,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.AX_Ku_profile, ser_AX_Ku);

            var ser_X0_Ku_T = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "X0_T_Ku",
                Color = System.Drawing.Color.DarkGray,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.X0_Ku_T_profile, ser_X0_Ku_T);

            var ser_dmg_Ku = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "dmg_Ku",
                Color = System.Drawing.Color.Orange,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.dmg_Ku_profile, ser_dmg_Ku);

            var ser_M_FRAP = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "M_FRAP",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.M_FRAP_profile_fit, ser_M_FRAP);

            var ser_ATM_FRAP = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "ATM_FRAP",
                Color = System.Drawing.Color.GreenYellow,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.ATM_FRAP_profile_fit, ser_ATM_FRAP);

            var ser_ATM_FRAP_Ku = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "ATM_FRAP_Ku",
                Color = System.Drawing.Color.DarkRed,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline
            };
            PrepareSeries(T, this.ATM_FRAP_Ku_profile_fit, ser_ATM_FRAP_Ku);

            this.chart1.SuspendLayout();

            this.chart1.Series.Clear();

            AddSeries(ser_Raw);
            AddSeries(ser_Raw_OffSet);
            AddSeries(ser_Fit);
            AddSeries(ser_dmg);
            AddSeries(ser_AX_PX);
            AddSeries(ser_AX);
            AddSeries(ser_PX);
            AddSeries(ser_P);
            AddSeries(ser_X0_T);
            AddSeries(ser_A_P_AX_PX);
            AddSeries(ser_A_AX_Ku);
            AddSeries(ser_AX_Ku);
            AddSeries(ser_X0_Ku_T);
            AddSeries(ser_dmg_Ku);
            AddSeries(ser_M_FRAP);
            AddSeries(ser_ATM_FRAP);
            AddSeries(ser_ATM_FRAP_Ku);

            //this.chart1.ChartAreas[0].RecalculateAxesScale();
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = GetMaximum() * 1.1d;
            this.chart1.ChartAreas[0].AxisX.Minimum = 0;
            this.chart1.ChartAreas[0].AxisX.Maximum = Math.Max(ser_Raw.Points.Count, ser_Fit.Points.Count);

            enableChartSeries();
            refreshChartLegendSeries();

            this.chart1.ResumeLayout(false);

            this.chart1.Invalidate();
            this.chart1.Update();
            this.chart1.Refresh();
        }
        private void enableChartSeries()
        {
            refreshLB = false;
            try
            {
                //if (lb.Nodes == null) return;

                foreach (var ser in chart1.Series)
                    if (serEnabledStatus.Keys.Contains(ser.Name))
                    {
                        ser.Enabled = serEnabledStatus[ser.Name];
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            refreshLB = true;
        }
        private void getChartSerStatus()
        {
            try
            {
                //if (lb.Nodes == null) return;

                foreach (TreeNode ser in lb.Nodes)
                    if (serEnabledStatus.Keys.Contains(ser.Text))
                    {
                        serEnabledStatus[ser.Text] = ser.Checked;
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void refreshChartLegendSeries()
        {
            lb.SuspendLayout();
            refreshLB = false;
            lb.Nodes.Clear();

            try
            {
                foreach (var ser in serEnabledStatus)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = ser.Key;
                    tn.Checked = ser.Value;
                    lb.Nodes.Add(tn);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            lb.ResumeLayout(true);

            refreshLB = true;
        }
        private void AddSeries(Series ser)
        {
            if (ser.Points.Count > 0)
            {
                this.chart1.Series.Add(ser);

                if (!serEnabledStatus.Keys.Contains(ser.Name))
                    serEnabledStatus.Add(ser.Name, true);
            }
            else
            {
                if (serEnabledStatus.Keys.Contains(ser.Name))
                    serEnabledStatus.Remove(ser.Name);
            }
        }
        private double GetMaximum()
        {
            List<double> vals = new List<double>();

            vals.Add(GetMaximum(this.M_profile_raw));
            vals.Add(GetMaximum(this.M_profile_fit));
            vals.Add(GetMaximum(this.dmg_profile));
            vals.Add(GetMaximum(this.AX_PX_profile));
            vals.Add(GetMaximum(this.PX_profile));
            vals.Add(GetMaximum(this.AX_profile));
            vals.Add(GetMaximum(this.X0_T_profile));
            vals.Add(GetMaximum(this.P_profile));
            vals.Add(GetMaximum(this.A_P_AX_PX_profile));
            vals.Add(GetMaximum(this.dmg_Ku_profile));
            vals.Add(GetMaximum(this.A_AX_Ku_profile));
            vals.Add(GetMaximum(this.AX_Ku_profile));
            vals.Add(GetMaximum(this.X0_Ku_T_profile));
            vals.Add(GetMaximum(this.M_FRAP_profile_fit));
            vals.Add(GetMaximum(this.ATM_FRAP_profile_fit));
            vals.Add(GetMaximum(this.ATM_FRAP_Ku_profile_fit));

            double max = 0;
            if (YmaxTBox.Text != "" && double.TryParse(YmaxTBox.Text, out max) && max > 0.0d)
            {
                return max;
            }
            else
            {
                return vals.Max();
            }
        }
        private double GetMaximum(double[][] input)
        {
            if (input == null) return 0;
            double max = 0;
            double localMax = 0;

            foreach (var vals in input)
                if (vals != null)
                {
                    localMax = vals.Max();
                    if (max < localMax) max = localMax;
                }

            return max;
        }
        private void PrepareSeries(int T, double[][] input,
            System.Windows.Forms.DataVisualization.Charting.Series ser)
        {
            if (input == null || input.Length <= T || input[T] == null)
                return;

            double[] data = input[T];

            for (int i = 0; i < data.Length; i++)
                ser.Points.AddXY(i, data[i]);

            ser.BorderWidth = 3;

        }
        private void SetTrackBarmaximum()
        {
            if (this.myForm == null) return;

            int max = M_profile_fit != null ? M_profile_fit.Length : 0;
            max = M_profile_raw != null && M_profile_raw.Length > max ? M_profile_raw.Length : max;

            if (tb.Value >= max) tb.Value = max - 1;
            tb.Maximum = max;
        }

    }
}
