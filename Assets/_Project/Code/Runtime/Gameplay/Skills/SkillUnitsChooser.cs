using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Skills
{
    public class SkillUnitsChooser : ISkillVisitor
    {
        private readonly float _maxDistance;
        private readonly LayerMask _unitMask;
        private readonly LayerMask _areaMask;
        private readonly Camera _camera;
        private readonly List<Unit> _chosenUnits = new();

        public event Action<bool, Vector3, float> AreaChecked;

        public SkillUnitsChooser(float maxDistance, LayerMask unitMask, LayerMask areaMask, Camera camera)
        {
            _maxDistance = maxDistance;
            _unitMask = unitMask;
            _areaMask = areaMask;
            _camera = camera;
        }

        public void Reset()
        {
            _chosenUnits.Clear();
        }
        
        public void Visit(TargetSkillConfig config)
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            var isHit = Physics.Raycast(ray, out var hitInfo, _maxDistance, _unitMask);

            if (isHit)
            {
                var unit = hitInfo.collider.GetComponent<Unit>();
                    
                if (unit != null && unit.IsInteractable)
                    _chosenUnits.Add(unit);
            }
        }

        public void Visit(AreaSkillConfig config)
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            var isHit = Physics.Raycast(ray, out var hitInfo, _maxDistance, _areaMask);

            if (isHit)
            {
                var unitColliders = Physics.OverlapSphere(hitInfo.point, config.Radius, _unitMask); //TODO: use non-alloc
                var units = unitColliders.Select(u => u.GetComponent<Unit>()).Where(u => u.IsInteractable);
                _chosenUnits.AddRange(units);
            }
            
            AreaChecked?.Invoke(isHit, hitInfo.point, config.Radius);
        }

        public List<Unit> GetResult()
        {
            return _chosenUnits;
        }
    }
}