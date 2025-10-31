using System;
using TMPro;
using UnityEngine;

namespace Gameplay.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _isInteractable;

        [Header("Debug settings")] 
        [SerializeField] private bool _debug;
        [SerializeField] private TMP_Text _debugTitle;
        [SerializeField] private Material _debugMaterial;

        private Material _defaultMaterial;
        private Renderer _renderer;

        public UnitState CurrentState { get; private set; } = UnitState.Normal;
        public bool IsInteractable => _isInteractable;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _defaultMaterial = _renderer.sharedMaterial;
        }

        public void PlayMovement(AnimationClip animationClip)
        {
            _animator.Play(animationClip.name);
            
            if (_debug)
                _debugTitle.text = animationClip.name;
        }

        public void SetState(UnitState state)
        {
            CurrentState = state;
        }
        
        public void SetHighlighted(bool highlighted)
        {
            if (_debug)
                _renderer.material = highlighted ? _debugMaterial : _defaultMaterial;
        }
    }
}