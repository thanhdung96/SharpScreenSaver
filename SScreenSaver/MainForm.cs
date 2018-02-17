using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SScreenSaver
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
					/*tblLayout.GetControlFromPosition(i % DIMENSION, i / DIMENSION).BackColor = Color.FromArgb(rd.Next(MIN_COLOR, MAX_COLOR), rd.Next(MIN_COLOR, MAX_COLOR), rd.Next(MIN_COLOR, MAX_COLOR));
					PanelDelay[i] = (byte)(rd.Next(MIN_DELAY, MAX_DELAY));*/
					Thread t = new Thread(new ParameterizedThreadStart(PanelTransition));
					t.Start(i as object);
					Thread.Sleep(10);
				}
			}
		}
		#endregion Timer Tick Event

		#region Threading
		private void PanelTransition(object PanelIndex)
		{
			int i = (int)PanelIndex;
			lock (tblLayout.GetControlFromPosition(i % DIMENSION, i / DIMENSION))
			{
				Random RandomColor = new Random();
				Color OldColor = tblLayout.GetControlFromPosition(i % DIMENSION, i / DIMENSION).BackColor;
				Color NewColor = Color.FromArgb(RandomColor.Next(MIN_COLOR, MAX_COLOR), RandomColor.Next(MIN_COLOR, MAX_COLOR), RandomColor.Next(MIN_COLOR, MAX_COLOR));
				List<Color> RGBLerp = RgbLinearInterpolate(OldColor, NewColor, 24);

				foreach (Color color in RGBLerp)
				{
					tblLayout.GetControlFromPosition(i % DIMENSION, i / DIMENSION).BackColor = color;
					Thread.Sleep(60);
				}
				RGBLerp=null;
				PanelDelay[i] = (byte)(RandomColor.Next(MIN_DELAY, MAX_DELAY));
			}
		}
		public List<Color> RgbLinearInterpolate(Color start, Color end, int colorCount)
		{
			List<Color> ret = new List<Color>();

			// linear interpolation lerp (r,a,b) = (1-r)*a + r*b = (1-r)*(ax,ay,az) + r*(bx,by,bz)
			for (int n = 0; n < colorCount; n++)
			{
				double r = (double)n / (double)(colorCount - 1);
				double nr = 1.0 - r;
				double A = (nr * start.A) + (r * end.A);
				double R = (nr * start.R) + (r * end.R);
				double G = (nr * start.G) + (r * end.G);
				double B = (nr * start.B) + (r * end.B);

				ret.Add(Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B));
			}

			return ret;
		}
		#endregion Threading
	}
}
