using TMPro;
using UI.Skills.Presenters;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UI.Skills
{
    public class SkillActivateButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cooldownTime;
        [SerializeField] private Image _cooldownFill;
        [SerializeField] private GameObject _cooldownRoot;
        [SerializeField] private Button _useButton;
        
        private SkillButtonPresenter _presenter;

        public void Initialize(SkillButtonPresenter presenter)
        {
            _presenter = presenter;
            
            _presenter.IsInCooldown.Subscribe((isCooldown) => _cooldownRoot.SetActive(isCooldown)).AddTo(this);
            _presenter.CooldownLeftTitle.Subscribe((time) => _cooldownTime.text = time).AddTo(this);
            _presenter.CooldownLeftPart.Subscribe((part) => _cooldownFill.fillAmount = part).AddTo(this);

            _presenter.ConfirmCommand.BindTo(_useButton).AddTo(this);
        }
    }
}