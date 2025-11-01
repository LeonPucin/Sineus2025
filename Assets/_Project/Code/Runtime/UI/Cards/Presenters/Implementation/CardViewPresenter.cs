using Gameplay.Movements;
using UniRx;
using UnityEngine;

namespace UI.Cards
{
    public class CardViewPresenter : ICardViewPresenter
    {
        private readonly MovementConfig _movement;
        private readonly BoolReactiveProperty _canBeSelected = new(false);
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private int _indexToReturn;
        
        public Sprite Icon { get; }
        public string Name { get; }
        public int DifficultyPoints { get; }
        
        public IReadOnlyReactiveProperty<bool> CanBeSelected => _canBeSelected;
        
        public ReactiveCommand SelectRequest { get; }
        public ReactiveCommand<int> SelectCommand { get; }
        public ReactiveCommand DisableSelectionCommand { get; }
        public ReactiveCommand<(int, MovementConfig)> SelectedEvent { get; }

        public CardViewPresenter(MovementConfig movement)
        {
            _movement = movement;
            Icon = movement.Icon;
            Name = movement.Title;
            DifficultyPoints = movement.DifficultyPoints;

            SelectRequest = new(_canBeSelected);
            SelectCommand = new();
            SelectedEvent = new();
            DisableSelectionCommand = new();
            
            SelectCommand.Subscribe((index) =>
            {
                _canBeSelected.Value = true;
                _indexToReturn = index;
            }).AddTo(_disposables);

            SelectRequest.Subscribe((index) =>
            {
                _canBeSelected.Value = false;
                SelectedEvent.Execute((_indexToReturn, _movement));
            }).AddTo(_disposables);
            
            DisableSelectionCommand.Subscribe((_) =>
            {
                _canBeSelected.Value = false;
            }).AddTo(_disposables);
        }
        
        ~CardViewPresenter()
        {
            _disposables.Dispose();
        }
    }
}