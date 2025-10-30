using Game.Utility;
using UnityEngine;
using Zenject;

namespace Audio.Triggers
{
    public abstract class SoundTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private RangedValue _pitchValue = new RangedValue { MinValue = 1, MaxValue = 1 };
        [SerializeField] private float _volumeScale = 1f;
        
        private AudioController _audioController;

        [Inject]
        private void Init(AudioController audioController)
        {
            _audioController = audioController;
        }

        public void SetClip(AudioClip clip)
        {
            _clip = clip;
        }

        protected void PlaySound()
        {
            _audioController.PlaySound(_clip, 
                Random.Range(_pitchValue.MinValue, _pitchValue.MaxValue), _volumeScale);
        }

        protected void PlaySound(Vector3 position)
        {
            _audioController.PlaySound(_clip, position, 
                Random.Range(_pitchValue.MinValue, _pitchValue.MaxValue), _volumeScale);
        }
    }
}