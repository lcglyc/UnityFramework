using FairyGUI;

namespace ECSModel
{
    public static class FUIHelper
    {
        public static GWindow asGWindow(this GObject gObject)
        {
            return gObject as GWindow;
        }
    }
}