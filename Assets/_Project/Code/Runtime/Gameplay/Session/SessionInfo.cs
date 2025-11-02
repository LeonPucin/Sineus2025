using System;
using Gameplay.Movements;

namespace Gameplay.Session
{
    public class SessionInfo
    {
        private readonly SessionConfig _config;
        private readonly LevelConfig[] _levels;
        
        private int _currentLevelIndex = 0;
        
        public LevelConfig CurrentLevel => _levels[_currentLevelIndex];
        public MovementSequence CurrentSequence { get; private set; }

        public bool IsEducated = false;

        public event Action LevelChanged;
        public event Action<int> SequenceMovementChanged;
        
        public SessionInfo(SessionConfig config)
        {
            _config = config;
            _levels = _config.Levels;
            _currentLevelIndex = 0;
            CreateNewSequence();
        }
        
        public bool IsLastLevel()
        {
            return _currentLevelIndex == _levels.Length - 1;
        }

        public void MoveToNextLevel()
        {
            _currentLevelIndex++;
            CreateNewSequence();
            LevelChanged?.Invoke();
        }
        
        private void CreateNewSequence()
        {
            if (CurrentSequence != null)
                CurrentSequence.MovementChanged -= OnMovementChanged;
            
            CurrentSequence = new MovementSequence(_config.SequenceSize);
            CurrentSequence.MovementChanged += OnMovementChanged;
        }
        
        private void OnMovementChanged(int index)
        {
            SequenceMovementChanged?.Invoke(index);
        }
        
        ~SessionInfo()
        {
            if (CurrentSequence != null)
                CurrentSequence.MovementChanged -= OnMovementChanged;
        }
    }
}