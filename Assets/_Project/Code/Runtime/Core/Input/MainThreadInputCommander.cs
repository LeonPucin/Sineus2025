using System;
using System.Collections.Generic;
using DoubleDCore.Periphery.Base;
using UnityEngine;

namespace Game.Input
{
    public class MainThreadInputCommander : MonoBehaviour
    {
        private IInputService<InputControls> _inputService;
        
        private readonly Queue<Action> _actions = new();

        [Zenject.Inject]
        private void Init(IInputService<InputControls> inputService)
        {
            _inputService = inputService;
        }
        
        private void Update()
        {
            while (_actions.TryDequeue(out var action))
                action.Invoke();
        }
        
        public void SwitchMap<TMap>() where TMap : Map
        {
            _actions.Enqueue(_inputService.SwitchMap<TMap>);
        }
    }
}