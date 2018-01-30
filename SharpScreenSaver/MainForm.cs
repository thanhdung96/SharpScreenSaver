using System.Windows.Forms;
using System.Threading;
using System;
using System.Drawing;

namespace SharpScreenSaver
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			this.Load += MainForm_Load;
			this.KeyDown += MainForm_KeyDown;
			this.FormClosing += MainForm_FormClosing;

			this.InitTimer.Tick += InitTimer_Tick;
			this.HideMouseTimer.Tick += HideMouseTimer_Tick;
			this.EditPanelTimer.Tick += EditPanelTimer_Tick;

			this.Shown += MainForm_Shown;

			TimeoutToHide = TimeSpan.FromSeconds(5);
			this.MouseMove += MainForm_MouseMove;
		}

		#region Hide mouse
		void HideMouseTimer_Tick(object sender, EventArgs e)
		{
			TimeSpan elaped = DateTime.Now - LastMouseMove;
			if (elaped >= TimeoutToHide && !IsHidden)
			{
				Cursor.Hide();
				IsHidden = true;
			}
		}

		void MainForm_MouseMove(object sender, MouseEventArgs e)
		{
			LastMouseMove = DateTime.Now;

			if (IsHidden)
			{
				Cursor.Show();
				IsHidden = false;
			}
		}
		#endregion Hide mouse

		#region Form Events
		void MainForm_Load(object sender, System.EventArgs e)
		{
			this.InitTimer.Enabled = true;
		}

		void MainForm_Shown(object sender, EventArgs e)
		{
			this.HideMouseTimer.Enabled = true;
		}

		void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.Close();
		}

		void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.InitTimer.Enabled = false;
			this.HideMouseTimer.Enabled = false;
			this.EditPanelTimer.Enabled = false;
		}
		#endregion Form Events

		#region Timer Tick Event
		void InitTimer_Tick(object sender, EventArgs e)
		{
			if (CurrentIndex < TOTAL_PANELS)
			{
				Panel pnl = new Panel();
				Color color = Color.FromArgb(rd.Next(MIN_COLOR, MAX_COLOR), rd.Next(MIN_COLOR, MAX_COLOR), rd.Next(MIN_COLOR, MAX_COLOR));
				pnl.BackColor = color;
				pnl.Dock = DockStyle.Fill;
				pnl.Margin = new Padding(0);
				this.tblLayout.Controls.Add(pnl);
				PanelDelay.Add((byte)(rd.Next(MIN_DELAY, MAX_DELAY)));
				CurrentIndex += 1;
			}
			else
			{
				this.EditPanelTimer.Enabled = true;
				this.InitTimer.Enabled = false;
			}
		}

		void EditPanelTimer_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < TOTAL_PANELS; i++)
			{
				PanelDelay[i]--;
			}
			for (int i = 0; i < TOTAL_PANELS; i++)
			{
				if (PanelDelay[i] == 0)
				{
					tblLayout.GetControlFromPosition(i % DIMENSION, i / DIMENSION).BackColor = Color.FromArgb(rd.Next(MIN_COLOR, MAX_COLOR), rd.Next(MIN_COLOR, MAX_COLOR), rd.Next(MIN_COLOR, MAX_COLOR));
					PanelDelay[i] = (byte)(rd.Next(MIN_DELAY, MAX_DELAY));
				}
			}
		}
		#endregion Timer Tick Event
	}
}
