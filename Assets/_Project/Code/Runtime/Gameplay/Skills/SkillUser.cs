using System.Linq;
using Gameplay.UnitCrowd;
using Gameplay.Units;

namespace Gameplay.Skills
{
    public class SkillUser : ISkillVisitor
    {
        private readonly UnitSpawnerConfig _spawnerConfig;
        
        private Unit _target;

        public SkillUser(UnitSpawnerConfig spawnerConfig)
        {
            _spawnerConfig = spawnerConfig;
        }

        public void SetTarget(Unit unit)
        {
            _target = unit;
        }
        
        public void Visit(TargetSkillConfig config)
        {
            if (config.TargetState == _target.CurrentState)
                _target.SetState(UnitState.Normal);
            else
                _target.SetState(_spawnerConfig.GetRandomBrokenState());
        }

        public void Visit(AreaSkillConfig config)
        {
            if (config.SkillTargets.Contains(_target.CurrentState))
                _target.SetState(UnitState.Normal);
        }
    }
}