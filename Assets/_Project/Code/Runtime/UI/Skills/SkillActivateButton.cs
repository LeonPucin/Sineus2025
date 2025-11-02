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
        [SerializeField] private TMP_Text _alterKey;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _glow;
        [SerializeField] private Color _highlightColor = Color.yellow;
        
        private SkillButtonPresenter _presenter;
        private Color _defaultColor;

        public void Initialize(SkillButtonPresenter presenter)
        {
            _defaultColor = _icon.color;
            _presenter = presenter;
            _alterKey.text = presenter.AlterKeyName;

            _icon.sprite = presenter.Icon;
            
            _presenter.IsInCooldown.Subscribe((isCooldown) => _cooldownRoot.SetActive(isCooldown)).AddTo(this);
            _presenter.CooldownLeftTitle.Subscribe((time) => _cooldownTime.text = time).AddTo(this);
            _presenter.CooldownLeftPart.Subscribe((part) => _cooldownFill.fillAmount = part).AddTo(this);
            _presenter.IsInUse.Subscribe((isInUse) =>
                {
                    _icon.color = isInUse ? _highlightColor : _defaultColor;
                    _glow.color = _highlightColor;
                    _glow.enabled = isInUse;
                }).AddTo(this);

            _presenter.ConfirmCommand.BindTo(_useButton).AddTo(this);
        }
    }
}