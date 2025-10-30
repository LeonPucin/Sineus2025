using Game.Gameplay.CameraControls;
using UnityEngine;
using Zenject;

namespace Game.Settings
{
    public class SensitivityChanger : IInitializable
    {
        private readonly ThirdPersonCamera _camera;
        private readonly SettingsData _settingsData;
        private readonly SensitivityConfig _config;

        public float Sensitivity
        {
            get
            {
                return _settingsData.Sensitivity;
            }
            set
            {
                var newVal = value;
                newVal = Mathf.Clamp(newVal, 0.01f, 1);
                _settingsData.Sensitivity = newVal;
                UpdateCameraSpeed();
            }
        }

        public SensitivityChanger(ThirdPersonCamera camera, SettingsData settingsData, SensitivityConfig config)
        {
            _camera = camera;
            _settingsData = settingsData;
            _config = config;
        }
        
        public void Initialize()
        {
            UpdateCameraSpeed();
        }

        private void UpdateCameraSpeed()
        {
            _camera.SetRotationSpeed(Mathf.Lerp(_config.MinValue, _config.MaxValue, _settingsData.Sensitivity));
        }
    }
}