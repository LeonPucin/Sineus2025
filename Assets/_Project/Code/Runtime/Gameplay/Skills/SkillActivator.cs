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
        private readonly SkillUser _skillUser;

        public SkillActivator(SkillUseConfirmator confirmator, UnitSpawnerConfig spawnerConfig)
        {
            _confirmator = confirmator;
            _spawnerConfig = spawnerConfig;
            _skillUser = new SkillUser(_spawnerConfig);
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
                _skillUser.SetTarget(unit);
                skillConfig.Accept(_skillUser);
            }
        }

        public void Dispose()
        {
            _confirmator.Confirmed -= OnSkillConfirmed;
        }
    }
}