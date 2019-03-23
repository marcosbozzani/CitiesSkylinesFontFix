using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace FontFix
{
	public class Game
	{
		private static List<ComponentData> componentDataList = new List<ComponentData>();

		public static void UpdateUI(Settings settings)
		{
			foreach (var textComponent in Object.FindObjectsOfType<UITextComponent>())
			{
				string[] fonts;
				var textFont = textComponent.font as UIDynamicFont;

				if (settings.FontFamily == "(Default)")
				{
					if (textFont.name == "ArchitectsDaughter")
					{
						fonts = new[] { "Architects Daughter" };
					}
					else
					{
						fonts = new[] { "Open Sans", "NanumGothic" };
					}
				}
				else
				{
					fonts = new[] { settings.FontFamily };
				}

				textFont.baseFont.fontNames = fonts;
				textFont.size = settings.BaseSize;

				if (textFont.lineHeight >= 19 && textFont.lineHeight <= 22)
				{
					textFont.lineHeight = 22;
					
					if (settings.BaseSize > 18)
					{
						textFont.lineHeight = 21;
					}

					if (settings.BaseSize > 21)
					{
						textFont.lineHeight = 20;
					}

					if (settings.BaseSize > 23)
					{
						textFont.lineHeight = 19;
					}
				}
			}

			if (settings.IncreaseSmallText)
			{
				EnableSmallTextIncrease();
			}
			else
			{
				DisableSmallTextIncrease();
			}

			var instance = Singleton<DistrictManager>.instance;

			if (instance != null)
			{
				instance.NamesModified();
			}

			try 
			{
				UIView.RefreshAll(true);
			}
			catch (System.Exception ex)
			{
				Log.Debug("UIView.RefreshAll(true)", ex.Message);
			}
		}

		private static void EnableSmallTextIncrease()
		{
			if (componentDataList.Count == 0)
			{
				foreach (var panel in Object.FindObjectsOfType<UIPanel>())
				{
					switch (panel.name)
					{
						case "InfoRoadTooltip":
						case "InfoTooltip":
						case "PoliciesTooltip":
						case "InfoAdvancedTooltip":
						case "InfoAdvancedTooltipDetail":
							componentDataList.Add(new ComponentData(panel, panel.height));
							panel.height *= 1.1f;
							break;
					}
				}

				foreach (var textComponent in Object.FindObjectsOfType<UITextComponent>())
				{
					if (textComponent.textScale < 0.9)
					{
						componentDataList.Add(new ComponentData(textComponent, textComponent.textScale));

						if (textComponent.textScale < 0.65)
						{
							textComponent.textScale = 0.75f;
						}
						else
						{
							textComponent.textScale = 0.9f;
						}
					}
				}
			}
		}

		private static void DisableSmallTextIncrease()
		{
			foreach (var componentData in componentDataList)
			{
				if (componentData.Reference != null)
				{
					if (componentData.Reference is UIPanel)
					{
						var component = (UIPanel)componentData.Reference;
						component.height = componentData.OriginalSize;
					}
					else if (componentData.Reference is UITextComponent)
					{
						var component = (UITextComponent)componentData.Reference;
						component.textScale = componentData.OriginalSize;
					}
				}
			}

			componentDataList.Clear();
		}
	}
}