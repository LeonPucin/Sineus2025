using System;

namespace Gameplay.UnitCrowd
{
    public class MovementTimer
    {
        private readonly CrowdMovementsController _movementController;

        private int _ticksLeft;
        private Action _onFinished;
        
        public MovementTimer(CrowdMovementsController movementController)
        {
            _movementController = movementController;
        }

        public void Start(int ticksDelay, Action onFinished)
        {
            _ticksLeft = ticksDelay + 1;
            _onFinished = onFinished;
            
            if (_ticksLeft < 0)
                onFinished?.Invoke();
            else
                _movementController.MovementStarting += OnTicked;
        }

        public void Stop()
        {
            _movementController.MovementStarting -= OnTicked;
        }

        private void OnTicked()
        {
            _ticksLeft--;

            if (_ticksLeft < 0)
            {
                _onFinished?.Invoke();
                Stop();
            }
        }
    }
}