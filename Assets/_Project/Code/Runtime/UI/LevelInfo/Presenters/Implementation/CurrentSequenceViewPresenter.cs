using Gameplay.Movements;
using Gameplay.Session;
using UniRx;
using Zenject;

namespace UI.LevelInfo
{
    public class CurrentSequenceViewPresenter : ICurrentSequenceViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly CompositeDisposable _disposables = new();
        
        private MovementConfig _movementConfig;
        
        public ISequenceSlotViewPresenter[] SequenceSlots { get; }
        public ReactiveCommand<MovementConfig> AddMovementRequest { get; } = new();

        public CurrentSequenceViewPresenter(SessionInfo sessionInfo, DiContainer diContainer)
        {
            _sessionInfo = sessionInfo;
            int slotCount = sessionInfo.CurrentSequence.Length;
            SequenceSlots = new ISequenceSlotViewPresenter[slotCount];

            for (int i = 0; i < slotCount; i++)
            {
                int currentIndex = i;
                var slotPresenter = diContainer.Instantiate<SequenceSlotViewPresenter>(new object[] { currentIndex });
                
                slotPresenter.AddRequest.Subscribe((_) =>
                {
                    if (_movementConfig != null && _sessionInfo.CurrentSequence.GetMovement(currentIndex) == null)
                    {
                        _sessionInfo.CurrentSequence.SetMovement(currentIndex, _movementConfig);
                        _movementConfig = null;
                        
                        foreach (var slot in SequenceSlots)
                        {
                            slot.SetAdditionAvailableCommand.Execute(false);
                            slot.SetRemovalAvailableCommand.Execute(true);
                        }
                    }
                }).AddTo(_disposables);
                
                SequenceSlots[i] = slotPresenter;
            }
            
            AddMovementRequest.Subscribe(config =>
            {
                _movementConfig = config;

                foreach (var slot in SequenceSlots)
                {
                    slot.SetAdditionAvailableCommand.Execute(true);
                    slot.SetRemovalAvailableCommand.Execute(false);
                }
            }).AddTo(_disposables);
        }

        ~CurrentSequenceViewPresenter()
        {
            _disposables.Dispose();
        }
    }
}