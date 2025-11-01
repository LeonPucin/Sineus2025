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
        
        public IReadOnlyReactiveProperty<bool> HasMovement => _hasMovement;
        public IReadOnlyReactiveProperty<Sprite> Icon => _icon;
        
        public ReactiveCommand RemoveRequest { get; }
        public ReactiveCommand AddRequest { get; }
        
        public SequenceSlotViewPresenter(SessionInfo sessionInfo, int movementIndex)
        {
            _sessionInfo = sessionInfo;
            _movementIndex = movementIndex;
            
            RemoveRequest = new ReactiveCommand(_hasMovement);
            AddRequest = new ReactiveCommand(_hasNotMovement);
            
            RemoveRequest.Subscribe(OnRemoveRequested).AddTo(_disposables);
            
            _sessionInfo.SequenceMovementChanged += OnSequenceMovementChanged;
        }

        private void OnRemoveRequested(Unit _)
        {
            _sessionInfo.CurrentSequence.RemoveMovement(_movementIndex);
        }

        private void OnSequenceMovementChanged(int index)
        {
            if (index != _movementIndex)
                return;
            
            var movement = _sessionInfo.CurrentSequence.GetMovement(_movementIndex);
            
            _hasMovement.Value = movement != null;
            _hasNotMovement.Value = movement == null;
            _icon.Value = movement != null ? movement.Icon : null;
        }

        ~SequenceSlotViewPresenter()
        {
            _sessionInfo.SequenceMovementChanged -= OnSequenceMovementChanged;
            _disposables.Dispose();
        }
    }
}