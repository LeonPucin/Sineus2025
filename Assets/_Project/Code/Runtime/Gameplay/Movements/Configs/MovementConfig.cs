using DoubleDCore.Identification;
using UnityEngine;

namespace Gameplay.Movements
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Configs/MovementConfig")]
    public class MovementConfig : IdentifyingScriptable
    {
        [SerializeField] private AnimationClip _animationClip;
        
        public AnimationClip AnimationClip => _animationClip;
        
        protected override string GetIDPrefix()
        {
            return "movement";
        }
    }
}