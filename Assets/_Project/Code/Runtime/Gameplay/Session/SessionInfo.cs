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

        public event Action LevelChanged;
        
        public SessionInfo(SessionConfig config)
        {
            _config = config;
            _levels = _config.Levels;
            _currentLevelIndex = 0;
            CurrentSequence = new MovementSequence(_config.SequenceSize);
        }
        
        public bool IsLastLevel()
        {
            return _currentLevelIndex == _levels.Length - 1;
        }

        public void MoveToNextLevel()
        {
            _currentLevelIndex++;
            CurrentSequence = new MovementSequence(_config.SequenceSize);
            LevelChanged?.Invoke();
        }
    }
}