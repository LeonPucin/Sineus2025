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

        public UnitState CurrentState { get; private set; } = UnitState.Normal;
        public bool IsInteractable => _isInteractable;

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
    }
}