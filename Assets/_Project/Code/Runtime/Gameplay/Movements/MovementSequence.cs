using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Movements
{
    public class MovementSequence
    {
        private readonly MovementConfig[] _sequence;
        
        public IReadOnlyList<MovementConfig> CurrentMovements => _sequence
            .Where(x => x != null).ToList();
        
        public int Length => _sequence.Length;

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
        }

        public void RemoveMovement(int index)
        {
            _sequence[index] = null;
        }
    }
}