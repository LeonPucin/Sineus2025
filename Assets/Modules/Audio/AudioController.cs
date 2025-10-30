using System.Collections.Generic;
using Game.Settings;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class AudioController : IInitializable
    {
        private readonly AudioSource _musicSource;
        private readonly AudioSceneService _audioSceneService;
        private readonly SettingsData _settingsData;
        private readonly List<AudioSource> _soundSources = new();

        public float MusicVolume
        {
            get
            {
                return _settingsData.MusicVolume;
            }
            set
            {
                _settingsData.MusicVolume = Mathf.Clamp01(value);
                UpdateSources();
            }
        }
        public float SoundVolume
        {
            get
            {
                return _settingsData.SoundVolume;
            }
            set
            {
                _settingsData.SoundVolume = Mathf.Clamp01(value);
                UpdateSources();
            }
        }

        public AudioController(AudioSceneService audioSceneService, SettingsData settingsData)
        {
            _musicSource = audioSceneService.MusicSource;
            _audioSceneService = audioSceneService;
            _settingsData = settingsData;
        }

        public void PlaySound(AudioClip clip, float pitch = 1, float volumeScale = 1)
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                if (_soundSources[i].isPlaying == false)
                {
                    _soundSources[i].pitch = pitch;
                    _soundSources[i].PlayOneShot(clip, volumeScale);
                    return;
                }
            }

            var newSoundSource = AddNewSource();
            newSoundSource.pitch = pitch;
            newSoundSource.PlayOneShot(clip, volumeScale);
        }
        
        public void PlaySound(AudioClip clip, Vector3 position, float pitch = 1, float volumeScale = 1)
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                if (_soundSources[i].isPlaying == false)
                {
                    _soundSources[i].transform.position = position;
                    _soundSources[i].pitch = pitch;
                    _soundSources[i].PlayOneShot(clip, volumeScale);
                    return;
                }
            }

            var newSoundSource = AddNewSource();
            newSoundSource.transform.position = position;
            newSoundSource.pitch = pitch;
            newSoundSource.PlayOneShot(clip, volumeScale);
        }

        private AudioSource AddNewSource()
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(_audioSceneService.SoundsRoot);
            gameObject.name = $"SoundSource{_soundSources.Count + 1}";
            var source = gameObject.AddComponent<AudioSource>();
            source.volume = _settingsData.SoundVolume;
            _soundSources.Add(source);
            return source;
        }

        private void UpdateSources()
        {
            _musicSource.volume = _settingsData.MusicVolume;

            foreach (var soundSource in _soundSources)
                soundSource.volume = _settingsData.SoundVolume;
        }

        public void Initialize()
        {
            UpdateSources();
        }
    }
}