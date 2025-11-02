using DoubleDCore.UI;
using DoubleDCore.UI.Base;

namespace UI.Pages
{
    public class MenuPage : MonoPage, IUIPage
    {
        public override void Initialize()
        {
            SetCanvasState(false);
        }

        public void Open()
        {
            SetCanvasState(true);
        }

        public override void Close()
        {
            SetCanvasState(false);
        }
    }
}
