using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class LevelStateView : MonoBehaviour
    {
        [SerializeField] private Slider _stateSlider;
        [SerializeField] private float _sliderSpeed = .5f;
        
        private LevelStateViewPresenter _presenter;
        private CompositeDisposable _disposables = new();

        public void Initialize(LevelStateViewPresenter presenter)
        {
            if (_presenter != null)
            {
                _disposables.Dispose();
                _presenter = null;
            }
            
            _presenter = presenter;
            _presenter.NormalUnitsPart
                .Subscribe(OnPartUpdated)
                .AddTo(_disposables);
        }

        private void OnPartUpdated(float part)
        {
            float distance = Mathf.Abs(_stateSlider.value - part);
            float duration = distance / _sliderSpeed;

            _stateSlider.DOKill();
            _stateSlider.DOValue(part, duration);
        }

        private void OnDestroy()
        {
            if (_presenter != null)
            {
                _disposables.Dispose();
                _presenter = null;
            }
        }
    }
}