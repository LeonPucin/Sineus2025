using UniRx;

namespace UI.LevelInfo
{
    public interface IRageViewPresenter
    {
        IReadOnlyReactiveProperty<float> Rage { get; }
    }
}