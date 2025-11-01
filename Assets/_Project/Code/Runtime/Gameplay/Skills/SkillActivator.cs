using System;
using System.Collections.Generic;
using DoubleDCore.TimeTools;
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
        private readonly Dictionary<SkillConfig, Timer> _skillCooldowns = new();

        public event Action<SkillConfig, float> CooldownStarted; 
        public event Action<SkillConfig> CooldownEnded;

        public SkillActivator(SkillUseConfirmator confirmator, UnitSpawnerConfig spawnerConfig)
        {
            _confirmator = confirmator;
            _spawnerConfig = spawnerConfig;
            _skillUser = new SkillUser(_spawnerConfig);
        }
        
        public void Activate(SkillConfig skill)
        {
            if (IsInCooldown(skill))
                return;
            
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
            
            StartCooldown(skillConfig);
        }

        public void Dispose()
        {
            _confirmator.Confirmed -= OnSkillConfirmed;
        }

        public float GetCooldownLeft(SkillConfig skill)
        {
            if (_skillCooldowns.ContainsKey(skill) == false)
                _skillCooldowns[skill] = new Timer(TimeBindingType.ScaledTime);

            var timer = _skillCooldowns[skill];
            return timer.RemainingTime;
        }

        public bool IsInCooldown(SkillConfig skill)
        {
            if (_skillCooldowns.ContainsKey(skill) == false)
                _skillCooldowns[skill] = new Timer(TimeBindingType.ScaledTime);
            
            var timer = _skillCooldowns[skill];
            return timer.IsWorking;
        }

        private void StartCooldown(SkillConfig skill)
        {
            if (_skillCooldowns.ContainsKey(skill) == false)
                _skillCooldowns[skill] = new Timer(TimeBindingType.ScaledTime);

            var timer = _skillCooldowns[skill];
            _ = timer.Start(skill.Cooldown, () => OnCooldownEnded(skill));
            CooldownStarted?.Invoke(skill, skill.Cooldown);
        }

        private void OnCooldownEnded(SkillConfig skill)
        {
            CooldownEnded?.Invoke(skill);
        }
    }
}