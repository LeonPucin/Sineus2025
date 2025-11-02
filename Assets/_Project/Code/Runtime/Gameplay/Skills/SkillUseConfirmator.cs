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
        public event Action Cancelled;
        public event Action Started;
        public event Action<bool, Vector3, float> AreaChecked;

        [Zenject.Inject]
        private void Init(IInputService<InputControls> inputService)
        {
            _inputControls = inputService.GetInputProvider();
            _unitsChooser = new SkillUnitsChooser(100f, _unitMask, _areaMask, _camera);
        }

        private void OnEnable()
        {
            _unitsChooser.AreaChecked += OnAreaChecked;
        }

        private void OnDisable()
        {
            _unitsChooser.AreaChecked -= OnAreaChecked;
        }
        
        private void OnAreaChecked(bool isHit, Vector3 position, float radius)
        {
            AreaChecked?.Invoke(isHit, position, radius);
        }

        public void ConfirmUse(SkillConfig skill)
        {
            if (_currentSkill != null)
                DisableLastHighlight();
            
            _currentSkill = skill;
            _lastResult = null;
            Started?.Invoke();
        }
        
        public void CancelConfirmation()
        {
            if (_currentSkill == null)
                return;
            
            DisableLastHighlight();
            
            Cancelled?.Invoke();
            _currentSkill = null;
        }

        private void Update()
        {
            if (_currentSkill == null)
                return;
            
            if (_inputControls.Character.Aim.WasPerformedThisFrame() ||
                _inputControls.Character.Esc.WasPerformedThisFrame())
            {
                CancelConfirmation();
                return;
            }

            if (_inputControls.Character.Fire.WasPerformedThisFrame())
            {
                if (_lastResult.Count == 0)
                    return;
                
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