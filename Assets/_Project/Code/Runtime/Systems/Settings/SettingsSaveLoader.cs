using UnityEngine;

namespace Game.Settings
{
    public class SettingsSaveLoader
    {
        private readonly SettingsData _settingsData;

        public SettingsSaveLoader(SettingsData settingsData)
        {
            _settingsData = settingsData;
        }

        public void Load()
        {
            string data = PlayerPrefs.GetString(_settingsData.Key, _settingsData.GetDefaultData());
            _settingsData.OnLoad(data);
        }

        public void Save()
        {
            string data = _settingsData.GetData();
            PlayerPrefs.SetString(_settingsData.Key, data);
        }
    }
}