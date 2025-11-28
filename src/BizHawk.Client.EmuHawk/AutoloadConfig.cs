using System.Collections.Generic;

namespace BizHawk.Client.EmuHawk
{
	/// <summary>
	/// Configuration for auto-loading ROMs on startup
	/// </summary>
	public class AutoloadConfig
	{
		/// <summary>
		/// Whether auto-load is enabled
		/// </summary>
		public bool Enabled { get; set; } = false;

		/// <summary>
		/// Path to the ROM file to auto-load (relative to EmuHawk.exe directory or absolute path)
		/// </summary>
		public string RomPath { get; set; } = string.Empty;

		/// <summary>
		/// Whether to start in fullscreen mode when auto-loading
		/// </summary>
		public bool Fullscreen { get; set; } = false;

		/// <summary>
		/// Whether to hide the status bar when auto-loading
		/// </summary>
		public bool HideStatusBar { get; set; } = false;

		/// <summary>
		/// Path to background image file (relative to EmuHawk.exe directory or absolute path)
		/// If empty, no background image will be used
		/// </summary>
		public string BackgroundImagePath { get; set; } = string.Empty;

		/// <summary>
		/// List of available background image paths (relative to EmuHawk.exe directory or absolute paths)
		/// </summary>
		public List<string> BackgroundImages { get; set; } = new List<string>();

		/// <summary>
		/// Index of the currently selected background image in BackgroundImages list
		/// </summary>
		public int CurrentBackgroundIndex { get; set; } = 0;

		/// <summary>
		/// Whether to enable auto-save to slot 9 when tile data matches
		/// </summary>
		public bool AutoSaveEnabled { get; set; } = true;

		/// <summary>
		/// Whether to enable auto-load of the most recent save state on game launch
		/// </summary>
		public bool AutoLoadEnabled { get; set; } = true;
	}
}

