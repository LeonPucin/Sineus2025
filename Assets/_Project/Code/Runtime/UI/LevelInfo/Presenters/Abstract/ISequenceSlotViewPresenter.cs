using Gameplay.Movements;
using UniRx;
using UnityEngine;

namespace UI.LevelInfo
{
    public interface ISequenceSlotViewPresenter
    {
        IReadOnlyReactiveProperty<bool> CanAddMovement { get; }
        IReadOnlyReactiveProperty<bool> CanRemoveMovement { get; }
        IReadOnlyReactiveProperty<Sprite> Icon { get; }
        ReactiveCommand RemoveRequest { get; }
        ReactiveCommand AddRequest { get; }
        ReactiveCommand<bool> SetAdditionAvailableCommand { get; }
        ReactiveCommand<bool> SetRemovalAvailableCommand { get; }
    }
}