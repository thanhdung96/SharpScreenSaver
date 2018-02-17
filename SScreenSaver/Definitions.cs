using System.Collections.Generic;
using System;

namespace SScreenSaver
{
	public partial class MainForm
	{
		private const int TOTAL_PANELS = 16;
		private const int DIMENSION = 4;
		private const int MAX_DELAY = 5;
		private const int MIN_DELAY = 2;
		private const int MAX_COLOR = 255;
		private const int MIN_COLOR = 0;
		private int CurrentIndex = 0;
		Random rd = new Random();

		private List<byte> PanelDelay = new List<byte>();

		public TimeSpan TimeoutToHide { get; private set; }
		public DateTime LastMouseMove { get; private set; }
		public bool IsHidden { get; private set; }
	}
}
