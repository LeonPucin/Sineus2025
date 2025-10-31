using UnityEngine;

namespace Gameplay.Skills
{
    [CreateAssetMenu(fileName = "Target Skill Config", menuName = "Configs/Skills/TargetSkillConfig")]
    public class TargetSkillConfig : SkillConfig
    {
        public override void Accept(ISkillVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}