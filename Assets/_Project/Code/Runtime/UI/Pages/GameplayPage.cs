using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using Gameplay.Skills;
using UI.Gameplay;
using UI.Skills;
using UI.Skills.Presenters;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace UI.Pages.SkillsPage
{
    public class GameplayPage : MonoPage, IUIPage
    {
        [SerializeField] private SerializedDictionary<SkillActivateButton, SkillConfig> _skillButtons;
        [SerializeField] private LevelTimerView _timerText;
        [SerializeField] private LevelStateView _stateView;
        
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
            var stateViewPresenter = _container.Instantiate<LevelStateViewPresenter>();
            var timerViewPresenter = _container.Instantiate<LevelTimerViewPresenter>();
            
            _stateView.Initialize(stateViewPresenter);
            _timerText.Initialize(timerViewPresenter);
            
            SetCanvasState(true);
        }

        public override void Close()
        {
            SetCanvasState(false);
        }
    }
}