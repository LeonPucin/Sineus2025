using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using Gameplay.Skills;
using UI.Skills;
using UI.Skills.Presenters;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace UI.Pages.SkillsPage
{
    public class SkillsPage : MonoPage, IUIPage
    {
        [SerializeField] private SerializedDictionary<SkillActivateButton, SkillConfig> _skillButtons;
        
        private DiContainer _container;

        [Inject]
        private void Init(DiContainer diContainer)
        {
            _container = diContainer;
        }
        
        public override void Initialize()
        {
            foreach (var (button, skill) in _skillButtons)
            {
                var presenter = _container.Instantiate<SkillButtonPresenter>(new object[] { skill });
                button.Initialize(presenter);
            }
            
            SetCanvasState(true);
        }

        public void Open()
        {
            SetCanvasState(true);
        }

        public override void Close()
        {
            SetCanvasState(false);
        }
    }
}