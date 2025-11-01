using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Movements
{
    public class MovementSequence
    {
        private readonly MovementConfig[] _sequence;
        
        public IReadOnlyList<MovementConfig> ValidSequence => _sequence
            .Where(x => x != null).ToList();
        
        public int Length => _sequence.Length;
        public int TotalDifficultyPoints { get; private set; }

        public event Action<int> MovementChanged; 

        public MovementSequence(int len)
        {
            _sequence = new MovementConfig[len];
        }
        
        public MovementConfig GetMovement(int index)
        {
            return _sequence[index];
        }

        public void SetMovement(int index, MovementConfig config)
        {
            _sequence[index] = config;
            TotalDifficultyPoints += config.DifficultyPoints;
            MovementChanged?.Invoke(index);
        }

        public void RemoveMovement(int index)
        {
            TotalDifficultyPoints -= _sequence[index].DifficultyPoints;
            _sequence[index] = null;
            MovementChanged?.Invoke(index);
        }
    }
}