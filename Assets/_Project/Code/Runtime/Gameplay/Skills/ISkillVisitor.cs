namespace Gameplay.Skills
{
    public interface ISkillVisitor
    {
        void Visit(TargetSkillConfig config);
        void Visit(AreaSkillConfig config);
    }
}