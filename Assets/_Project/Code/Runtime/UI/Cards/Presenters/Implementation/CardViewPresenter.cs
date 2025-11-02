using Gameplay.Movements;
using UniRx;
using UnityEngine;

namespace UI.Cards
{
    public class CardViewPresenter : ICardViewPresenter
    {
        private readonly MovementConfig _movement;
        private readonly BoolReactiveProperty _canBeSelected = new(true);
        private readonly CompositeDisposable _disposables = new();
        
        public Sprite Icon { get; }
        public string Name { get; }
        public int DifficultyPoints { get; }
        
        public IReadOnlyReactiveProperty<bool> CanBeSelected => _canBeSelected;
        
        public ReactiveCommand SelectRequest { get; }
        public ReactiveCommand<bool> SetSelectionAvailabilityCommand{ get; }
        public ReactiveCommand<MovementConfig> SelectedEvent { get; }

        public CardViewPresenter(MovementConfig movement)
        {
            _movement = movement;
            
            Icon = movement.Icon;
            Name = movement.Title;
            DifficultyPoints = movement.DifficultyPoints;

            SelectRequest = new(_canBeSelected);
            SelectedEvent = new();
            SetSelectionAvailabilityCommand = new();
            
            SetSelectionAvailabilityCommand.Subscribe((isAvailable) =>
            {
                _canBeSelected.Value = isAvailable;
            }).AddTo(_disposables);

            SelectRequest.Subscribe((_) =>
            {
                _canBeSelected.Value = false;
                SelectedEvent.Execute(_movement);
            }).AddTo(_disposables);
        }
        
        ~CardViewPresenter()
        {
            _disposables.Dispose();
        }
    }
}