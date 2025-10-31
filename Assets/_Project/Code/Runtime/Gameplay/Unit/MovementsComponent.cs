using System;
using System.Collections.Generic;
using DoubleDCore.TimeTools;
using Gameplay.Movements;
using ModestTree;
using TMPro;
using UnityEngine;

namespace Gameplay.Unit
{
    [Serializable]
    public class MovementsComponent
    {
        private readonly Queue<Movement> _movementQueue = new Queue<Movement>();
        private readonly Timer _animationTimer = new Timer(TimeBindingType.ScaledTime);
        
        [SerializeField] private Animator _animator;
        
        [Header("Debug settings")]
        [SerializeField] private bool _debug;
        [SerializeField] private TMP_Text _stateTitle;
        
        private MoveSequence _sequence;
        private Movement _currentMovement;
        private UnitConfig _unitConfig;

        public void Init(UnitConfig config)
        {
            _unitConfig = config;
        }
        
        public void SetSequence(MoveSequence sequence)
        {
            _sequence = sequence;
        }
        
        public void StartPlaying()
        {
            PlayNextMovement();
        }
        
        public void StopPlaying()
        {
            
        }

        private void PlayNextMovement()
        {
            if (_movementQueue.IsEmpty())
            {
                Debug.Log("No movements in current queue, loading sequence...");
                LoadSequence();
            }
            
            _currentMovement = _movementQueue.Dequeue();
            var clip = _currentMovement.Config.AnimationClip;
            _animator.Play(clip.name);
            _ = _animationTimer.Start(clip.length, PlayNextMovement);

            if (_debug)
            {
                _stateTitle.text = clip.name;
                Debug.Log($"SWITCHED TO MOVEMENT: {clip.name}");
            }
        }

        private void LoadSequence()
        {
            _movementQueue.Clear();
            
            if (_sequence == null || _sequence.CurrentMovements.IsEmpty())
            {
                Debug.LogError("No movement sequence set or sequence is empty!");
                return;
            }
            
            foreach (var movementConfig in _sequence.CurrentMovements)
            {
                var movement = new Movement(movementConfig);
                _movementQueue.Enqueue(movement);
            }
        }
    }
}