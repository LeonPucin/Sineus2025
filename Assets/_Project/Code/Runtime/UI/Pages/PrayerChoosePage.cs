using DoubleDCore.UI;
using DoubleDCore.UI.Base;

public class PrayerChoosePage : MonoPage, IPayloadPage<PrayerChooseInfo>
{
    public override void Initialize()
    {
        SetCanvasState(false);
    }

    public void Open(PrayerChooseInfo context)
    {
        SetCanvasState(true);
    }

    public override void Close()
    {
        SetCanvasState(false);
    }
}

public class PrayerChooseInfo
{
}