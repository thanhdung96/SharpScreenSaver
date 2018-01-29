using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace SharpScreenSaver
{
	public partial class MainForm
	{
		private const int ThreadPoolSize = 16;
		private const int Dimension = 4;
		private int CurrentIndex = 0;
		Random rd = new Random();

		private List<Thread> ThreadPool = new List<Thread>();
		private List<Panel> PanelPool = new List<Panel>();
	}
}
