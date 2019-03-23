#define DEBUG

using System;
using ColossalFramework.Plugins;

namespace FontFix
{
    public class Log
    {
        public static void Debug(string label, object value = null)
		{
#if DEBUG
			var message = "FontFix:" + label;

			if (value != null)
			{
				message += ":" + value;
			}

			DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, message);
#endif
		}

        public static void Error(string label, object value = null)
		{
			var message = "FontFix:" + label;

			if (value != null)
			{
				message += ":" + value;
			}

			DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, message);
		}
    }
}