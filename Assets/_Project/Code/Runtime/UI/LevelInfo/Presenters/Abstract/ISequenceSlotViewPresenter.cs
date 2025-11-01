using Gameplay.Movements;
using UniRx;
using UnityEngine;

namespace UI.LevelInfo
{
    public interface ISequenceSlotViewPresenter
    {
        IReadOnlyReactiveProperty<bool> HasMovement { get; }
        IReadOnlyReactiveProperty<Sprite> Icon { get; }
        ReactiveCommand RemoveRequest { get; }
        ReactiveCommand AddRequest { get; }
        ReactiveCommand<MovementConfig> AddCommand { get; }
        ReactiveCommand RemoveCommand { get; }
    }
}