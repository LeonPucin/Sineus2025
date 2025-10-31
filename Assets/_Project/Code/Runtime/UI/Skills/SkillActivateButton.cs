using DoubleDCore.UI.Base;
using Gameplay.Skills;
using UnityEngine;

namespace UI.Skills
{
    public class SkillActivateButton : ButtonListener
    {
        [SerializeField] private SkillConfig _connectedSkill;
        
        private SkillActivator _skillActivator;

        [Zenject.Inject]
        private void Init(SkillActivator skillActivator)
        {
            _skillActivator = skillActivator;
        }
        
        protected override void OnButtonClicked()
        {
            _skillActivator.Activate(_connectedSkill);
        }
    }
}