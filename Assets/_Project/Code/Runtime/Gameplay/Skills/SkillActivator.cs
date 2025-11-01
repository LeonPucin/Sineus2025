using System;
using System.Collections.Generic;
using DoubleDCore.Periphery.Base;
using DoubleDCore.TimeTools;
using Gameplay.UnitCrowd;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Skills
{
    public class SkillActivator : IInitializable, IDisposable
    {
        private readonly SkillUseConfirmator _confirmator;
        private readonly UnitSpawnerConfig _spawnerConfig;
        private readonly SkillUser _skillUser;
        private readonly Dictionary<SkillConfig, Timer> _skillCooldowns = new();
        private readonly InputControls _inputControls;

        private SkillConfig _currentSkill;

        public event Action<SkillConfig, float> CooldownStarted; 
        public event Action<SkillConfig> CooldownEnded;
        public event Action<SkillConfig> CurrentSkillChanged;

        public SkillActivator(SkillUseConfirmator confirmator, UnitSpawnerConfig spawnerConfig,
            IInputService<InputControls> inputService)
        {
            _inputControls = inputService.GetInputProvider();
            _confirmator = confirmator;
            _spawnerConfig = spawnerConfig;
            _skillUser = new SkillUser(_spawnerConfig);
        }
        
        public void Activate(SkillConfig skill)
        {
            if (IsInCooldown(skill))
                return;

            _currentSkill = skill;
            CurrentSkillChanged?.Invoke(_currentSkill);
            _confirmator.ConfirmUse(skill);
        }

        public void Initialize()
        {
            _confirmator.Confirmed += OnSkillConfirmed;
            _inputControls.Character.Esc.performed += OnCancelPerformed;
            _inputControls.Character.Aim.performed += OnCancelPerformed;
        }

        private void OnCancelPerformed(InputAction.CallbackContext obj)
        {
            _currentSkill = null;
            CurrentSkillChanged?.Invoke(_currentSkill);
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
            _inputControls.Character.Esc.performed -= OnCancelPerformed;
            _inputControls.Character.Aim.performed -= OnCancelPerformed;
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
            if (_currentSkill == skill)
                _confirmator.ConfirmUse(skill);
            
            CooldownEnded?.Invoke(skill);
        }
    }
}