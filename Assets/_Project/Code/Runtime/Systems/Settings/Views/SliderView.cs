using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Settings
{
    public class SliderView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _sliderValue;

        public float Value
        {
            get
            {
                return _slider.value;
            }
            set
            {
                _slider.value = value;
                _sliderValue.text = $"{Mathf.RoundToInt(value * 100f)}%";
            }
        }

        public event Action<float> ValueChanged;

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(OnSliderChanged);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.AddListener(OnSliderChanged);
        }

        private void OnSliderChanged(float newVal)
        {
            _sliderValue.text = $"{Mathf.RoundToInt(newVal * 100f)}%";
            ValueChanged?.Invoke(newVal);
        }
    }
}