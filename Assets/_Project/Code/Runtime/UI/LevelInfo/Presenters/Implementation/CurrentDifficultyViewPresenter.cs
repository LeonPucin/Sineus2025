using System.Linq;
using Gameplay.Movements;
using Gameplay.Session;
using UniRx;

namespace UI.LevelInfo
{
    public class CurrentDifficultyViewPresenter : ICurrentDifficultyViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly IntReactiveProperty _currentDifficulty = new(0);
        
        public IReadOnlyReactiveProperty<int> CurrentDifficulty => _currentDifficulty;
        public int MaxDifficulty { get; }

        public CurrentDifficultyViewPresenter(MovementConfigsCatalog movementsCatalog, SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
            MaxDifficulty = movementsCatalog.GetAllItems().Sum(x => x.DifficultyPoints);

            _sessionInfo.SequenceMovementChanged += OnSequenceMovementChanged;
            _sessionInfo.LevelChanged += OnLevelChanged;
        }

        private void OnLevelChanged()
        {
            _currentDifficulty.Value = 0;
        }

        private void OnSequenceMovementChanged(int _)
        {
            _currentDifficulty.Value = _sessionInfo.CurrentSequence.TotalDifficultyPoints;
        }

        ~CurrentDifficultyViewPresenter()
        {
            _sessionInfo.SequenceMovementChanged -= OnSequenceMovementChanged;
            _sessionInfo.LevelChanged -= OnLevelChanged;
        }
    }
}