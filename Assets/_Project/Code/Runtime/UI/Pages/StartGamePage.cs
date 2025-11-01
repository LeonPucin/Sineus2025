using DoubleDCore.UI;
using DoubleDCore.UI.Base;

namespace UI.Pages
{
    public class StartGamePage : MonoPage, IUIPage
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