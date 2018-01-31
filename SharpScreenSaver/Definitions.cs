using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;

namespace SharpScreenSaver
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


		[FlagsAttribute]
		public enum EXECUTION_STATE : uint
		{
			ES_AWAYMODE_REQUIRED = 0x00000040,
			ES_CONTINUOUS = 0x80000000,
			ES_DISPLAY_REQUIRED = 0x00000002,
			ES_SYSTEM_REQUIRED = 0x00000001
			// Legacy flag, should not be used.
			// ES_USER_PRESENT = 0x00000004
		}
	}
}
