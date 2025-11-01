using Gameplay.Movements;
using Gameplay.Session;
using UniRx;
using Zenject;

namespace UI.LevelInfo
{
    public class LevelInfoViewPresenter : ILevelInfoViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly StringReactiveProperty _name = new("");
        private readonly StringReactiveProperty _description = new("");
        private readonly CompositeDisposable _disposables = new();
        
        public ICurrentSequenceViewPresenter CurrentSequenceViewPresenter { get; }
        public ICurrentDifficultyViewPresenter CurrentDifficultyViewPresenter { get; }
        
        public IReadOnlyReactiveProperty<string> Name => _name;
        public IReadOnlyReactiveProperty<string> Description => _description;
        
        public ReactiveCommand PlayCommand { get; } = new();
        public ReactiveCommand<int> AddMovementRequest { get; } = new();
        public ReactiveCommand<(int, MovementConfig)> AddMovementCommand { get; } = new();

        public LevelInfoViewPresenter(DiContainer diContainer, SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
            
            CurrentSequenceViewPresenter = diContainer.Instantiate<CurrentSequenceViewPresenter>();
            CurrentDifficultyViewPresenter = diContainer.Instantiate<CurrentDifficultyViewPresenter>();
            
            CurrentSequenceViewPresenter.AddMovementRequest.Subscribe((index) =>
            {
                AddMovementRequest.Execute(index);
            }).AddTo(_disposables);
            
            AddMovementCommand.Subscribe((info) =>
            {
                CurrentSequenceViewPresenter.AddMovementCommand.Execute(info);
            }).AddTo(_disposables);
            
            _sessionInfo.LevelChanged += OnLevelChanged;
        }

        private void OnLevelChanged()
        {
            var currentLevel = _sessionInfo.CurrentLevel;
            _name.Value = currentLevel.Name;
            _description.Value = currentLevel.Description;
            CurrentDifficultyViewPresenter.ChangeDifficultyCommand.Execute(0);
        }
        
        ~LevelInfoViewPresenter()
        {
            _sessionInfo.LevelChanged -= OnLevelChanged;
            _disposables.Dispose();
        }
    }
}