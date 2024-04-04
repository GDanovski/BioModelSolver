using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ODESolver
{
    class Panel_Variable : Panel
    {
        public bool SetAutoRefresh
        {
            set
            {
                this.Tag = value;
            }
        }
        #region Variables

        private ODEModel.Variable _variable;
        private Label _NameLab;
        private ToolTip _NameToolTip;
        private TextBox _ValueTB;
        private TextBox _MaxTB;
        private TextBox _MinTB;

        #endregion Variables

        #region Events

        public event EventHandler ReloadChartNeeded;
        public delegate void ReloadChartEventHandler(object sender, EventArgs e);
        protected virtual void ReloadingChartNeeded(EventArgs e)
        {
            EventHandler handler = ReloadChartNeeded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion Events

        #region Properties

        public ODEModel.Variable Variable
        {
            get { return this._variable; }
            set { this._variable = value; }
        }
        public string ConstName
        {
            get { return this.Variable.Name; }
            set { this.Variable.Name = value; }
        }
        public double ConstValue
        {
            get { return this.Variable.Value; }
            set { this.Variable.Value = value; }
        }
        public double ConstMin
        {
            get { return this.Variable.Minimum; }
            set { this.Variable.Minimum = value; }
        }
        public double ConstMax
        {
            get { return this.Variable.Maximum; }
            set { this.Variable.Maximum = value; }
        }
        #endregion Properties


        public void RefreshValues()
        {
            this._NameLab.Text = this.Variable.DisplayName;
            this._NameToolTip.SetToolTip(this._NameLab, this.Variable.DisplayName);
            this._ValueTB.Text = this.Variable.Value.ToString();
            this._MaxTB.Text = this.Variable.Maximum.ToString();
            this._MinTB.Text = this.Variable.Minimum.ToString();
        }
        public void AssignValues()
        {
            double value, Minimum, Maximum;

            double.TryParse(this._ValueTB.Text, out value);
            double.TryParse(this._MaxTB.Text, out Maximum);
            double.TryParse(this._MinTB.Text, out Minimum);

            this.Variable.Value = value;
            this.Variable.Maximum = Maximum;
            this.Variable.Minimum = Minimum;
        }
        public Panel_Variable(ODEModel.Variable Variable, bool autoRefresh)
        {
            this.SetAutoRefresh = autoRefresh;
            this.Variable = Variable;

            this.SuspendLayout();
            this.Dock = DockStyle.Top;
            this.Height = 30;

            //add controls
            this._NameLab = new Label();
            this._NameLab.Location = new Point(5, 7);
            this._NameLab.Width = 45;
            this.Controls.Add(this._NameLab);

            this._NameToolTip = new ToolTip();
            this._NameToolTip.InitialDelay = 500;
            this._NameToolTip.ReshowDelay = 100;
            this._NameToolTip.AutoPopDelay = 5000;
            this._NameToolTip.SetToolTip(this._NameLab, this._NameLab.Text);

            this._ValueTB = new TextBox();
            this._ValueTB.Location = new Point(50, 5);
            this._ValueTB.Width = 95;
            this.Controls.Add(this._ValueTB);

            this._MinTB = new TextBox();
            this._MinTB.Location = new Point(150, 5);
            this._MinTB.Width = 95;
            this.Controls.Add(this._MinTB);

            this._MaxTB = new TextBox();
            this._MaxTB.Location = new Point(250, 5);
            this._MaxTB.Width = 95;
            this.Controls.Add(this._MaxTB);

            this.Resize += panel_Resize;

            RefreshValues();
            //add handlers
            this.Disposed += OnDisposing;

            _ValueTB.LostFocus += Text_Changed;
            _MaxTB.LostFocus += Text_Changed;
            _MinTB.LostFocus += Text_Changed;

            _ValueTB.KeyDown += Text_KeyDown;
            _MaxTB.KeyDown += Text_KeyDown;
            _MinTB.KeyDown += Text_KeyDown;

            OnResizing();
            this.ResumeLayout();
        }
        private void OnDisposing(object sender, EventArgs e)
        {
            Close();
        }
        public void Close()
        {
            _NameLab = null;
            _ValueTB = null;
            _MaxTB = null;
            _MinTB = null;
        }
        private void Text_Changed(object sender, EventArgs e)
        {
            if (!(bool)this.Tag) return;

            AssignValues();
            RefreshValues();
            ReloadingChartNeeded(new EventArgs());
        }
        private void Text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (!(bool)this.Tag) return;

            AssignValues();
            RefreshValues();
            ReloadingChartNeeded(new EventArgs());

            //e.SuppressKeyPress = true;
            //e.Handled = true;
        }
        private void panel_Resize(object sender,EventArgs e)
        {
            OnResizing();
        }
        private void OnResizing()
        {
            int distance = 5;
            int position = distance;
            int step = (this.Width - 2* distance) / 7;
            int width = step;

            this._NameLab.Location = new Point(position, 5);
            this._NameLab.Width = width;
            position += (width + distance);

            width = 2 * step;

            this._ValueTB.Location = new Point(position, 5);
            this._ValueTB.Width = width;
            position += (width + distance);

            this._MinTB.Location = new Point(position, 5);
            this._MinTB.Width = width;
            position += (width + distance);

            this._MaxTB.Location = new Point(position, 5);
            this._MaxTB.Width = width;
            position += (width + distance);
        }
    }
    
}

