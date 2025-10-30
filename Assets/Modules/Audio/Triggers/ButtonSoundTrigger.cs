using System;
using UnityEngine;
using UnityEngine.UI;

namespace Audio.Triggers
{
    [RequireComponent(typeof(Button))]
    public class ButtonSoundTrigger : SoundTrigger
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            PlaySound();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}