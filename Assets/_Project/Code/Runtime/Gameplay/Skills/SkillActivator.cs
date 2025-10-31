using System;
using System.Collections.Generic;
using Gameplay.UnitCrowd;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Skills
{
    public class SkillActivator : IInitializable, IDisposable
    {
        private readonly SkillUseConfirmator _confirmator;
        private readonly UnitSpawnerConfig _spawnerConfig;

        public SkillActivator(SkillUseConfirmator confirmator, UnitSpawnerConfig spawnerConfig)
        {
            _confirmator = confirmator;
            _spawnerConfig = spawnerConfig;
        }
        
        public void Activate(SkillConfig skill)
        {
            _confirmator.ConfirmUse(skill);
        }

        public void Initialize()
        {
            _confirmator.Confirmed += OnSkillConfirmed;
        }

        private void OnSkillConfirmed(SkillConfig skillConfig, IEnumerable<Unit> units)
        {
            foreach (var unit in units)
            {
                if (unit.CurrentState == skillConfig.TargetState)
                    unit.SetState(UnitState.Normal);
                else
                    unit.SetState(_spawnerConfig.GetRandomBrokenState());
            }
        }

        public void Dispose()
        {
            _confirmator.Confirmed -= OnSkillConfirmed;
        }
    }
}