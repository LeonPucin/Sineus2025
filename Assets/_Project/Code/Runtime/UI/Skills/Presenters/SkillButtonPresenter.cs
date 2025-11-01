using Cysharp.Threading.Tasks;
using Gameplay.Skills;
using UniRx;
using UnityEngine;

namespace UI.Skills.Presenters
{
    public class SkillButtonPresenter
    {
        private readonly SkillActivator _skillActivator;
        private readonly SkillConfig _connectedSkill;
        private readonly StringReactiveProperty _cooldownLeftTitle = new("");
        private readonly FloatReactiveProperty _cooldownLeftPart = new(0);
        private readonly BoolReactiveProperty _isInCooldown = new(false);
        private readonly BoolReactiveProperty _isNotInCooldown = new(true);
        private readonly CompositeDisposable _disposables = new();
        
        public IReadOnlyReactiveProperty<string> CooldownLeftTitle => _cooldownLeftTitle;
        public IReadOnlyReactiveProperty<float> CooldownLeftPart => _cooldownLeftPart;
        public IReadOnlyReactiveProperty<bool> IsInCooldown => _isInCooldown;
        public ReactiveCommand ConfirmCommand { get; }

        public SkillButtonPresenter(SkillActivator skillActivator, SkillConfig connectedSkill)
        {
            _skillActivator = skillActivator;
            _connectedSkill = connectedSkill;
            
            ConfirmCommand = new ReactiveCommand(_isNotInCooldown);
            ConfirmCommand.Subscribe(ActivateSkill).AddTo(_disposables);
            
            _skillActivator.CooldownStarted += OnCooldownStarted;
            _skillActivator.CooldownEnded += OnCooldownEnded;
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
            _ = UpdateCooldownState();
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

        private async UniTask UpdateCooldownState()
        {
            var waiter = UniTask.Yield();
            float totalCooldown = _connectedSkill.Cooldown;
            
            while (_cooldownLeftPart.Value > 0)
            {
                var cooldownLeft = _skillActivator.GetCooldownLeft(_connectedSkill);
                _cooldownLeftTitle.Value = $"{cooldownLeft:F1}";
                _cooldownLeftPart.Value = cooldownLeft / totalCooldown;
                
                await waiter;
            }
        }
        
        ~SkillButtonPresenter()
        {
            _skillActivator.CooldownStarted -= OnCooldownStarted;
            _skillActivator.CooldownEnded -= OnCooldownEnded;
            _disposables.Dispose();
        }
    }
}