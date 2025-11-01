using Gameplay.Movements;
using UniRx;
using UnityEngine;

namespace UI.Cards
{
    public interface ICardViewPresenter
    {
        Sprite Icon { get; }
        string Name { get; }
        int DifficultyPoints { get; }
        IReadOnlyReactiveProperty<bool> CanBeSelected { get; }
        ReactiveCommand SelectRequest { get; }
        ReactiveCommand<int> SelectCommand { get; }
        ReactiveCommand DisableSelectionCommand { get; }
        ReactiveCommand<(int, MovementConfig)> SelectedEvent { get; }
    }
}