using Gameplay.Session;
using UniRx;
using UnityEngine;

namespace UI.LevelInfo
{
    public class SequenceSlotViewPresenter : ISequenceSlotViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly int _movementIndex;
        private readonly BoolReactiveProperty _hasMovement = new(false);
        private readonly BoolReactiveProperty _hasNotMovement = new(true);
        private readonly ReactiveProperty<Sprite> _icon = new(null);
        private readonly CompositeDisposable _disposables = new();
        
        private readonly BoolReactiveProperty _canAddMovement = new(false);
        private readonly BoolReactiveProperty _canRemoveMovement = new(false);
        
        public IReadOnlyReactiveProperty<Sprite> Icon => _icon;
        
        public IReadOnlyReactiveProperty<bool> CanAddMovement => _canAddMovement;
        public IReadOnlyReactiveProperty<bool> CanRemoveMovement => _canRemoveMovement;
        
        public ReactiveCommand RemoveRequest { get; }
        public ReactiveCommand AddRequest { get; }
        public ReactiveCommand<bool> SetAdditionAvailableCommand { get; } = new();
        public ReactiveCommand<bool> SetRemovalAvailableCommand { get; } = new();
        
        public SequenceSlotViewPresenter(SessionInfo sessionInfo, int movementIndex)
        {
            _sessionInfo = sessionInfo;
            _movementIndex = movementIndex;
            
            RemoveRequest = new ReactiveCommand(_canRemoveMovement);
            AddRequest = new ReactiveCommand(_canAddMovement);
            
            RemoveRequest.Subscribe(OnRemoveRequested).AddTo(_disposables);

            SetAdditionAvailableCommand.Subscribe((canAdd) =>
            {
                _canAddMovement.Value = canAdd && _hasNotMovement.Value;
            }).AddTo(_disposables);
            
            SetRemovalAvailableCommand.Subscribe((canRemove) =>
            {
                _canRemoveMovement.Value = canRemove && _hasMovement.Value;
            }).AddTo(_disposables);
            
            _sessionInfo.SequenceMovementChanged += OnSequenceMovementChanged;
            _sessionInfo.LevelChanged += OnLevelChanged;
        }

        private void OnLevelChanged()
        {
            UpdateMovementInfo();
        }

        private void OnRemoveRequested(Unit _)
        {
            _sessionInfo.CurrentSequence.RemoveMovement(_movementIndex);
        }

        private void OnSequenceMovementChanged(int index)
        {
            if (index != _movementIndex)
                return;

            UpdateMovementInfo();
        }

        private void UpdateMovementInfo()
        {
            var movement = _sessionInfo.CurrentSequence.GetMovement(_movementIndex);
            
            _hasMovement.Value = movement != null;
            _hasNotMovement.Value = movement == null;
            _icon.Value = movement != null ? movement.Icon : null;
            
            _canAddMovement.Value = _canAddMovement.Value && _hasNotMovement.Value;
            _canRemoveMovement.Value = _canRemoveMovement.Value && _hasMovement.Value;
        }

        ~SequenceSlotViewPresenter()
        {
            _sessionInfo.SequenceMovementChanged -= OnSequenceMovementChanged;
            _sessionInfo.LevelChanged -= OnLevelChanged;
            _disposables.Dispose();
        }
    }
}