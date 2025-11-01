using System;
using System.Collections.Generic;
using System.Linq;
using DoubleDCore.TimeTools;
using Gameplay.Movements;
using Gameplay.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.UnitCrowd
{
    public class CrowdMovementsController
    {
        private readonly CrowdMovementsConfig _config;
        private readonly Timer _animationTimer = new(TimeBindingType.ScaledTime);
        
        private List<Unit> _units;
        private Unit _mainUnit;
        private MovementSequence _movementSequence;
        private MovementConfig _lastMovement;
        private int _currentMovementIndex;

        public event Action MovementStarting;

        public CrowdMovementsController(CrowdMovementsConfig config)
        {
            _config = config;
        }

        public void Setup(List<Unit> otherUnits, Unit mainUnit, MovementSequence movementSequence)
        {
            _units = otherUnits.ToList();
            _units.Add(mainUnit);
            _mainUnit = mainUnit;
            
            _movementSequence = movementSequence;
            _currentMovementIndex = 0;
        }
        
        public void Start()
        {
            foreach (var unit in _units)
                unit.StateChanged += OnUnitStateChanged;
            
            PlayNextMovement();
        }

        public void Stop()
        {
            foreach (var unit in _units)
                unit.StateChanged -= OnUnitStateChanged;
            
            _animationTimer.Stop();
        }
        
        private void PlayNextMovement()
        {
            MovementStarting?.Invoke();
            _lastMovement = _movementSequence.ValidSequence[_currentMovementIndex];
            
            foreach (var unit in _units)
            {
                PlayUnitMovement(unit);
            }
            
            _currentMovementIndex = (_currentMovementIndex + 1) % _movementSequence.ValidSequence.Count;
            _ = _animationTimer.Start(_lastMovement.AnimationClip.length, PlayNextMovement);
        }

        private void OnUnitStateChanged(Unit unit, UnitState previousState)
        {
            if (unit.CurrentState == UnitState.Invincible)
            {
                AnimatorStateInfo stateInfo = _mainUnit.Animator.GetCurrentAnimatorStateInfo(0);
                float normalizedTime = stateInfo.normalizedTime;
                unit.PlayMovement(_lastMovement.AnimationClip, normalizedTime);
            }
        }
        
        private void PlayUnitMovement(Unit unit)
        {
            AnimationClip resultClip;
                
            switch (unit.CurrentState)
            {
                case UnitState.Invincible:
                case UnitState.Normal:
                    resultClip = _movementSequence.ValidSequence[_currentMovementIndex].AnimationClip;
                    break;
                case UnitState.Idle:
                    resultClip = _config.IdleMovementConfig.AnimationClip;
                    break;
                case UnitState.Broken:
                    resultClip = _config.PossibleMovements[Random.Range(0, _config.PossibleMovements.Length)]
                        .AnimationClip;
                    
                    while (resultClip == _movementSequence.ValidSequence[_currentMovementIndex].AnimationClip)
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
    }
}