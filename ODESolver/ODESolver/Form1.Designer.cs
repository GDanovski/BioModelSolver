namespace ODESolver
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.licenseAgreementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBox_Log = new System.Windows.Forms.RichTextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.button_Solve = new System.Windows.Forms.Button();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel_Variables = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.checkBox_ShowConst = new System.Windows.Forms.CheckBox();
            this.button_Stop = new System.Windows.Forms.Button();
            this.button_Refresh = new System.Windows.Forms.Button();
            this.checkBox_AutoRefresh = new System.Windows.Forms.CheckBox();
            this.label_Counter = new System.Windows.Forms.Label();
            this.button_Info = new System.Windows.Forms.Button();
            this.textBox_Iterations = new System.Windows.Forms.TextBox();
            this.comboBox_Models = new System.Windows.Forms.ComboBox();
            this.button_Load = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel_OpenGL = new System.Windows.Forms.Panel();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Legend_TreeView = new System.Windows.Forms.TreeView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button_Profile = new System.Windows.Forms.Button();
            this.button_Video = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_StDevScale = new System.Windows.Forms.TextBox();
            this.textBox_YMax = new System.Windows.Forms.TextBox();
            this.label_YMax = new System.Windows.Forms.Label();
            this.label_Dir = new System.Windows.Forms.Label();
            this.label_StDev = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel11.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1270, 43);
            this.panel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exportChartToolStripMenuItem,
            this.exportDataToolStripMenuItem,
            this.licenseAgreementToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1270, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(147, 29);
            this.openToolStripMenuItem.Text = "Open Variables";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(140, 29);
            this.saveToolStripMenuItem.Text = "Save Variables";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exportChartToolStripMenuItem
            // 
            this.exportChartToolStripMenuItem.Name = "exportChartToolStripMenuItem";
            this.exportChartToolStripMenuItem.Size = new System.Drawing.Size(126, 29);
            this.exportChartToolStripMenuItem.Text = "Export Chart";
            this.exportChartToolStripMenuItem.Click += new System.EventHandler(this.exportChartToolStripMenuItem_Click);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(121, 29);
            this.exportDataToolStripMenuItem.Text = "Export Data";
            this.exportDataToolStripMenuItem.Click += new System.EventHandler(this.exportDataToolStripMenuItem_Click);
            // 
            // licenseAgreementToolStripMenuItem
            // 
            this.licenseAgreementToolStripMenuItem.Name = "licenseAgreementToolStripMenuItem";
            this.licenseAgreementToolStripMenuItem.Size = new System.Drawing.Size(177, 29);
            this.licenseAgreementToolStripMenuItem.Text = "License Agreement";
            this.licenseAgreementToolStripMenuItem.Click += new System.EventHandler(this.licenseAgreementToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 1000);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1270, 52);
            this.panel2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(981, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(266, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Copyright (C) 2024  Georgi Danovski";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.splitter2);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 43);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(560, 957);
            this.panel3.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.richTextBox_Log);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 884);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(560, 73);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log:";
            // 
            // richTextBox_Log
            // 
            this.richTextBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Log.Location = new System.Drawing.Point(4, 24);
            this.richTextBox_Log.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.richTextBox_Log.Name = "richTextBox_Log";
            this.richTextBox_Log.ReadOnly = true;
            this.richTextBox_Log.Size = new System.Drawing.Size(552, 44);
            this.richTextBox_Log.TabIndex = 0;
            this.richTextBox_Log.Text = "";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button_Solve);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 807);
            this.panel6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(560, 77);
            this.panel6.TabIndex = 2;
            // 
            // button_Solve
            // 
            this.button_Solve.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Solve.Location = new System.Drawing.Point(22, 22);
            this.button_Solve.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Solve.Name = "button_Solve";
            this.button_Solve.Size = new System.Drawing.Size(513, 35);
            this.button_Solve.TabIndex = 0;
            this.button_Solve.Text = "Solve model";
            this.button_Solve.UseVisualStyleBackColor = true;
            this.button_Solve.Click += new System.EventHandler(this.button_Solve_Click);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 802);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(560, 5);
            this.splitter2.TabIndex = 4;
            this.splitter2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel_Variables);
            this.groupBox1.Controls.Add(this.panel7);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 228);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(560, 574);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variables";
            // 
            // panel_Variables
            // 
            this.panel_Variables.AutoScroll = true;
            this.panel_Variables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Variables.Location = new System.Drawing.Point(4, 79);
            this.panel_Variables.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel_Variables.Name = "panel_Variables";
            this.panel_Variables.Size = new System.Drawing.Size(552, 490);
            this.panel_Variables.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label7);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(4, 24);
            this.panel7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(552, 55);
            this.panel7.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(375, 17);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 20);
            this.label7.TabIndex = 3;
            this.label7.Text = "Maximum";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 18);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Minimum";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 17);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "Value";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 17);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Name";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkBox_ShowConst);
            this.panel4.Controls.Add(this.button_Stop);
            this.panel4.Controls.Add(this.button_Refresh);
            this.panel4.Controls.Add(this.checkBox_AutoRefresh);
            this.panel4.Controls.Add(this.label_Counter);
            this.panel4.Controls.Add(this.button_Info);
            this.panel4.Controls.Add(this.textBox_Iterations);
            this.panel4.Controls.Add(this.comboBox_Models);
            this.panel4.Controls.Add(this.button_Load);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(560, 228);
            this.panel4.TabIndex = 0;
            // 
            // checkBox_ShowConst
            // 
            this.checkBox_ShowConst.AutoSize = true;
            this.checkBox_ShowConst.Location = new System.Drawing.Point(22, 188);
            this.checkBox_ShowConst.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_ShowConst.Name = "checkBox_ShowConst";
            this.checkBox_ShowConst.Size = new System.Drawing.Size(184, 24);
            this.checkBox_ShowConst.TabIndex = 10;
            this.checkBox_ShowConst.Text = "Show const variables";
            this.checkBox_ShowConst.UseVisualStyleBackColor = true;
            this.checkBox_ShowConst.CheckedChanged += new System.EventHandler(this.checkBox_ShowConst_CheckedChanged);
            // 
            // button_Stop
            // 
            this.button_Stop.Location = new System.Drawing.Point(339, 166);
            this.button_Stop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(112, 35);
            this.button_Stop.TabIndex = 9;
            this.button_Stop.Text = "Stop";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // button_Refresh
            // 
            this.button_Refresh.Location = new System.Drawing.Point(218, 164);
            this.button_Refresh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Refresh.Name = "button_Refresh";
            this.button_Refresh.Size = new System.Drawing.Size(112, 35);
            this.button_Refresh.TabIndex = 8;
            this.button_Refresh.Text = "Refresh";
            this.button_Refresh.UseVisualStyleBackColor = true;
            this.button_Refresh.Click += new System.EventHandler(this.button_Refresh_Click);
            // 
            // checkBox_AutoRefresh
            // 
            this.checkBox_AutoRefresh.AutoSize = true;
            this.checkBox_AutoRefresh.Location = new System.Drawing.Point(22, 152);
            this.checkBox_AutoRefresh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_AutoRefresh.Name = "checkBox_AutoRefresh";
            this.checkBox_AutoRefresh.Size = new System.Drawing.Size(123, 24);
            this.checkBox_AutoRefresh.TabIndex = 7;
            this.checkBox_AutoRefresh.Text = "Auto refresh";
            this.checkBox_AutoRefresh.UseVisualStyleBackColor = true;
            this.checkBox_AutoRefresh.CheckedChanged += new System.EventHandler(this.checkBox_AutoRefresh_CheckedChanged);
            // 
            // label_Counter
            // 
            this.label_Counter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Counter.AutoSize = true;
            this.label_Counter.Location = new System.Drawing.Point(340, 111);
            this.label_Counter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Counter.Name = "label_Counter";
            this.label_Counter.Size = new System.Drawing.Size(51, 20);
            this.label_Counter.TabIndex = 6;
            this.label_Counter.Text = "label8";
            this.label_Counter.Visible = false;
            // 
            // button_Info
            // 
            this.button_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Info.Location = new System.Drawing.Point(486, 60);
            this.button_Info.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Info.Name = "button_Info";
            this.button_Info.Size = new System.Drawing.Size(64, 35);
            this.button_Info.TabIndex = 5;
            this.button_Info.Text = "Info";
            this.button_Info.UseVisualStyleBackColor = true;
            this.button_Info.Click += new System.EventHandler(this.button_Info_Click);
            // 
            // textBox_Iterations
            // 
            this.textBox_Iterations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Iterations.Location = new System.Drawing.Point(182, 106);
            this.textBox_Iterations.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_Iterations.Name = "textBox_Iterations";
            this.textBox_Iterations.Size = new System.Drawing.Size(148, 26);
            this.textBox_Iterations.TabIndex = 4;
            this.textBox_Iterations.Text = "1000";
            // 
            // comboBox_Models
            // 
            this.comboBox_Models.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Models.FormattingEnabled = true;
            this.comboBox_Models.Location = new System.Drawing.Point(182, 62);
            this.comboBox_Models.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox_Models.Name = "comboBox_Models";
            this.comboBox_Models.Size = new System.Drawing.Size(292, 28);
            this.comboBox_Models.TabIndex = 3;
            this.comboBox_Models.SelectedIndexChanged += new System.EventHandler(this.comboBox_Models_SelectedIndexChanged);
            // 
            // button_Load
            // 
            this.button_Load.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Load.Location = new System.Drawing.Point(20, 11);
            this.button_Load.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Load.Name = "button_Load";
            this.button_Load.Size = new System.Drawing.Size(516, 35);
            this.button_Load.TabIndex = 2;
            this.button_Load.Text = "Load data file";
            this.button_Load.UseVisualStyleBackColor = true;
            this.button_Load.Click += new System.EventHandler(this.button_Load_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 114);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of iterations:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 66);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select model:";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.panel_OpenGL);
            this.panel11.Controls.Add(this.splitter3);
            this.panel11.Controls.Add(this.groupBox3);
            this.panel11.Controls.Add(this.panel5);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(560, 43);
            this.panel11.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(710, 957);
            this.panel11.TabIndex = 3;
            // 
            // panel_OpenGL
            // 
            this.panel_OpenGL.BackColor = System.Drawing.Color.White;
            this.panel_OpenGL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_OpenGL.Location = new System.Drawing.Point(0, 152);
            this.panel_OpenGL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel_OpenGL.Name = "panel_OpenGL";
            this.panel_OpenGL.Size = new System.Drawing.Size(456, 805);
            this.panel_OpenGL.TabIndex = 2;
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter3.Location = new System.Drawing.Point(456, 152);
            this.splitter3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(4, 805);
            this.splitter3.TabIndex = 3;
            this.splitter3.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Legend_TreeView);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox3.Location = new System.Drawing.Point(460, 152);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(250, 805);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Legend:";
            // 
            // Legend_TreeView
            // 
            this.Legend_TreeView.BackColor = System.Drawing.SystemColors.Control;
            this.Legend_TreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Legend_TreeView.CheckBoxes = true;
            this.Legend_TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Legend_TreeView.Location = new System.Drawing.Point(4, 24);
            this.Legend_TreeView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Legend_TreeView.Name = "Legend_TreeView";
            this.Legend_TreeView.ShowLines = false;
            this.Legend_TreeView.ShowPlusMinus = false;
            this.Legend_TreeView.ShowRootLines = false;
            this.Legend_TreeView.Size = new System.Drawing.Size(242, 776);
            this.Legend_TreeView.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.button_Profile);
            this.panel5.Controls.Add(this.button_Video);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.textBox_StDevScale);
            this.panel5.Controls.Add(this.textBox_YMax);
            this.panel5.Controls.Add(this.label_YMax);
            this.panel5.Controls.Add(this.label_Dir);
            this.panel5.Controls.Add(this.label_StDev);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(710, 152);
            this.panel5.TabIndex = 1;
            // 
            // button_Profile
            // 
            this.button_Profile.Location = new System.Drawing.Point(12, 104);
            this.button_Profile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Profile.Name = "button_Profile";
            this.button_Profile.Size = new System.Drawing.Size(73, 35);
            this.button_Profile.TabIndex = 7;
            this.button_Profile.Text = "Profile";
            this.button_Profile.UseVisualStyleBackColor = true;
            this.button_Profile.Click += new System.EventHandler(this.button_Profile_Click);
            // 
            // button_Video
            // 
            this.button_Video.Location = new System.Drawing.Point(12, 62);
            this.button_Video.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Video.Name = "button_Video";
            this.button_Video.Size = new System.Drawing.Size(73, 35);
            this.button_Video.TabIndex = 6;
            this.button_Video.Text = "Video";
            this.button_Video.UseVisualStyleBackColor = true;
            this.button_Video.Click += new System.EventHandler(this.button_Video_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(456, 75);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 20);
            this.label8.TabIndex = 5;
            this.label8.Text = "X";
            // 
            // textBox_StDevScale
            // 
            this.textBox_StDevScale.Location = new System.Drawing.Point(486, 70);
            this.textBox_StDevScale.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_StDevScale.Name = "textBox_StDevScale";
            this.textBox_StDevScale.Size = new System.Drawing.Size(148, 26);
            this.textBox_StDevScale.TabIndex = 4;
            // 
            // textBox_YMax
            // 
            this.textBox_YMax.Location = new System.Drawing.Point(168, 107);
            this.textBox_YMax.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_YMax.Name = "textBox_YMax";
            this.textBox_YMax.Size = new System.Drawing.Size(148, 26);
            this.textBox_YMax.TabIndex = 3;
            this.textBox_YMax.Visible = false;
            this.textBox_YMax.TextChanged += new System.EventHandler(this.textBox_YMax_TextChanged);
            // 
            // label_YMax
            // 
            this.label_YMax.AutoSize = true;
            this.label_YMax.Location = new System.Drawing.Point(106, 111);
            this.label_YMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_YMax.Name = "label_YMax";
            this.label_YMax.Size = new System.Drawing.Size(57, 20);
            this.label_YMax.TabIndex = 2;
            this.label_YMax.Text = "Y Max:";
            this.label_YMax.Visible = false;
            // 
            // label_Dir
            // 
            this.label_Dir.AutoSize = true;
            this.label_Dir.Location = new System.Drawing.Point(22, 18);
            this.label_Dir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Dir.Name = "label_Dir";
            this.label_Dir.Size = new System.Drawing.Size(42, 20);
            this.label_Dir.TabIndex = 1;
            this.label_Dir.Text = "File: ";
            this.label_Dir.Visible = false;
            // 
            // label_StDev
            // 
            this.label_StDev.AutoSize = true;
            this.label_StDev.Location = new System.Drawing.Point(102, 69);
            this.label_StDev.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_StDev.Name = "label_StDev";
            this.label_StDev.Size = new System.Drawing.Size(61, 20);
            this.label_StDev.TabIndex = 0;
            this.label_StDev.Text = "StDev: ";
            this.label_StDev.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "TAB-delimited text files| *.txt| CSV files|*.csv";
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.Control;
            this.splitter1.Location = new System.Drawing.Point(560, 43);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 957);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 1052);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1039, 739);
            this.Name = "Form1";
            this.Text = "BioModelSolver";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel_OpenGL;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel_Variables;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox richTextBox_Log;
        private System.Windows.Forms.Button button_Load;
        private System.Windows.Forms.TextBox textBox_Iterations;
        private System.Windows.Forms.ComboBox comboBox_Models;
        private System.Windows.Forms.Button button_Solve;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_StDev;
        private System.Windows.Forms.Button button_Info;
        private System.Windows.Forms.Label label_Dir;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_Counter;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportChartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private System.Windows.Forms.Label label_YMax;
        private System.Windows.Forms.TextBox textBox_YMax;
        private System.Windows.Forms.Button button_Refresh;
        private System.Windows.Forms.CheckBox checkBox_AutoRefresh;
        private System.Windows.Forms.ToolStripMenuItem licenseAgreementToolStripMenuItem;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_StDevScale;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView Legend_TreeView;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.Button button_Profile;
        private System.Windows.Forms.Button button_Video;
        private System.Windows.Forms.CheckBox checkBox_ShowConst;
    }
}

