using System;
using EPOOutline;
using TMPro;
using UnityEngine;

namespace Gameplay.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Outlinable _outlinable;
        [SerializeField] private bool _isInteractable;

        [Header("Debug settings")] [SerializeField]
        private bool _debug;

        [SerializeField] private TMP_Text _debugTitle;
        [SerializeField] private Material _debugMaterial;

        private Material _defaultMaterial;

        public UnitState CurrentState { get; private set; } = UnitState.Normal;
        public bool IsInteractable => _isInteractable;
        
        public event Action<Unit, UnitState> StateChanged;

        private void Awake()
        {
            _defaultMaterial = _renderer.sharedMaterial;
        }

        public void PlayMovement(AnimationClip animationClip)
        {
            //_animator.Play(animationClip.name);
            _animator.SetTrigger(animationClip.name);

            if (_debug)
                _debugTitle.text = animationClip.name;
        }

        public void SetState(UnitState state)
        {
            var oldState = CurrentState;
            CurrentState = state;
            
            if (state != oldState)
                StateChanged?.Invoke(this, state);
        }

        public void SetHighlighted(bool highlighted)
        {
            if (_debug)
                _renderer.material = highlighted ? _debugMaterial : _defaultMaterial;

            if (_outlinable)
            {
                _outlinable.enabled = highlighted;
            }
        }
    }
}