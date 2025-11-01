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
            MovementChanged?.Invoke(index);
        }

        public void RemoveMovement(int index)
        {
            _sequence[index] = null;
            MovementChanged?.Invoke(index);
        }
    }
}