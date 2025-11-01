using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Skills
{
    [CreateAssetMenu(fileName = "Target Skill Config", menuName = "Configs/Skills/TargetSkillConfig")]
    public class TargetSkillConfig : SkillConfig
    {
        [SerializeField] private UnitState _targetState;
        
        public UnitState TargetState => _targetState;
        
        public override void Accept(ISkillVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}