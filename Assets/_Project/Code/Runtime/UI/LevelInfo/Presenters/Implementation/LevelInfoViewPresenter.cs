using Gameplay.Movements;
using Gameplay.Quests;
using Gameplay.Session;
using UniRx;
using Zenject;

namespace UI.LevelInfo
{
    public class LevelInfoViewPresenter : ILevelInfoViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly StartLevelQuestConditionChecker _startLevelChecker;
        private readonly LevelStarter _levelStarter;
        private readonly StringReactiveProperty _name = new("");
        private readonly StringReactiveProperty _description = new("");
        private readonly CompositeDisposable _disposables = new();
        private readonly BoolReactiveProperty _canPlayLevel;
        
        public ICurrentSequenceViewPresenter CurrentSequenceViewPresenter { get; }
        public ICurrentDifficultyViewPresenter CurrentDifficultyViewPresenter { get; }
        
        public IReadOnlyReactiveProperty<string> Name => _name;
        public IReadOnlyReactiveProperty<string> Description => _description;
        
        public ReactiveCommand PlayRequest { get; }
        public ReactiveCommand<MovementConfig> AddMovementRequest { get; } = new();

        public LevelInfoViewPresenter(DiContainer diContainer, SessionInfo sessionInfo,
            StartLevelQuestConditionChecker startLevelChecker, LevelStarter levelStarter)
        {
            _sessionInfo = sessionInfo;
            _startLevelChecker = startLevelChecker;
            _levelStarter = levelStarter;

            _canPlayLevel = new(_startLevelChecker.CanStartLevel());
            PlayRequest = new ReactiveCommand(_canPlayLevel);
            
            CurrentSequenceViewPresenter = diContainer.Instantiate<CurrentSequenceViewPresenter>();
            CurrentDifficultyViewPresenter = diContainer.Instantiate<CurrentDifficultyViewPresenter>();
            
            AddMovementRequest.Subscribe((config) =>
            {
                CurrentSequenceViewPresenter.AddMovementRequest.Execute(config);
            }).AddTo(_disposables);
            
            PlayRequest.Subscribe((_) =>
            {
                _levelStarter.SetupLevel();
            }).AddTo(_disposables);
            
            _sessionInfo.LevelChanged += OnLevelChanged;
            _sessionInfo.SequenceMovementChanged += OnMovementChanged;
            OnLevelChanged();
        }

        private void OnMovementChanged(int _)
        {
            _canPlayLevel.Value = _startLevelChecker.CanStartLevel();
        }

        private void OnLevelChanged()
        {
            var currentLevel = _sessionInfo.CurrentLevel;
            _name.Value = currentLevel.Name;
            _description.Value = currentLevel.Description;
            _canPlayLevel.Value = _startLevelChecker.CanStartLevel();
        }
        
        ~LevelInfoViewPresenter()
        {
            _sessionInfo.LevelChanged -= OnLevelChanged;
            _sessionInfo.SequenceMovementChanged -= OnMovementChanged;
            _disposables.Dispose();
        }
    }
}