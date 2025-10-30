using UnityEngine;

namespace Audio
{
    public class AudioSceneService : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private Transform _soundsRoot;

        public AudioSource MusicSource => _musicSource;
        public Transform SoundsRoot => _soundsRoot;
    }
}