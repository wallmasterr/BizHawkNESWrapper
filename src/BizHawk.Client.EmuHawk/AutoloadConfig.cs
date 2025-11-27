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
	}
}

