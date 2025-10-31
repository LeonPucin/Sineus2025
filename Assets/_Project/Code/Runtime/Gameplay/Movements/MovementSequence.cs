using System.Collections.Generic;

namespace Gameplay.Movements
{
    public class MovementSequence
    {
        private List<MovementConfig> _movements = new List<MovementConfig>();
        
        public IReadOnlyList<MovementConfig> CurrentMovements => _movements;
        
        public void AddMovement(MovementConfig movementConfig)
        {
            _movements.Add(movementConfig);
        }
        
        public void RemoveMovement(MovementConfig movementConfig)
        {
            _movements.Remove(movementConfig);
        }

        public void RemoveMovement(int index)
        {
            _movements.RemoveAt(index);
        }
    }
}