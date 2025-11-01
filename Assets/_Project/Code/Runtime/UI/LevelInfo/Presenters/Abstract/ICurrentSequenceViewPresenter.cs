using Gameplay.Movements;
using UniRx;

namespace UI.LevelInfo
{
    public interface ICurrentSequenceViewPresenter
    {
        ISequenceSlotViewPresenter[] SequenceSlots { get; }
        ReactiveCommand<int> AddMovementRequest { get; }
        ReactiveCommand<(int, MovementConfig)> AddMovementCommand { get; }
    }
}