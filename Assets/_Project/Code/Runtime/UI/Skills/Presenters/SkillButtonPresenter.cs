using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DoubleDCore.Periphery.Base;
using Gameplay.Skills;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Skills.Presenters
{
    public class SkillButtonPresenter : IDisposable
    {
        private readonly SkillActivator _skillActivator;
        private readonly SkillConfig _connectedSkill;
        private readonly StringReactiveProperty _cooldownLeftTitle = new("");
        private readonly FloatReactiveProperty _cooldownLeftPart = new(0);
        private readonly BoolReactiveProperty _isInCooldown = new(false);
        private readonly BoolReactiveProperty _isNotInCooldown = new(true);
        private readonly BoolReactiveProperty _isInUse = new(false);
        private readonly CompositeDisposable _disposables = new();
        private readonly Key _alterKey;

        private CancellationTokenSource _cts = new();
        
        public IReadOnlyReactiveProperty<string> CooldownLeftTitle => _cooldownLeftTitle;
        public IReadOnlyReactiveProperty<float> CooldownLeftPart => _cooldownLeftPart;
        public IReadOnlyReactiveProperty<bool> IsInCooldown => _isInCooldown;
        public IReadOnlyReactiveProperty<bool> IsInUse => _isInUse;
        public ReactiveCommand ConfirmCommand { get; }
        public string AlterKeyName { get; }
        public Sprite Icon => _connectedSkill.Icon;

        public SkillButtonPresenter(SkillActivator skillActivator, SkillConfig connectedSkill)
        {
            _skillActivator = skillActivator;
            _connectedSkill = connectedSkill;
            _alterKey = connectedSkill.AlterKey;
            AlterKeyName = _alterKey.ToString();
            
            ConfirmCommand = new ReactiveCommand(_isNotInCooldown);
            ConfirmCommand.Subscribe(ActivateSkill).AddTo(_disposables);
            
            _skillActivator.CooldownStarted += OnCooldownStarted;
            _skillActivator.CooldownEnded += OnCooldownEnded;
            _skillActivator.CurrentSkillChanged += OnCurrentSkillChanged;
        }

        private void OnCurrentSkillChanged(SkillConfig newSkill)
        {
            _isInUse.Value = newSkill == _connectedSkill;
        }

        private void ActivateSkill(Unit _)
        {
            _skillActivator.Activate(_connectedSkill);
        }

        private void OnCooldownStarted(SkillConfig skill, float duration)
        {
            if (skill != _connectedSkill)
                return;
            
            _isInCooldown.Value = true;
            _isNotInCooldown.Value = false;
            _cooldownLeftPart.Value = 1;

            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            _cts = new();
            _ = UpdateCooldownState(_cts.Token);
        }
        
        private void OnCooldownEnded(SkillConfig skill)
        {
            if (skill != _connectedSkill)
                return;
            
            _isInCooldown.Value = false;
            _isNotInCooldown.Value = true;
            _cooldownLeftPart.Value = 0;
            _cooldownLeftTitle.Value = "";
        }

        private async UniTask UpdateCooldownState(CancellationToken token)
        {
            float totalCooldown = _connectedSkill.Cooldown;
            
            while (_cooldownLeftPart.Value > 0 && !token.IsCancellationRequested)
            {
                var cooldownLeft = _skillActivator.GetCooldownLeft(_connectedSkill);
                _cooldownLeftTitle.Value = $"{cooldownLeft:F1}";
                _cooldownLeftPart.Value = cooldownLeft / totalCooldown;
                
                await UniTask.Yield(cancellationToken: token);
            }
        }

        public void Dispose()
        {
            _skillActivator.CooldownStarted -= OnCooldownStarted;
            _skillActivator.CooldownEnded -= OnCooldownEnded;
            _skillActivator.CurrentSkillChanged -= OnCurrentSkillChanged;
            
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
            
            _disposables.Dispose();
        }
    }
}