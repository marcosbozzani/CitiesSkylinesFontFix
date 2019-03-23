using System;
using System.IO;
using UnityEngine;
using ColossalFramework;

namespace FontFix
{
    public class Settings
    {
        public static string[] FontFamilies;
		public static string[] BaseSizes = new string[]
		{
			"13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25"
		};

        public static string SettingsPath;
        static Settings()
        {
            var pathName = GameSettings.FindSettingsFileByName("gameSettings").pathName;
            SettingsPath = Path.Combine(Path.GetDirectoryName(pathName), "FontFix.settings");

            var fonts = Font.GetOSInstalledFontNames();
            FontFamilies = new string[fonts.Length + 1];
            FontFamilies[0] = DefaultFontFamily;
            fonts.CopyTo(FontFamilies, 1);

            Log.Debug("Fonts", string.Join("\n", Settings.FontFamilies));
        }

        public string FontFamily { get; set; }
	    public int BaseSize { get; set; }
	    public bool IncreaseSmallText { get; set; }

        public int FontFamilyIndex
        {
            get
            {
                return Array.IndexOf(FontFamilies, FontFamily);
            }
        }

        public int BaseSizeIndex
        {
            get
            {
                return Array.IndexOf(BaseSizes, BaseSize.ToString());
            }
        }

        public const string DefaultFontFamily = "(Default)";
        public const int DefaultBaseSize = 16;
        public const bool DefaultIncreaseSmallText = false;

        public void Reset()
		{
			FontFamily = DefaultFontFamily;
			BaseSize = DefaultBaseSize;
			IncreaseSmallText = DefaultIncreaseSmallText;
		}

        public void Load()
        {
            Reset();

            if (File.Exists(SettingsPath))
            {
                Log.Debug("SettingsPath", SettingsPath);

                try
                {
                    using (var reader = new StreamReader(SettingsPath))
                    {
                        string line = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var value = ReadValue(line, "FontFamily");
                            if (value != null)
                            {
                                FontFamily = value;
                                var index = Array.IndexOf(FontFamilies, FontFamily);
                                if (index == -1)
                                {
                                    FontFamily = DefaultFontFamily;
                                }
                            }

                            value = ReadValue(line, "BaseSize");
                            if (value != null)
                            {
                                int result = 0;
                                if (int.TryParse(value, out result))
                                {
                                    BaseSize = result;
                                    var index = Array.IndexOf(BaseSizes, BaseSize.ToString());
                                    if (index == -1)
                                    {
                                        BaseSize = DefaultBaseSize;
                                    }
                                }
                                else
                                {
                                    BaseSize = DefaultBaseSize;
                                }
                            }
                            
                            value = ReadValue(line, "IncreaseSmallText");
                            if (value != null)
                            {
                                bool result = false;
                                if (bool.TryParse(value, out result))
                                {
                                    IncreaseSmallText = result;
                                }
                                else
                                {
                                    IncreaseSmallText = DefaultIncreaseSmallText;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Settings", ex.Message);
                    Reset();
                }
            }

            Game.UpdateUI(this);
            Log.Debug("Settings", "Loaded");
        }

        private string ReadValue(string input, string name)
        {
            Log.Debug("ReadValue.input", input);
            Log.Debug("ReadValue.name", name);

            if (input.StartsWith(name + "="))
            {
                var items = input.Split(new[] { name + "=" }, StringSplitOptions.RemoveEmptyEntries);
                
                Log.Debug("ReadValue.items", string.Join(", ", items));
                if (items.Length == 1)
                {
                    return items[0];
                }

                return null;                                
            }

            return null;
        }        

        public void Save()
        {
            try
            {
                using (var writer = new StreamWriter(SettingsPath))
                {
                    writer.WriteLine("FontFamily=" + FontFamily);
                    writer.WriteLine("BaseSize=" + BaseSize);
                    writer.WriteLine("IncreaseSmallText=" + IncreaseSmallText);
                    Game.UpdateUI(this);
                    Log.Debug("Settings", "Saved");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Settings", ex.Message);
            }
        }
    }
}