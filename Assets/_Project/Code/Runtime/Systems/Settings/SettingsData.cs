using System;
using UnityEngine;

namespace Game.Settings
{
    public class SettingsData
    {
        public float MusicVolume = .5f;
        public float SoundVolume = .5f;
        public float Sensitivity = 0.5f;

        public string Key => "settings";
        
        public string GetData()
        {
            return JsonUtility.ToJson(new EncryptData
            {
                MusicVolume = MusicVolume,
                SoundVolume = SoundVolume,
                Sensitivity = Sensitivity
            });
        }

        public string GetDefaultData()
        {
            return JsonUtility.ToJson(new EncryptData
            {
                MusicVolume = .5f,
                SoundVolume = .5f,
                Sensitivity = 0.5f
            });
        }

        public void OnLoad(string data)
        {
            var encryptData = JsonUtility.FromJson<EncryptData>(data);

            MusicVolume = encryptData.MusicVolume;
            SoundVolume = encryptData.SoundVolume;
            Sensitivity = encryptData.Sensitivity;
        }

        [Serializable]
        private class EncryptData
        {
            public float MusicVolume;
            public float SoundVolume;
            public float Sensitivity;
        }
    }
}