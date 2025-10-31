using DoubleDCore.Identification;
using UnityEngine;

namespace Gameplay.Movements
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Configs/MovementConfig")]
    public class MovementConfig : IdentifyingScriptable
    {
        [SerializeField] private AnimationClip _animationClip;
        [SerializeField] private int _difficultyPoints;
        
        public AnimationClip AnimationClip => _animationClip;
        public int DifficultyPoints => _difficultyPoints;
        
        protected override string GetIDPrefix()
        {
            return "movement";
        }
    }
}