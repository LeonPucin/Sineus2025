using System;
using EPOOutline;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace Gameplay.Units
{
    public class Unit : SerializedMonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _isInteractable;

        [Header("Components")] 
        [SerializeField] private OutlineComponent _outlineComponent;
        
        [Header("Debug settings")] 
        [SerializeField] private bool _debug;
        [SerializeField] private TMP_Text _debugTitle;
        
        [OdinSerialize, ReadOnly] public UnitState CurrentState { get; private set; } = UnitState.Normal;
        public bool IsInteractable => _isInteractable;
        public Animator Animator => _animator;
        
        //<summary>
        /// Invoked when unit state is changed
        /// Unit - the unit whose state has changed
        /// UnitState - previous state of the unit
        //</summary>
        public event Action<Unit, UnitState> StateChanged;

        public void PlayMovement(AnimationClip animationClip)
        {
            _animator.SetTrigger(animationClip.name);

            if (_debug)
                _debugTitle.text = CurrentState.ToString();
        }

        public void PlayMovement(AnimationClip animationClip, float clipTime)
        {
            _animator.Play(animationClip.name, 0, clipTime);
            
            if (_debug)
                _debugTitle.text = CurrentState.ToString();
        }

        public void SetState(UnitState state)
        {
            var oldState = CurrentState;
            CurrentState = state;

            if (state != oldState)
            {
                if (_isInteractable)
                    _outlineComponent.ChangeOutline(state);
                
                StateChanged?.Invoke(this, oldState);
            }
        }

        public void SetHighlighted(bool highlighted)
        {
            _outlineComponent.SetHighlight(highlighted);
        }
    }
}