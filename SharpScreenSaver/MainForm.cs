﻿using System.Windows.Forms;
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
		}

		void InitTimer_Tick(object sender, EventArgs e)
		{
			if (CurrentIndex < ThreadPoolSize)
			{
				Panel pnl = new Panel();
				Color color = Color.FromArgb(rd.Next(0, 255), rd.Next(0, 255), rd.Next(0, 255));
				pnl.BackColor = color;
				pnl.Dock = DockStyle.Fill;
				this.tblLayout.Controls.Add(pnl);
				CurrentIndex += 1;
			}
			else
			{
				InitTimer.Enabled = false;
			}
		}

		void MainForm_Load(object sender, System.EventArgs e)
		{
			this.InitTimer.Enabled = true;
		}

		void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.Close();
		}

		void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			
		}
	}
}
