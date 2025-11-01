using Gameplay.Movements;
using UniRx;
using UnityEngine;

namespace UI.LevelInfo
{
    public class SequenceSlotViewPresenter : ISequenceSlotViewPresenter
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly BoolReactiveProperty _hasMovement = new(false);
        private readonly ReactiveProperty<Sprite> _icon = new(null);
        
        public IReadOnlyReactiveProperty<bool> HasMovement => _hasMovement;
        public IReadOnlyReactiveProperty<Sprite> Icon => _icon;
        
        public ReactiveCommand RemoveRequest { get; } = new();
        public ReactiveCommand AddRequest { get; } = new();
        
        public ReactiveCommand<MovementConfig> AddCommand { get; } = new();
        public ReactiveCommand RemoveCommand { get; } = new();

        public SequenceSlotViewPresenter()
        {
            AddCommand.Subscribe((config) =>
            {
                _hasMovement.Value = true;
                _icon.Value = config.Icon;
            }).AddTo(_disposables);
            
            RemoveCommand.Subscribe((_) =>
            {
                _hasMovement.Value = false;
                _icon.Value = null;
            }).AddTo(_disposables);
        }
        
        ~SequenceSlotViewPresenter()
        {
            _disposables.Dispose();
        }
    }
}