using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BizHawk.Common.PathExtensions;

namespace BizHawk.Client.EmuHawk
{
	/// <summary>
	/// Simple dialog for selecting background images
	/// </summary>
	public partial class BackgroundSelectionDialog : Form
	{
		private readonly AutoloadConfig _config;
		private readonly string _autoloadPath;
		private ListBox _listBox;
		private Button _okButton;
		private Button _cancelButton;
		private Label _infoLabel;

		public BackgroundSelectionDialog(AutoloadConfig config, string autoloadPath)
		{
			_config = config;
			_autoloadPath = autoloadPath;
			InitializeComponent();
			LoadBackgrounds();
		}

		private void InitializeComponent()
		{
			Text = "Select Background Image";
			Size = new Size(400, 300);
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;

			_infoLabel = new Label
			{
				Text = "Available Background Images:",
				Location = new Point(12, 12),
				Size = new Size(360, 20),
				AutoSize = false
			};

			_listBox = new ListBox
			{
				Location = new Point(12, 40),
				Size = new Size(360, 180),
				SelectionMode = SelectionMode.One
			};

			_okButton = new Button
			{
				Text = "OK",
				DialogResult = DialogResult.OK,
				Location = new Point(216, 230),
				Size = new Size(75, 23)
			};

			_cancelButton = new Button
			{
				Text = "Cancel",
				DialogResult = DialogResult.Cancel,
				Location = new Point(297, 230),
				Size = new Size(75, 23)
			};

			Controls.Add(_infoLabel);
			Controls.Add(_listBox);
			Controls.Add(_okButton);
			Controls.Add(_cancelButton);

			AcceptButton = _okButton;
			CancelButton = _cancelButton;
		}

		private void LoadBackgrounds()
		{
			_listBox.Items.Clear();
			
			// Add "None" option
			_listBox.Items.Add("(None)");
			
			// Ensure BackgroundImages list exists
			if (_config.BackgroundImages == null)
			{
				_config.BackgroundImages = new List<string>();
			}
			
			// Add all background images from config
			if (_config.BackgroundImages.Count > 0)
			{
				foreach (var bgPath in _config.BackgroundImages)
				{
					if (string.IsNullOrEmpty(bgPath))
						continue;
					var displayName = Path.GetFileName(bgPath);
					if (string.IsNullOrEmpty(displayName))
						displayName = bgPath;
					_listBox.Items.Add(displayName);
				}
			}

			// Select current background
			if (!string.IsNullOrEmpty(_config.BackgroundImagePath))
			{
				var currentIndex = _config.BackgroundImages.IndexOf(_config.BackgroundImagePath);
				if (currentIndex >= 0)
				{
					_listBox.SelectedIndex = currentIndex + 1; // +1 for "(None)" option
				}
				else
				{
					// Current background not in list, show it anyway
					var displayName = Path.GetFileName(_config.BackgroundImagePath);
					if (string.IsNullOrEmpty(displayName))
						displayName = _config.BackgroundImagePath;
					_listBox.Items.Add($"{displayName} (current)");
					_listBox.SelectedIndex = _listBox.Items.Count - 1;
				}
			}
			else
			{
				_listBox.SelectedIndex = 0; // Select "(None)"
			}
		}

		public string SelectedBackgroundPath
		{
			get
			{
				var selectedIndex = _listBox.SelectedIndex;
				if (selectedIndex <= 0)
					return string.Empty; // "(None)" selected

				// Adjust for "(None)" option
				var bgIndex = selectedIndex - 1;
				if (_config.BackgroundImages != null && bgIndex >= 0 && bgIndex < _config.BackgroundImages.Count)
				{
					return _config.BackgroundImages[bgIndex];
				}

				// If it's the "(current)" item, return the current path
				if (selectedIndex == _listBox.Items.Count - 1 && _listBox.Items[selectedIndex].ToString().EndsWith("(current)"))
				{
					return _config.BackgroundImagePath;
				}

				return string.Empty;
			}
		}
	}
}
