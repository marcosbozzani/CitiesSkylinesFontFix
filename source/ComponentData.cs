using ColossalFramework.UI;

namespace FontFix
{
    public class ComponentData
    {
        public UIComponent Reference;

        public float OriginalSize;

        public ComponentData(UIComponent reference, float size)
        {
            this.Reference = reference;
            this.OriginalSize = size;
        }
    }
}