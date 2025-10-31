using System;
using DoubleDCore.Periphery.Base;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Input
{
    public class InputDisabler : MonoBehaviour
    {
        [Tooltip("name or path to action")]
        [SerializeField] private string _actionName;

        private InputControls _inputControls;
        private InputAction _action;

        [Inject]
        private void Init(IInputService<InputControls> inputServices)
        {
            _inputControls = inputServices.GetInputProvider();
        }

        private void Start()
        {
            _action = _inputControls.FindAction(_actionName);
        }

        public void DisableAction()
        {
            _action.Disable();
        }

        public void EnableAction()
        {
            _action.Enable();
        }
    }
}