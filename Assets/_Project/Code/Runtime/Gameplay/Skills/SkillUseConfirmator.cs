using System;
using System.Collections.Generic;
using System.Linq;
using DoubleDCore.Periphery.Base;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Skills
{
    public class SkillUseConfirmator : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _unitMask;
        [SerializeField] private LayerMask _areaMask;
        
        private SkillConfig _currentSkill;
        private InputControls _inputControls;
        private SkillUnitsChooser _unitsChooser;
        private List<Unit> _lastResult;

        public event Action<SkillConfig, IEnumerable<Unit>> Confirmed;

        [Zenject.Inject]
        private void Init(IInputService<InputControls> inputService)
        {
            _inputControls = inputService.GetInputProvider();
            _unitsChooser = new SkillUnitsChooser(100f, _unitMask, _areaMask, _camera);
        }
        
        public void ConfirmUse(SkillConfig skill)
        {
            _currentSkill = skill;
        }

        private void Update()
        {
            if (_currentSkill == null)
                return;
            
            if (_inputControls.Character.Aim.WasPerformedThisFrame() ||
                _inputControls.Character.Esc.WasPerformedThisFrame())
            {
                _currentSkill = null;
                DisableLastHighlight();
                return;
            }

            if (_inputControls.Character.Fire.WasPerformedThisFrame())
            {
                DisableLastHighlight();
                Confirmed?.Invoke(_currentSkill, _lastResult);
                _currentSkill = null;
            }
        }

        private void FixedUpdate()
        {
            if (_currentSkill == null)
                return;
            
            _unitsChooser.Reset();
            _currentSkill.Accept(_unitsChooser);
            var result = _unitsChooser.GetResult();

            DisableLastHighlight();
            
            foreach (var unit in result)
                unit.SetHighlighted(true);
            
            _lastResult = result.ToList();
        }

        private void DisableLastHighlight()
        {
            if (_lastResult != null)
            {
                foreach (var unit in _lastResult)
                    unit.SetHighlighted(false);
            }
        }
    }
}