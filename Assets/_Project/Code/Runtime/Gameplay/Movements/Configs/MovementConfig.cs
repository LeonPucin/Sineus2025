using DoubleDCore.Identification;
using UnityEngine;

namespace Gameplay.Movements
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Configs/MovementConfig")]
    public class MovementConfig : IdentifyingScriptable
    {
        [SerializeField] private AnimationClip _animationClip;
        [SerializeField] private int _difficultyPoints;

        [SerializeField] private string _title;
        [SerializeField] private Sprite _icon;
        [SerializeField] private AudioClip _audioClip;

        public AnimationClip AnimationClip => _animationClip;
        public int DifficultyPoints => _difficultyPoints;

        public string Title => _title;
        public Sprite Icon => _icon;
        public AudioClip AudioClip => _audioClip;

        protected override string GetIDPrefix()
        {
            return "movement";
        }
    }
}