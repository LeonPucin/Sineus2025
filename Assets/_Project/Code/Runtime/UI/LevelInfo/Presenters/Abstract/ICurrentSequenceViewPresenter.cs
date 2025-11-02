using Gameplay.Movements;
using UniRx;

namespace UI.LevelInfo
{
    public interface ICurrentSequenceViewPresenter
    {
        ISequenceSlotViewPresenter[] SequenceSlots { get; }
        ReactiveCommand<MovementConfig> AddMovementRequest { get; }
    }
}