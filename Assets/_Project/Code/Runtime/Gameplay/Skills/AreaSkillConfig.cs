using UnityEngine;

namespace Gameplay.Skills
{
    [CreateAssetMenu(fileName = "Area Skill Config", menuName = "Configs/Skills/Area Skill Config")]
    public class AreaSkillConfig : SkillConfig
    {
        [SerializeField] private float _radius;

        public float Radius => _radius;
        
        public override void Accept(ISkillVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}