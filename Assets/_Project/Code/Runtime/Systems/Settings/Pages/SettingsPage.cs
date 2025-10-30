using Audio;
using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Settings
{
    public class SettingsPage : MonoPage, IUIPage, ISingleOpenablePage
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundsSlider;
        [SerializeField] private Slider _sensitivitySlider;
        
        private AudioController _audioController;
        private SettingsSaveLoader _settingsSaveLoader;
        private SensitivityChanger _sensitivityChanger;

        [Inject]
        private void Init(AudioController audioController, SensitivityChanger sensitivityChanger,
            SettingsSaveLoader settingsSaveLoader)
        {
            _audioController = audioController;
            _settingsSaveLoader = settingsSaveLoader;
            _sensitivityChanger = sensitivityChanger;
        }
        
        public void Open()
        {
            _musicSlider.value = _audioController.MusicVolume;
            _soundsSlider.value = _audioController.SoundVolume;
            _sensitivitySlider.value = _sensitivityChanger.Sensitivity;

            _musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
            _soundsSlider.onValueChanged.AddListener(OnSoundValueChanged);
            _sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
            
            SetCanvasState(true);
        }

        private void OnSensitivityChanged(float val)
        {
            _sensitivityChanger.Sensitivity = val;
            _settingsSaveLoader.Save();
        }

        private void OnMusicValueChanged(float val)
        {
            _audioController.MusicVolume = val;
            _settingsSaveLoader.Save();
        }
        
        private void OnSoundValueChanged(float val)
        {
            _audioController.SoundVolume = val;
            _settingsSaveLoader.Save();
        }

        public override void Close()
        {
            _musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
            _soundsSlider.onValueChanged.RemoveListener(OnSoundValueChanged);
            _sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
            
            SetCanvasState(false);
        }
    }
}