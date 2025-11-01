using Gameplay.Session;
using UniRx;
using Zenject;

namespace UI.LevelInfo
{
    public class CurrentSequenceViewPresenter : ICurrentSequenceViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly CompositeDisposable _disposables = new();
        
        public ISequenceSlotViewPresenter[] SequenceSlots { get; }
        public ReactiveCommand<int> AddMovementRequest { get; } = new();

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
                    AddMovementRequest.Execute(currentIndex);
                }).AddTo(_disposables);
                
                SequenceSlots[i] = slotPresenter;
            }
        }

        ~CurrentSequenceViewPresenter()
        {
            _disposables.Dispose();
        }
    }
}