using System.Collections.Generic;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Skills
{
    public class AreaSkillIndicator : MonoBehaviour
    {
        private const float AREA_VISUAL_MULTIPLIER = 2f;
        
        [SerializeField] private GameObject _indicatorObject;
        [SerializeField] private SkillUseConfirmator _useConfirmator;

        private void OnEnable()
        {
            _useConfirmator.AreaChecked += OnAreaChecked;
            _useConfirmator.Cancelled += OnCancelled;
            _useConfirmator.Confirmed += OnConfirmed;
        }

        private void OnDisable()
        {
            _useConfirmator.AreaChecked -= OnAreaChecked;
            _useConfirmator.Cancelled -= OnCancelled;
            _useConfirmator.Confirmed -= OnConfirmed;
        }
        
        private void OnConfirmed(SkillConfig arg1, IEnumerable<Unit> arg2)
        {
            _indicatorObject.SetActive(false);
        }
        
        private void OnCancelled()
        {
            _indicatorObject.SetActive(false);
        }

        private void OnAreaChecked(bool isSuccess, Vector3 point, float radius)
        {
            _indicatorObject.SetActive(isSuccess);
            
            if (isSuccess)
            {
                _indicatorObject.transform.position = point;
                _indicatorObject.transform.localScale = Vector3.one * radius * AREA_VISUAL_MULTIPLIER;
            }
        }
    }
}