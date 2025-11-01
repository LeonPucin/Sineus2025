using Gameplay.Movements;
using UniRx;

namespace UI.LevelInfo
{
    public interface ILevelInfoViewPresenter
    {
        ICurrentSequenceViewPresenter CurrentSequenceViewPresenter { get; }
        ICurrentDifficultyViewPresenter CurrentDifficultyViewPresenter { get; }
        IReadOnlyReactiveProperty<string> Name { get; }
        IReadOnlyReactiveProperty<string> Description { get; }
        ReactiveCommand PlayCommand { get; }
        ReactiveCommand<int> AddMovementRequest { get; }
        ReactiveCommand<(int, MovementConfig)> AddMovementCommand { get; }
    }
}