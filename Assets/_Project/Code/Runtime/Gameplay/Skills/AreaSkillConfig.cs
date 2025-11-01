using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Skills
{
    [CreateAssetMenu(fileName = "Area Skill Config", menuName = "Configs/Skills/Area Skill Config")]
    public class AreaSkillConfig : SkillConfig
    {
        [SerializeField] private float _radius;
        [SerializeField] private UnitState[] _skillTargets;

        public float Radius => _radius;
        public UnitState[] SkillTargets => _skillTargets;
        
        public override void Accept(ISkillVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}