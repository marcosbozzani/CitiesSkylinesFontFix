using System;
using ICities;
using ColossalFramework.UI;

namespace FontFix
{
	public class Main : IUserMod 
	{
		public string Name 
		{
			get { return "Font Fix"; }
		}

		public string Description 
		{
			get { return "Fix small font"; }
		}

		private UIDropDown FontFamilyDropDown;
		private UIDropDown BaseSizeDropDown;
		private UICheckBox IncreaseSmallTextCheckBox;
		private Settings Settings = new Settings();

		public void OnSettingsUI(UIHelperBase helper)
		{
			Log.Debug("OnSettingsUI");
			
			Settings.Load();

			var group = helper.AddGroup("Font Settings");
			
			FontFamilyDropDown = (UIDropDown)group.AddDropdown
			(
				"Font Family", 
				Settings.FontFamilies, 
				Settings.FontFamilyIndex, 
				OnFontFamilyChanged
			);
			FontFamilyDropDown.width *= 2f;
			FontFamilyDropDown.listWidth = (int)FontFamilyDropDown.width;

			BaseSizeDropDown = (UIDropDown)group.AddDropdown
			(
				"Font Size", 
				Settings.BaseSizes, 
				Settings.BaseSizeIndex, 
				OnBaseSizeChanged
			);

			IncreaseSmallTextCheckBox = (UICheckBox)group.AddCheckbox
			(
				"Increase size of small text", 
				Settings.IncreaseSmallText, 
				OnIncreaseSmallTextChanged
			);
		
			group.AddSpace(25);
			group.AddButton("Reset to Default", Reset);
		}

		private void OnFontFamilyChanged(int index)
		{
			Settings.FontFamily = Settings.FontFamilies[index];
			Log.Debug("OnFontFamilyChanged", Settings.FontFamily);
			Settings.Save();
		}

		private void OnBaseSizeChanged(int index)
		{
			Settings.BaseSize = int.Parse(Settings.BaseSizes[index]);
			Log.Debug("OnBaseSizeChanged", Settings.BaseSize);
			Settings.Save();
		}

		private void OnIncreaseSmallTextChanged(bool value)
		{
			Settings.IncreaseSmallText = value;
			Log.Debug("OnIncreaseSmallTextChanged", Settings.IncreaseSmallText);
			Settings.Save();
		}

		private void Reset()
		{
			Settings.Reset();
			FontFamilyDropDown.selectedIndex = Settings.FontFamilyIndex;
			BaseSizeDropDown.selectedIndex = Settings.BaseSizeIndex;
			IncreaseSmallTextCheckBox.isChecked = Settings.IncreaseSmallText;
			Settings.Save();
		}
	}
}