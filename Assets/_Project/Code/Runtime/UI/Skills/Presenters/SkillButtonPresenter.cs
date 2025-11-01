using System.Threading;
using Cysharp.Threading.Tasks;
using DoubleDCore.Periphery.Base;
using Gameplay.Skills;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private readonly BoolReactiveProperty _isInUse = new(false);
        private readonly CompositeDisposable _disposables = new();
        private readonly Key _alterKey;
        private readonly CancellationTokenSource _cts = new();
        private readonly InputControls _inputControls;

        public IReadOnlyReactiveProperty<string> CooldownLeftTitle => _cooldownLeftTitle;
        public IReadOnlyReactiveProperty<float> CooldownLeftPart => _cooldownLeftPart;
        public IReadOnlyReactiveProperty<bool> IsInCooldown => _isInCooldown;
        public IReadOnlyReactiveProperty<bool> IsInUse => _isInUse;
        public ReactiveCommand ConfirmCommand { get; }
        public string AlterKeyName { get; }

        public SkillButtonPresenter(SkillActivator skillActivator, SkillConfig connectedSkill,
            IInputService<InputControls> inputService)
        {
            _inputControls = inputService.GetInputProvider();
            _skillActivator = skillActivator;
            _connectedSkill = connectedSkill;
            _alterKey = connectedSkill.AlterKey;
            AlterKeyName = _alterKey.ToString();
            
            ConfirmCommand = new ReactiveCommand(_isNotInCooldown);
            ConfirmCommand.Subscribe(ActivateSkill).AddTo(_disposables);
            
            _skillActivator.CooldownStarted += OnCooldownStarted;
            _skillActivator.CooldownEnded += OnCooldownEnded;
            _skillActivator.CurrentSkillChanged += OnCurrentSkillChanged;

            _ = ListenForAlterKeyAsync(_cts.Token);
        }

        private void OnCurrentSkillChanged(SkillConfig newSkill)
        {
            _isInUse.Value = newSkill == _connectedSkill;
        }

        private async UniTask ListenForAlterKeyAsync(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                if (_inputControls.Character.enabled == false)
                    continue;
                
                if (Keyboard.current[_alterKey].wasReleasedThisFrame && ConfirmCommand.CanExecute.Value)
                    ConfirmCommand.Execute();
                
                await UniTask.Yield(cancellationToken: token);
            }
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
            float totalCooldown = _connectedSkill.Cooldown;
            
            while (_cooldownLeftPart.Value > 0)
            {
                var cooldownLeft = _skillActivator.GetCooldownLeft(_connectedSkill);
                _cooldownLeftTitle.Value = $"{cooldownLeft:F1}";
                _cooldownLeftPart.Value = cooldownLeft / totalCooldown;
                
                await UniTask.Yield();
            }
        }
        
        ~SkillButtonPresenter()
        {
            _skillActivator.CooldownStarted -= OnCooldownStarted;
            _skillActivator.CooldownEnded -= OnCooldownEnded;
            _skillActivator.CurrentSkillChanged -= OnCurrentSkillChanged;
            _disposables.Dispose();
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}