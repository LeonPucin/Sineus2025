using UniRx;

namespace UI.LevelInfo
{
    public interface ICurrentDifficultyViewPresenter
    {
        IReadOnlyReactiveProperty<int> CurrentDifficulty { get; }
        int MaxDifficulty { get; }
        ReactiveCommand<int> ChangeDifficultyCommand { get; }
    }
}