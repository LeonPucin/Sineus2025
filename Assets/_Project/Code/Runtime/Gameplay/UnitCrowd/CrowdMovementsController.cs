using System.Collections.Generic;
using DoubleDCore.TimeTools;
using Gameplay.Movements;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.UnitCrowd
{
    public class CrowdMovementsController
    {
        private readonly CrowdMovementsConfig _config;
        private readonly Timer _animationTimer = new(TimeBindingType.ScaledTime);
        
        private List<Unit> _units;
        private MovementSequence _movementSequence;
        private int _currentMovementIndex;

        public CrowdMovementsController(CrowdMovementsConfig config)
        {
            _config = config;
        }

        public void Setup(List<Unit> units, MovementSequence movementSequence)
        {
            _units = units;
            _movementSequence = movementSequence;
            _currentMovementIndex = 0;
        }
        
        public void Start()
        {
            PlayNextMovement();
        }

        public void Stop()
        {
            _animationTimer.Stop();
        }
        
        private void PlayNextMovement()
        {
            var currentMovement = _movementSequence.CurrentMovements[_currentMovementIndex];
            
            foreach (var unit in _units)
            {
                AnimationClip resultClip;
                
                switch (unit.CurrentState)
                {
                    case UnitState.Normal:
                        resultClip = currentMovement.AnimationClip;
                        break;
                    case UnitState.Idle:
                        resultClip = _config.IdleMovementConfig.AnimationClip;
                        break;
                    case UnitState.Broken:
                        resultClip = _config.PossibleMovements[Random.Range(0, _config.PossibleMovements.Length)]
                            .AnimationClip;
                        break;
                    default:
                        resultClip = null;
                        Debug.LogError($"Unknown state {unit.CurrentState}");
                        break;
                }
                
                unit.PlayMovement(resultClip);
            }
            
            _currentMovementIndex = (_currentMovementIndex + 1) % _movementSequence.CurrentMovements.Count;
            //_ = _animationTimer.Start(currentMovement.AnimationClip.length, PlayNextMovement);
            _ = _animationTimer.Start(_config.CrowdLength, PlayNextMovement);
        }
    }
}