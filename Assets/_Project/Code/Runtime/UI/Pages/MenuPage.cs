using DoubleDCore.UI;
using DoubleDCore.UI.Base;

public class MenuPage : MonoPage, IUIPage
{
    public override void Initialize()
    {
        SetCanvasState(true);
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
